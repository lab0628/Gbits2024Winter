using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool
{
    private Queue<GameObject> pool;
    private GameObject prefab;
    /// <summary>
    /// 创建一个对象池 并生成amount个对象在池子中
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="amount"></param>
    public GameObjectPool(GameObject prefab, int amount)
    {
        this.prefab = prefab;
        pool = new Queue<GameObject>(amount);
        for (int i = 0; i < amount; i++)
        {
            pool.Enqueue(InstantiateGameObject());
        }
    }

    private GameObject InstantiateGameObject()
    {
        GameObject obj = GameObject.Instantiate(prefab);
        obj.transform.SetParent(PoolMgr.Ins.transform);
        obj.SetActive(false);
        return obj;
    }

    /// <summary>
    /// 从对象池获取一个对象出来
    /// </summary>
    /// <returns></returns>
    public GameObject Get()
    {
        GameObject obj = pool.Count > 0 ? pool.Dequeue() : InstantiateGameObject();
        obj.SetActive(true);
        return obj;
        
    }
    /// <summary>
    /// 将一个对象归还于对象池中
    /// </summary>
    /// <param name="obj"></param>
    public void Put(GameObject obj)
    {
        if (obj != null)
        {
            obj.transform.SetParent(PoolMgr.Ins.transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
        
    }
    

}
