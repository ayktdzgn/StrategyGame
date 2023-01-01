using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    Building _selectedBuilding;
    bool _isCarryingBuilding = false;

    Subscriber<OnSelectEvent<ISelectable>> onObjectSelectSubscriber;
    Subscriber<Vector2Int> onSpawnPointSetter;

    public bool IsCarryingBuilding { get => _isCarryingBuilding; set => _isCarryingBuilding = value; }

    private void Start()
    {
        onObjectSelectSubscriber = new Subscriber<OnSelectEvent<ISelectable>>(GameController.Instance.InputController.OnObjectSelected);
        onObjectSelectSubscriber.Publisher.MessagePublisher += SetSelectedObject;

        onSpawnPointSetter = new Subscriber<Vector2Int>(GameController.Instance.InputController.OnGetPointPosition);
        onSpawnPointSetter.Publisher.MessagePublisher += SetSpawnPoint;
    }

    private void SetSelectedObject(object sender, Message<OnSelectEvent<ISelectable>> message)
    {
        if (_selectedBuilding != null){ SetProducerSpawnPointLocator(false);}
        if (message.GenericMessage.selectedObject == null)
        {
            SetProducerSpawnPointLocator(false);
            _selectedBuilding = null;
            return;
        }

        PlantBuilding((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if (message.GenericMessage.selectedObject is Building building)
        {
            _selectedBuilding = building;
            SetProducerSpawnPointLocator(true);
        }
        else
        {
            SetProducerSpawnPointLocator(false);
            _selectedBuilding = null;
        }
    }

    private void SetSpawnPoint(object sender, Message<Vector2Int> message)
    {
        if (_selectedBuilding != null && _selectedBuilding is IProducer producerBuilding)
        {
            if (GameController.Instance.GridController.GetGridAvailability(message.GenericMessage, producerBuilding.SpawnPointLocatorSize.x, producerBuilding.SpawnPointLocatorSize.y))
            {
                producerBuilding.SetSpawnPointLocatorPosition(message.GenericMessage);
            }
        }
    }

    private void SetProducerSpawnPointLocator(bool status)
    {
        if (_selectedBuilding is IProducer producerBuilding)
        {
            producerBuilding.SpawnPointLocatorStatus(status);
        }
    }

    public void CarryBuilding(string buildName)
    {
        if (_isCarryingBuilding) return;
        Building building = BuildingFactory.Instance.GetNewProduct(buildName);
        CarryBuilding(building);
    }

    private void CarryBuilding(Building building)
    {
        _isCarryingBuilding = true;
        _selectedBuilding = building;
        StartCoroutine(MoveBuilding(building));
    }

    private void PlantBuilding(Vector2 mousePos)
    {
        if (_selectedBuilding == null) return;
        var pos = new Vector2Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));
        if (GameController.Instance.GridController.GetGridAvailability(pos, _selectedBuilding.Width, _selectedBuilding.Height))
        {
            GameController.Instance.GridController.SetGridBuilt(pos, _selectedBuilding.Width, _selectedBuilding.Height);
            _selectedBuilding.transform.position = new Vector2(pos.x, pos.y);

            _isCarryingBuilding = false;
            _selectedBuilding = null;
        }
    }

    IEnumerator MoveBuilding(Building building)
    {
        while (_isCarryingBuilding)
        {
            var mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            building.transform.position = mousePos;

            var pos = new Vector2Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y)); // Calculate the grid position

            if (GameController.Instance.GridController.GetGridAvailability(pos, building.Width, building.Height))
            {
                building.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                building.GetComponent<SpriteRenderer>().color = Color.red;
            }

            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
