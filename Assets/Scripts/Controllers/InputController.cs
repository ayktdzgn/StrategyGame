using System.Collections;
using System.Collections.Generic;
using Core.Grid;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    [SerializeField] LayerMask _layerMask;
    bool _isCarryingBuilding = false;
    Building _building;

    public bool IsCarryingBuilding { get => _isCarryingBuilding; set => _isCarryingBuilding = value; }

    IPublisher<OnSelectEvent<ISelectable>> onObjectSelected = new Publisher<OnSelectEvent<ISelectable>>();
    public IPublisher<OnSelectEvent<ISelectable>> OnObjectSelected { get { return onObjectSelected; } }

    IPublisher<Vector2Int> onGetPointPosition = new Publisher<Vector2Int>();
    public IPublisher<Vector2Int> OnGetPointPosition { get => onGetPointPosition; }


    public void InputUpdate()
    {
        Vector2 raycastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(raycastPosition, Vector2.zero, _layerMask);

        if (hit.collider == null || EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            PlantBuilding(raycastPosition);

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

    public void CarryBuilding(ref Building building)
    {
        _isCarryingBuilding = true;
        _building = building;
        StartCoroutine(MoveBuilding(building));
    }

    void PlantBuilding(Vector2 mousePos)
    {
        if (_building == null) return;
        var pos = new Vector2Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));
        if (GameController.Instance.GridController.GetGridAvailability(pos, _building.Width, _building.Height))
        {
            GameController.Instance.GridController.SetGridBuilt(pos, _building.Width, _building.Height);
            _building.transform.position = new Vector3(pos.x,pos.y,-1);

            _isCarryingBuilding = false;
            _building = null;
        }
    }

    IEnumerator MoveBuilding(Building building)
    {
        while (_isCarryingBuilding)
        {
            var mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            building.transform.position = mousePos;

            var pos = new Vector2Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y)); // Calculate the grid position

            if (GameController.Instance.GridController.GetGridAvailability(pos,building.Width,building.Height))
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

public class OnSelectEvent<T> where T: ISelectable
{
    public T selectedObject;

    public OnSelectEvent(T selectedObject)
    {
        this.selectedObject = selectedObject;
    }
}
