using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Pool
{
    public class Pool<T> where T : Component
    {
        T _prefab;
        int _count;
        Transform _transform;

        Queue<T> _poolQueue = new Queue<T>();
        Stack<T> _poolStack = new Stack<T>();

        public Pool(T prefab, int count, Transform transform)
        {
            _prefab = prefab;
            _count = count;
            _transform = transform;
        }

        //Init it self instantiate objects and push them into stack and queue
        public void Init()
        {
            for (int i = 0; i < _count; i++)
            {
                T obj = Object.Instantiate(_prefab, _transform);
                obj.gameObject.SetActive(false);
                _poolQueue.Enqueue(obj);
                _poolStack.Push(obj);
            }
        }
        //Get Object from pool and pop it from stack. If stack run out then pool will init itself
        public T GetObject()
        {
            if (_poolStack.Count < 1) Init();
            T obj = _poolQueue.Dequeue();

            obj.gameObject.SetActive(true);

            _poolQueue.Enqueue(obj);
            _poolStack.Pop();

            return obj;
        }
        //Back object to pool and add it push it stack
        public void BackToPool(T obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_transform);
            obj.transform.position = _transform.position;
            _poolStack.Push(obj);
        }
    }
}
