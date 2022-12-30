using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    Building _selectedProducerBuilding;

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
        if (e.GenericMessage.selectedObject == null) { return; }
        if(_selectedProducerBuilding != null)
        {
            if (_selectedProducerBuilding is IProducer)
            {
                ((IProducer)_selectedProducerBuilding).SpawnPointLocatorStatus(false);
            }
        }

        if (e.GenericMessage.selectedObject is Building)
        {
            _selectedProducerBuilding = ((Building)e.GenericMessage.selectedObject);
            if (_selectedProducerBuilding is IProducer)
            {
                ((IProducer)_selectedProducerBuilding).SpawnPointLocatorStatus(true);
            }
        }
    }

    void SetSpawnPoint(object sender, Message<Vector2Int> e)
    {
        if (_selectedProducerBuilding != null && _selectedProducerBuilding is IProducer)
        {
            if (GameController.Instance.GridController.GetGridAvailability(e.GenericMessage, _selectedProducerBuilding.Width, _selectedProducerBuilding.Height))
            {
                ((IProducer)_selectedProducerBuilding).SetSpawnPointLocatorPosition(e.GenericMessage);
            }
        }
    }
}
