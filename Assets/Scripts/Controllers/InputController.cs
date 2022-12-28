using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    bool _isCarryingBuilding = false;
    Building _building;

    public bool IsCarryingBuilding { get => _isCarryingBuilding; set => _isCarryingBuilding = value; }

    private void Start()
    {
        
    }

    public void InputUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 raycastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(raycastPosition, Vector2.zero);

            if (hit.collider != null)
            {
               
            }
        }
    }

    public void BuildingCarry(ref Building building)
    {
        _isCarryingBuilding = true;
        _building = building;
        StartCoroutine(MoveBuilding(building));
    }

    IEnumerator MoveBuilding(Building building)
    {
        while (_isCarryingBuilding)
        {
            //building.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var pos = new Vector2Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y)); // Calculate the grid position

            if (GameController.Instance.GridController.GetGridAvailability(building.Width,building.Height))
            {
                building.GetComponent<SpriteRenderer>().color = Color.white;

                if (Input.GetMouseButtonDown(0)) // Left Click; Place the selected prefab
                {
                    GameController.Instance.GridController.SetGridBuilt(building.Width, building.Height);
                    building.transform.position = (Vector2)pos;

                    _isCarryingBuilding = false;
                    _building = null;
                }
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
