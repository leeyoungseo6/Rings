using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    private Dictionary<string, Pool<PoolableMono>> _pools = new();

    public void CreatePool(PoolableMono prefab, int count = 10)
    {
        Pool<PoolableMono> pool = new Pool<PoolableMono>(prefab, transform, count);
        _pools.Add(prefab.gameObject.name, pool);
    }

    public PoolableMono Pop(string name)
    {
        PoolableMono item;
        try
        {
            item = _pools[name].Pop();
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine(e);
            Debug.LogError($"Prefab does not exist on pool : {name}");
            return null;
        }
        item.Init();
        return item;
    }

    public void Push(PoolableMono obj)
    {
        _pools[obj.name].Push(obj);
    }
}
