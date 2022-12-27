using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Factory<T> : MonoBehaviour where T: MonoBehaviour
{
    [Serializable]
    public struct FactorySettings
    {
        public T productPrefab;
        public int count;
    }

    [SerializeField] protected Pool<T> _poolPrefab;
    [SerializeField] protected FactorySettings[] _factorySettings;
    Dictionary<string, Pool<T>> poolDic = new Dictionary<string, Pool<T>>();

    void Awake()
    {
        for (int i = 0; i < _factorySettings.Length; i++)
        {
            var _pool = Instantiate(_poolPrefab, transform);
            _pool.name = _factorySettings[i].productPrefab.name + " Pool";
            _pool.Init(_factorySettings[i].productPrefab, _factorySettings[i].count);

            poolDic.Add(_pool.name, _pool.GetComponent<Pool<T>>());
        }
    }

    public T GetNewProduct(string name)
    {
        if (poolDic.TryGetValue(name, out Pool<T> value))
        {
            return value.GetObject();
        }
        
        return null;
    }
}
