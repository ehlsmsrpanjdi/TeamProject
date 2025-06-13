using UnityEngine;
using System.Collections.Generic;

public class ZombiePool : MonoBehaviour
{
    [Header("좀비 프리팹")]
    public GameObject zombiePrefab;
    [Header("풀링 좀비 수")]
    public int poolSize = 50;

    // 비활성화된 좀비 오브젝트를 저장할 큐 (오브젝트 풀)
    private Queue<GameObject> pool = new Queue<GameObject>();

    // 게임 시작 시, 지정된 풀 크기만큼 좀비 프리팹을 미리 생성하여 비활성화 상태로 큐에 저장
    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(zombiePrefab, transform); // 부모를 현재 오브젝트로 설정
            obj.SetActive(false);  // 초기에는 비활성화
            pool.Enqueue(obj);     // 큐에 추가
        }
    }

    // 풀에서 좀비 오브젝트 하나를 꺼내서 위치 및 회전을 설정하고 활성화하여 반환
    public GameObject GetZombie(Vector3 position)
    {
        if (pool.Count == 0)
        {
            return null; // 남은 좀비가 없으면 null 반환
        }

        GameObject obj = pool.Dequeue();          // 큐에서 하나 꺼냄
        obj.transform.position = position;        // 위치 설정
        obj.transform.rotation = Quaternion.identity; // 회전 초기화
        obj.SetActive(true);                      // 활성화
        return obj;
    }

    // 사용이 끝난 좀비 오브젝트를 다시 비활성화하고 풀에 반환
    public void ReturnZombie(GameObject zombie)
    {
        zombie.SetActive(false);  // 비활성화
        pool.Enqueue(zombie);     // 큐에 다시 넣음
    }
}
