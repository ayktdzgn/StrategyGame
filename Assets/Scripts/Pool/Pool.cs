using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> : MonoBehaviour where T : MonoBehaviour
{
    T _prefab;
    int _count;

    Queue<T> _poolQueue = new Queue<T>();
    Stack<T> _poolStack = new Stack<T>();

    public void Init(T prefab, int count)
    {
        _prefab = prefab;
        _count = count;

        for (int i = 0; i < _count; i++)
        {
            T obj = Instantiate(_prefab, transform);
            obj.gameObject.SetActive(false);
            _poolQueue.Enqueue(obj);
            _poolStack.Push(obj);
        }
    }

    public T GetObject()
    {
        if (_poolStack.Count < 1) Init(_prefab,_count);
        T obj = _poolQueue.Dequeue();

        obj.gameObject.SetActive(true);

        _poolQueue.Enqueue(obj);
        _poolStack.Pop();

        return obj;
    }

    public void BackToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        obj.transform.position = transform.position;
        _poolStack.Push(obj);
    }
}
