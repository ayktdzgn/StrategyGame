using System.Collections;
using System.Collections.Generic;
using Core.Interfaces;
using Core.PublishSubscribe;
using UnityEngine;

namespace Core.Controllers
{
    public class MobileCommandController : MonoBehaviour
    {
        List<IMobile> _mobileObjectList = new List<IMobile>();

        Subscriber<OnSelectEvent<ISelectable>> onObjectSelectSubscriber;
        Subscriber<Vector2Int> onMovePointSetter;

        private void Start()
        {
            onObjectSelectSubscriber = new Subscriber<OnSelectEvent<ISelectable>>(GameController.Instance.InputController.OnObjectSelected);
            onObjectSelectSubscriber.Publisher.MessagePublisher += SetSelectedObjects;

            onMovePointSetter = new Subscriber<Vector2Int>(GameController.Instance.InputController.OnGetPointPosition);
            onMovePointSetter.Publisher.MessagePublisher += MoveUnitsSelectedPoint;
        }
        //Set IMobile list and Set selected color if it selected
        private void SetSelectedObjects(object sender, Message<OnSelectEvent<ISelectable>> message)
        {
            if (message.GenericMessage.selectedObject == null) { ClearSelectedMobileList(); return; }
            if (message.GenericMessage.selectedObject is IMobile mobile)
            {
                if (!_mobileObjectList.Contains(mobile))
                {
                    _mobileObjectList.Add(mobile);
                    mobile.SetSelectedColor(true);
                }
                else
                {
                    _mobileObjectList.Remove(mobile);
                    mobile.SetSelectedColor(false);
                }
            }
            else
            {
                ClearSelectedMobileList();
            }
        }
        //Set IMobile list's move destination
        private void MoveUnitsSelectedPoint(object sender, Message<Vector2Int> message)
        {
            RefreshMobileList();
            for (int i = 0; i < _mobileObjectList.Count; i++)
            {
                _mobileObjectList[i].Move(message.GenericMessage);
            }
        }
        //Refresh mobile list by if it is alive
        private void RefreshMobileList()
        {
            for (int i = 0; i < _mobileObjectList.Count; i++)
            {
                if (_mobileObjectList[i] is IAttackable attackable && !attackable.IsAlive)
                    _mobileObjectList.RemoveAt(i);
            }
        }
        //Cleare IMobile List and Set unselected color
        private void ClearSelectedMobileList()
        {
            for (int i = 0; i < _mobileObjectList.Count; i++)
            {
                _mobileObjectList[i].SetSelectedColor(false);
            }
            _mobileObjectList.Clear();
        }
    }
}
