using System;
using System.Collections;
using System.Collections.Generic;
using Core.Pathfinding;
using UnityEngine;

public class Unit : Entity, IProduct, IMobile
{
    [SerializeField] Color _selectedColor;
    IEnumerator _movement;

    public void SetSelectedColor(bool status)
    {
        if (status)
            _spriteRenderer.color = _selectedColor;
        else
            _spriteRenderer.color = Color.white;
    }

    public void Move(Vector2Int destination)
    {
        var destinationPath = Pathfinding.Instance.FindPath(transform.position, (Vector2)destination);

        if (destinationPath != null && destinationPath.Count > 1)
        {
            destinationPath.RemoveAt(0);
        }
        Move(40,0,destinationPath);
    }

    private void Move(float speed, int currentPathIndex, List<Vector3> pathVectorList)
    {
        if (_movement != null) StopCoroutine(_movement);
        var movementHandler = new MovementHandler(speed,currentPathIndex,pathVectorList);
        _movement = movementHandler.Movement(transform);
        StartCoroutine(_movement);
    }
}
