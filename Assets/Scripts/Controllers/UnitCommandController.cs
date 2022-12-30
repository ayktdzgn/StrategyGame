using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCommandController : MonoBehaviour
{
    List<IMobile> _mobileObjectList = new List<IMobile>();

    Subscriber<OnSelectEvent<ISelectable>> onObjectSelectSubscriber;

    private void Start()
    {
        onObjectSelectSubscriber = new Subscriber<OnSelectEvent<ISelectable>>(GameController.Instance.InputController.OnObjectSelected);
        onObjectSelectSubscriber.Publisher.MessagePublisher += SetSelectedObjects;
    }

    void SetSelectedObjects(object sender, Message<OnSelectEvent<ISelectable>> e)
    {
        if (e.GenericMessage.selectedObject == null) { ClearSelectedMobileList();  return; }
        if (e.GenericMessage.selectedObject is IMobile)
        {
            if (!_mobileObjectList.Contains((IMobile)e.GenericMessage.selectedObject))
            {
                _mobileObjectList.Add((IMobile)e.GenericMessage.selectedObject);
                ((IMobile)e.GenericMessage.selectedObject).SetSelectedColor(true);
            }
            else
            {
                _mobileObjectList.Remove((IMobile)e.GenericMessage.selectedObject);
                ((IMobile)e.GenericMessage.selectedObject).SetSelectedColor(false);
            }
        }
    }

    void ClearSelectedMobileList()
    {
        for (int i = 0; i < _mobileObjectList.Count; i++)
        {
            _mobileObjectList[i].SetSelectedColor(false);
        }
        _mobileObjectList.Clear();
    }
}
