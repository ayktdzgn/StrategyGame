using System.Collections;
using System.Collections.Generic;
using Core.Interfaces;
using Core.PublishSubscribe;
using UnityEngine;

namespace Core.Controllers
{
    public class AttackCommandController : MonoBehaviour
    {
        Subscriber<OnSelectEvent<ISelectable>> onSelectSubscriber;
        Subscriber<OnAttackableSelectEvent<IAttackable>> onAttackableSelectSubscriber;

        List<IAttacker> _attackerObjectList = new List<IAttacker>();
        IAttackable _targetAttackable;

        private void Start()
        {
            onSelectSubscriber = new Subscriber<OnSelectEvent<ISelectable>>(GameController.Instance.InputController.OnObjectSelected);
            onSelectSubscriber.Publisher.MessagePublisher += SetAttackerObjects;

            onAttackableSelectSubscriber = new Subscriber<OnAttackableSelectEvent<IAttackable>>(GameController.Instance.InputController.OnAttackableObjectSelected);
            onAttackableSelectSubscriber.Publisher.MessagePublisher += SetAttackableSelectedObjects;
        }
        /// <summary>
        /// Set Attacker List with Publisher's events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void SetAttackerObjects(object sender, Message<OnSelectEvent<ISelectable>> message)
        {
            if (message.GenericMessage.selectedObject == null) { ClearSelectedAttackerList(); return; }
            if (message.GenericMessage.selectedObject is IAttacker attacker)
            {
                if (!_attackerObjectList.Contains(attacker))
                {
                    _attackerObjectList.Add(attacker);
                }
                else
                {
                    _attackerObjectList.Remove(attacker);
                }
            }
            else
            {
                ClearSelectedAttackerList();
            }
        }
        /// <summary>
        /// Set Attackable objects command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void SetAttackableSelectedObjects(object sender, Message<OnAttackableSelectEvent<IAttackable>> message)
        {
            if (message.GenericMessage.selectedObject != null)
            {
                _targetAttackable = message.GenericMessage.selectedObject;

                if (_targetAttackable is IAttacker attacker && _attackerObjectList.Contains(attacker))
                {
                    _attackerObjectList.Remove(attacker);
                }

                RefreshAttackerList();
                for (int i = 0; i < _attackerObjectList.Count; i++)
                {
                    if (_targetAttackable.IsAlive)
                        _attackerObjectList[i].Attack(_targetAttackable);
                }
            }
            else
            {
                _targetAttackable = null;
                for (int i = 0; i < _attackerObjectList.Count; i++)
                {
                    _attackerObjectList[i].StopAttack();
                }
            }
        }

        /// <summary>
        /// Refresh Attacker List base on is it alive
        /// </summary>
        private void RefreshAttackerList()
        {
            for (int i = 0; i < _attackerObjectList.Count; i++)
            {
                if (_attackerObjectList[i] is IAttackable attackable && !attackable.IsAlive)
                    _attackerObjectList.RemoveAt(i);
            }
        }
        //Clear Attacker list and set target null
        private void ClearSelectedAttackerList()
        {
            _targetAttackable = null;
            _attackerObjectList.Clear();
        }
    }
}
