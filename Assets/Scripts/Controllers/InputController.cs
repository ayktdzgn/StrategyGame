using System.Collections;
using System.Collections.Generic;
using Core.Grid;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    IPublisher<OnSelectEvent<ISelectable>> onObjectSelected = new Publisher<OnSelectEvent<ISelectable>>();
    public IPublisher<OnSelectEvent<ISelectable>> OnObjectSelected { get { return onObjectSelected; } }

    IPublisher<Vector2Int> onGetPointPosition = new Publisher<Vector2Int>();
    public IPublisher<Vector2Int> OnGetPointPosition { get => onGetPointPosition; }


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
        }
    }
}

public class OnSelectEvent<T> where T: ISelectable
{
    public T selectedObject;

    public OnSelectEvent(T selectedObject)
    {
        this.selectedObject = selectedObject;
    }
}
