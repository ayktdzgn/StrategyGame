using System;
using System.Collections;
using System.Collections.Generic;
using Core.Pool;
using Core.Singleton;
using UnityEngine;

namespace Core.Factory
{
    public abstract class Factory<T> : Singleton<Factory<T>> where T : MonoBehaviour
    {
        [Serializable]
        public struct FactorySettings
        {
            public T productPrefab;
            public int count;
        }

        [SerializeField] private FactorySettings[] _factorySettings;
        Dictionary<string, Pool<T>> _poolDic = new Dictionary<string, Pool<T>>();

        public Dictionary<string, Pool<T>> PoolDic { get => _poolDic; }
        public FactorySettings[] FactorySettingsArr { get => _factorySettings; }

        public override void Awake()
        {
            base.Awake();
            for (int i = 0; i < _factorySettings.Length; i++)
            {
                var pool = new Pool<T>(_factorySettings[i].productPrefab, _factorySettings[i].count, transform);
                pool.Init();

                _poolDic.Add(_factorySettings[i].productPrefab.name, pool);
            }
        }

        public T GetNewProduct(string name)
        {
            if (_poolDic.TryGetValue(name, out Pool<T> value))
            {
                return value.GetObject();
            }

            return null;
        }

        public void RefuseProduct(string name, T product)
        {
            if (_poolDic.TryGetValue(name, out Pool<T> value))
            {
                value.BackToPool(product);
            }
        }
    }
}
