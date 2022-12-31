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

    private void SetSelectedObject(object sender, Message<OnSelectEvent<ISelectable>> message)
    {
        if (_selectedBuilding != null){ SetProducerSpawnPointLocator(false);}
        if (message.GenericMessage.selectedObject == null) { SetProducerSpawnPointLocator(false); _selectedBuilding = null; return; }

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
}
