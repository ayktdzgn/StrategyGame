using System.Collections;
using System.Collections.Generic;
using Core.Grid;
using Core.Interfaces;
using Core.PublishSubscribe;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Controllers
{
    public class InputController : MonoBehaviour
    {
        IPublisher<OnSelectEvent<ISelectable>> onObjectSelected = new Publisher<OnSelectEvent<ISelectable>>();
        public IPublisher<OnSelectEvent<ISelectable>> OnObjectSelected { get { return onObjectSelected; } }

        IPublisher<OnAttackableSelectEvent<IAttackable>> onAttackableObjectSelected = new Publisher<OnAttackableSelectEvent<IAttackable>>();
        public IPublisher<OnAttackableSelectEvent<IAttackable>> OnAttackableObjectSelected { get { return onAttackableObjectSelected; } }

        IPublisher<Vector2Int> onGetPointPosition = new Publisher<Vector2Int>();
        public IPublisher<Vector2Int> OnGetPointPosition { get => onGetPointPosition; }

        //Check mouse click and check raycast. Publish events with these cases
        public void InputUpdate()
        {
            Vector2 raycastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(raycastPosition, Vector2.zero);

            if (hit.collider == null || EventSystem.current.IsPointerOverGameObject())
                return;

            if (Input.GetMouseButtonDown(0))
            {
                var selectableObject = hit.collider.GetComponent<ISelectable>();
                if (OnObjectSelected != null)
                    OnObjectSelected.Publish(new OnSelectEvent<ISelectable>(selectableObject));
            }
            if (Input.GetMouseButtonDown(1))
            {
                var pos = new Vector2Int(Mathf.RoundToInt(raycastPosition.x), Mathf.RoundToInt(raycastPosition.y));
                if (OnGetPointPosition != null)
                    onGetPointPosition.Publish(pos);

                var attackableObject = hit.collider.GetComponent<IAttackable>();
                if (OnAttackableObjectSelected != null)
                    OnAttackableObjectSelected.Publish(new OnAttackableSelectEvent<IAttackable>(attackableObject));
            }
        }
    }
    //Hold ISelectable object
    public class OnSelectEvent<T> where T : ISelectable
    {
        public T selectedObject;

        public OnSelectEvent(T selectedObject)
        {
            this.selectedObject = selectedObject;
        }
    }
    //Hold IAttackable object
    public class OnAttackableSelectEvent<T> where T : IAttackable
    {
        public T selectedObject;

        public OnAttackableSelectEvent(T selectedObject)
        {
            this.selectedObject = selectedObject;
        }
    }
}
