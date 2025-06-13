using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("좀비 풀링")]
    public ZombiePool zombiePool;
    [Header("스폰 위치")]
    public Transform spawnAreaCenter;
    [Header("스폰 크기")]
    public Vector2 spawnAreaSize = new Vector2(10f, 10f);

    // 지정된 수만큼 좀비를 소환
    public void SpawnWave(int zombieCount)
    {
        for (int i = 0; i < zombieCount; i++)
        {
            Vector3 randomPos = GetRandomPositionInArea(); // 랜덤 위치 계산

            // NavMesh 위의 유효한 위치로 보정
            if (UnityEngine.AI.NavMesh.SamplePosition(randomPos, out UnityEngine.AI.NavMeshHit hit, 1f, UnityEngine.AI.NavMesh.AllAreas))
            {
                randomPos = hit.position; // 유효 위치로 보정
            }
            else
            {
                Debug.LogWarning("[ZombieSpawner] NavMesh 위에서 위치를 찾지 못했습니다");
                continue; // 유효 위치를 찾지 못했으면 스킵
            }

            // 10% 확률로 좀비 2종 타입 선택
            int zombieType = 1;
            if (Random.value <= 0.1f)
                zombieType = 2;

            GameObject zombie = zombiePool.GetZombie(zombieType, randomPos); // 타입과 위치 인자 전달
            if (zombie == null) break; // 풀이 비어있으면 중단

            // 위치 설정은 GetZombie 내부에서 처리되므로 제거
        }
    }

    // 소환 구역 내에서 무작위 위치를 반환
    private Vector3 GetRandomPositionInArea()
    {
        Vector3 center = spawnAreaCenter.position;
        float halfX = spawnAreaSize.x / 2f;
        float halfZ = spawnAreaSize.y / 2f;

        float randX = Random.Range(-halfX, halfX);
        float randZ = Random.Range(-halfZ, halfZ);

        return new Vector3(center.x + randX, center.y, center.z + randZ);
    }

    // 에디터에서 소환 구역을 시각적으로 표시
    private void OnDrawGizmos()
    {
        if (spawnAreaCenter == null) return;

        Gizmos.color = Color.green;
        Vector3 size = new Vector3(spawnAreaSize.x, 0.1f, spawnAreaSize.y);
        Gizmos.DrawWireCube(spawnAreaCenter.position, size);
    }
}
