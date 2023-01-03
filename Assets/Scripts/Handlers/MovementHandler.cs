using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Handlers
{
    public class MovementHandler
    {
        private float _speed = 40f;
        private int _currentPathIndex;
        private List<Vector3> _pathVectorList = new List<Vector3>();
        private Action _stopAction;

        /// <summary>
        /// Handle each objects movement
        /// </summary>
        /// <param name="speed">movement speed</param>
        /// <param name="currentPathIndex">movement path start index</param>
        /// <param name="pathVectorList">movement path</param>
        /// <param name="stopAction">Do something(Action) when it finish</param>
        public MovementHandler(float speed, int currentPathIndex, List<Vector3> pathVectorList, Action stopAction = null)
        {
            _speed = speed;
            _currentPathIndex = currentPathIndex;
            _pathVectorList = pathVectorList;
            _stopAction = stopAction;
        }

        private void StopMoving()
        {
            _pathVectorList = null;
            _stopAction?.Invoke();
        }

        /// <summary>
        /// Movement Coroutine
        /// </summary>
        /// <param name="mobileTransform"> Set mobile object transform </param>
        /// <returns></returns>
        public IEnumerator Movement(Transform mobileTransform)
        {
            Vector3 currentPos = mobileTransform.transform.position;
            float time = 0;
            while (_pathVectorList != null)
            {
                Vector3 targetPosition = _pathVectorList[_currentPathIndex];
                if (Vector3.Distance(mobileTransform.position, targetPosition) > 0.03f)
                {
                    mobileTransform.position = Vector3.Lerp(currentPos, targetPosition, time);
                    time += Time.deltaTime * _speed;
                    yield return null;
                }
                else
                {
                    _currentPathIndex++;
                    currentPos = mobileTransform.position;
                    time = 0;

                    if (_currentPathIndex >= _pathVectorList.Count)
                    {
                        mobileTransform.position = targetPosition;
                        StopMoving();
                    }
                    yield return null;
                }
                yield return new WaitForEndOfFrame();
            }
            yield return null;
        }
    }
}
