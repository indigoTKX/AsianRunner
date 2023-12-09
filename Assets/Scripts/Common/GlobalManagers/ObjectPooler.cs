using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private List<ObjectPoolData> _objectPoolDatas;

    public GameObject GetPooledObject(string poolUid)
    {
        if (!_objectPools.ContainsKey(poolUid))
        {
            Debug.LogError($"Pool with uid {poolUid} doesn't exist!");
            return null;
        }
        
        var pool = _objectPools[poolUid];
        GameObject obj;
        if (!pool.IsEmpty())
        {
            obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        var prefab = _objectPoolDatas.First(data => data.Uid == poolUid);
        obj = _diContainer.InstantiatePrefab(prefab.Prefab);
        return obj;
    }

    public void ReturnToPool(GameObject obj, string poolUid)
    {
        if (!_objectPools.ContainsKey(poolUid))
        {
            Debug.LogError($"Pool with uid {poolUid} doesn't exist!");
            return;
        }
        
        obj.SetActive(false);
        var pool = _objectPools[poolUid];
        pool.Enqueue(obj);
    }

    private Dictionary<string, Queue<GameObject>> _objectPools = new();
    
    private DiContainer _diContainer;

    [Inject]
    private void Construct(DiContainer diContainer)
    {
        _diContainer = diContainer;
    }
    
    private void Awake()
    {
        foreach (var data in _objectPoolDatas)
        {
            var pool = new Queue<GameObject>();
            for (int i = 0; i < data.InitialCount; i++)
            {
                var obj = _diContainer.InstantiatePrefab(data.Prefab);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
            _objectPools.Add(data.Uid, pool);
        }
    }

    [Serializable]
    private class ObjectPoolData
    {
        public string Uid = "Object";
        public int InitialCount = 10;
        public GameObject Prefab;
    }
}
