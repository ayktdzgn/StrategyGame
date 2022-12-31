using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Factory<T> : Singleton<Factory<T>> where T: MonoBehaviour
{
    [Serializable]
    public struct FactorySettings
    {
        public T productPrefab;
        public int count;
    }

    [SerializeField] protected Pool<T> _poolPrefab;
    [SerializeField] private FactorySettings[] _factorySettings;
    Dictionary<string, Pool<T>> poolDic = new Dictionary<string, Pool<T>>();

    public Dictionary<string, Pool<T>> PoolDic { get => poolDic; }
    public FactorySettings[] FactorySettingsArr { get => _factorySettings; }

    public override void Awake()
    {
        base.Awake();
        for (int i = 0; i < _factorySettings.Length; i++)
        {
            var pool = Instantiate(_poolPrefab, transform);
            pool.name = _factorySettings[i].productPrefab.name;
            pool.Init(_factorySettings[i].productPrefab, _factorySettings[i].count);

            poolDic.Add(pool.name, pool.GetComponent<Pool<T>>());
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
