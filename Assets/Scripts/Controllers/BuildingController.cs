using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    Building _selectedBuilding;

    Subscriber<OnSelectEvent<ISelectable>> onObjectSelectSubscriber;
    Subscriber<Vector2Int> onSpawnPointSetter;

    private void Start()
    {
        onObjectSelectSubscriber = new Subscriber<OnSelectEvent<ISelectable>>(GameController.Instance.InputController.OnObjectSelected);
        onObjectSelectSubscriber.Publisher.MessagePublisher += SetSelectedObject;

        onSpawnPointSetter = new Subscriber<Vector2Int>(GameController.Instance.InputController.OnGetPointPosition);
        onSpawnPointSetter.Publisher.MessagePublisher += SetSpawnPoint;
    }

    void SetSelectedObject(object sender, Message<OnSelectEvent<ISelectable>> e)
    {
        if (_selectedBuilding != null){ SetProducerSpawnPointLocator(false);}
        if (e.GenericMessage.selectedObject == null) { SetProducerSpawnPointLocator(false); _selectedBuilding = null; return; }

        if (e.GenericMessage.selectedObject is Building)
        {
            _selectedBuilding = ((Building)e.GenericMessage.selectedObject);
            SetProducerSpawnPointLocator(true);
        }
        else
        {
            SetProducerSpawnPointLocator(false);
            _selectedBuilding = null;
        }
    }

    void SetSpawnPoint(object sender, Message<Vector2Int> e)
    {
        if (_selectedBuilding != null && _selectedBuilding is IProducer)
        {
            if (GameController.Instance.GridController.GetGridAvailability(e.GenericMessage, ((IProducer)_selectedBuilding).SpawnPointLocatorSize.x, ((IProducer)_selectedBuilding).SpawnPointLocatorSize.y))
            {
                ((IProducer)_selectedBuilding).SetSpawnPointLocatorPosition(e.GenericMessage);
            }
        }
    }

    void SetProducerSpawnPointLocator(bool status)
    {
        if (_selectedBuilding is IProducer)
        {
            ((IProducer)_selectedBuilding).SpawnPointLocatorStatus(status);
        }
    }
}
