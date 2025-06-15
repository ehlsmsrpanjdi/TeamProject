using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("스폰 위치 중심")]
    public Transform spawnAreaCenter;

    [Header("스폰 범위 크기 (X,Z)")]
    public Vector2 spawnAreaSize = new Vector2(10f, 10f);

    // 좀비 n마리 소환
    public void SpawnWave(int zombieCount)
    {
        for (int i = 0; i < zombieCount; i++)
        {
            Vector3 randomPos = GetRandomPositionInArea();

            // NavMesh 위로 위치 보정
            if (UnityEngine.AI.NavMesh.SamplePosition(randomPos, out UnityEngine.AI.NavMeshHit hit, 1f, UnityEngine.AI.NavMesh.AllAreas))
            {
                randomPos = hit.position;
            }
            else
            {
                Debug.LogWarning("[ZombieSpawner] NavMesh 위 위치를 찾지 못해 스킵됨");
                continue;
            }

            // 10% 확률로 Zombie2, 아니면 Zombie1
            string zombieKey = (Random.value <= 0.1f) ? "Zombie2" : "Zombie1";

            Zombie zombie = Pool.Instance.GetZombie(zombieKey);
            if (zombie == null) continue;

            zombie.transform.position = randomPos;
        }
    }

    // 소환 구역 내 랜덤 위치 계산
    private Vector3 GetRandomPositionInArea()
    {
        Vector3 center = spawnAreaCenter.position;
        float halfX = spawnAreaSize.x / 2f;
        float halfZ = spawnAreaSize.y / 2f;

        float randX = Random.Range(-halfX, halfX);
        float randZ = Random.Range(-halfZ, halfZ);

        return new Vector3(center.x + randX, center.y, center.z + randZ);
    }

    // 에디터에서 스폰 구역 시각화
    private void OnDrawGizmos()
    {
        if (spawnAreaCenter == null) return;

        Gizmos.color = Color.green;
        Vector3 size = new Vector3(spawnAreaSize.x, 0.1f, spawnAreaSize.y);
        Gizmos.DrawWireCube(spawnAreaCenter.position, size);
    }
}
