using System.Collections.Generic;
using UnityEngine;

public class ZombiePool : MonoBehaviour
{
    [Header("좀비1 프리팹")]
    [SerializeField] private GameObject zombie1Prefab;
    [Header("좀비2 프리팹")]
    [SerializeField] private GameObject zombie2Prefab;

    [Header("좀비1풀 크기")]
    [SerializeField] private int zombie1PoolSize = 50;
    [Header("좀비2풀 크기")]
    [SerializeField] private int zombie2PoolSize = 25;

    // 비활성화된 좀비 오브젝트를 저장할 큐 (오브젝트 풀)
    private Queue<GameObject> zombie1Pool = new Queue<GameObject>();
    private Queue<GameObject> zombie2Pool = new Queue<GameObject>();
    // 게임 시작 시, 지정된 풀 크기만큼 좀비 프리팹을 미리 생성하여 비활성화 상태로 큐에 저장
    private void Awake()
    {
        // 좀비 1종 풀 생성
        for (int i = 0; i < zombie1PoolSize; i++)
        {
            GameObject obj = Instantiate(zombie1Prefab, transform);
            obj.SetActive(false);
            zombie1Pool.Enqueue(obj);
        }

        // 좀비 2종 풀 생성
        for (int i = 0; i < zombie2PoolSize; i++)
        {
            GameObject obj = Instantiate(zombie2Prefab, transform);
            obj.SetActive(false);
            zombie2Pool.Enqueue(obj);
        }
    }

    // 타입에 맞는 좀비 풀에서 오브젝트를 꺼내 활성화 후 반환
    public GameObject GetZombie(int type, Vector3 position)
    {
        Queue<GameObject> pool = (type == 2) ? zombie2Pool : zombie1Pool;

        if (pool.Count == 0)
            return null;

        GameObject zombie = pool.Dequeue();
        zombie.transform.position = position;
        zombie.transform.rotation = Quaternion.identity;
        zombie.SetActive(true);
        return zombie;
    }

    // 사용 후 좀비 오브젝트를 다시 풀에 반환하여 비활성화하고 큐에 저장
    public void ReturnZombie(int type, GameObject zombie)
    {
        zombie.SetActive(false);
        if (type == 2)
            zombie2Pool.Enqueue(zombie);
        else
            zombie1Pool.Enqueue(zombie);
    }
}

