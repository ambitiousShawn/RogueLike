using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    映射关系：
        外部调用时Get和Set时，传入的参数name是Resources资源下的全路径。
        当切换场景时，务必调用一次ClearAll()，防止下一个场景没有缓存池对象。
        缓存池中的不同物体仍可以细致分类，此处暂时做成V1.0。
 */


public class PoolManager : Singleton<PoolManager>
{
    //字典存储对应类型的游戏对象(缓存池)
    public Dictionary<string, Queue<GameObject>> cachePool = new Dictionary<string, Queue<GameObject>>();

    //缓存池对象
    private GameObject basePool;
    private GameObject activePool;
    private GameObject inactivePool;

    //从缓存池取
    public GameObject GetElement(string name,Transform initTrans = null)
    {
        //设置层级
        if (basePool == null)
        {
            basePool = new GameObject("CachePool");
            activePool = new GameObject("ActivePool");
            activePool.transform.parent = basePool.transform;
            inactivePool = new GameObject("InactivePool");
            inactivePool.transform.parent = basePool.transform;
        }

        GameObject res = null;
        
        if (cachePool.ContainsKey(name) && cachePool[name].Count > 0)
        {
            //当有想要的缓存元素时
            res = cachePool[name].Dequeue();
        }
        else
        {
            //当没有想要的元素时
            res = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(name));
            res.name = name;
            
        }
        //拿出缓存后，让场景物体激活
        res.SetActive(true);
        res.transform.parent = activePool.transform;
        if (initTrans != null)
            res.transform.position = initTrans.position;
        return res;
    }

    //存入缓存
    public void SetElement(string name,GameObject element)
    {
        //设置父对象
        element.transform.parent = inactivePool.transform;

        //存入缓存后，让场景物体失活
        element.SetActive(false);
        if (cachePool.ContainsKey(name))
        {
            //当存在对应缓存池时
            cachePool[name].Enqueue(element);
        }
        else
        {
            //当不存在对应缓存池时
            cachePool.Add(name, new Queue<GameObject>());
        }
    }

    //清空缓存池，主要用于场景切换
    public void ClearAll()
    {
        cachePool.Clear();
        basePool = null;
    }
}
