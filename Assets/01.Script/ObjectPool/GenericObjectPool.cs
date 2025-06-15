using System.Collections.Generic;
using UnityEngine;

public class GenericObjectPool<T> where T : Component
{
    private readonly T prefab;           // 생성할 프리팹
    private readonly Queue<T> pool = new Queue<T>(); // 오브젝트 풀 큐
    private readonly Transform parent;   // 생성될 오브젝트의 부모

    //초기 오브젝트를 생성하여 비활성화 상태로 큐에 저장
    public GenericObjectPool(T prefab, int initialCount = 10, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialCount; i++)
        {
            T obj = Object.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    // 오브젝트 하나 꺼내서 활성화 상태로 반환
    public T Get()
    {
        T obj = (pool.Count > 0) ? pool.Dequeue() : Object.Instantiate(prefab, parent);
        obj.gameObject.SetActive(true);
        return obj;
    }

    // 사용 완료된 오브젝트를 비활성화 후 큐에 다시 저장
    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
