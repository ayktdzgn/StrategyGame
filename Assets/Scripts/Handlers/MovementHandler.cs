
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler
{
    private float _speed = 40f;
    private int _currentPathIndex;
    private List<Vector3> _pathVectorList = new List<Vector3>();

    public MovementHandler(float speed, int currentPathIndex, List<Vector3> pathVectorList)
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
                yield return new WaitForEndOfFrame();
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
