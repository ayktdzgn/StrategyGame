using System;
using System.Collections;
using System.Collections.Generic;
using Core.Pathfinding;
using UnityEngine;

public class Unit : Entity, IProduct, IMobile
{
    [SerializeField] Color _selectedColor;

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

        Debug.Log("current pos : " + transform.position + " - Destination : " + destination);

        foreach (var item in destinationPath)
        {
            Debug.Log(item);
        }

        if (destinationPath != null && destinationPath.Count > 1)
        {
            destinationPath.RemoveAt(0);
        }
        Move(40,0,destinationPath);
    }

    void Move(float speed, int currentPathIndex, List<Vector3> pathVectorList)
    {
        var movementHandler = new MovementHandler(speed,currentPathIndex,pathVectorList);
        StartCoroutine(movementHandler.Movement(transform));
    }
}

public class MovementHandler
{
    private float _speed = 40f;
    private int _currentPathIndex;
    private List<Vector3> _pathVectorList = new List<Vector3>();

    public MovementHandler(float speed, int currentPathIndex , List<Vector3> pathVectorList)
    {
        _speed = speed;
        _currentPathIndex = currentPathIndex;
        _pathVectorList = pathVectorList;
    }

    private void StopMoving()
    {
        _pathVectorList = null;
    }

    public IEnumerator Movement(Transform mobileTransform)
    {
        while (_pathVectorList != null)
        {
            Vector3 targetPosition = _pathVectorList[_currentPathIndex];
            if (Vector3.Distance(mobileTransform.position, targetPosition) > 0.03f)
            {
                Vector3 moveDir = (targetPosition - mobileTransform.position).normalized;

                float distanceBefore = Vector3.Distance(mobileTransform.position, targetPosition);
                mobileTransform.position = mobileTransform.position + moveDir * _speed * Time.deltaTime;
            }
            else
            {
                _currentPathIndex++;
                if (_currentPathIndex >= _pathVectorList.Count)
                {
                    mobileTransform.position = targetPosition;
                    StopMoving();
                }
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
