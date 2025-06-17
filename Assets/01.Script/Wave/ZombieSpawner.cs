using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("스폰 위치 중심")]
    public Transform spawnAreaCenter;

    [Header("스폰 범위 크기 (X,Z)")]
    public Vector2 spawnAreaSize = new Vector2(10f, 10f);

    public int SpawnWave(int zombieCount, bool isWeak)
    {
        int spawnSuccess = 0;

        for (int i = 0; i < zombieCount; i++)
        {
            Vector3 randomPos = GetRandomPositionInArea();

            // NavMesh 위 위치 보정
            if (UnityEngine.AI.NavMesh.SamplePosition(randomPos, out UnityEngine.AI.NavMeshHit hit, 1f, UnityEngine.AI.NavMesh.AllAreas))
            {
                randomPos = hit.position;
            }
            else
            {
                Debug.LogWarning("[ZombieSpawner] NavMesh 위 위치를 찾지 못해 스킵됨");
                continue;
            }

            string zombieKey = (Random.value <= 0.1f) ? "Zombie2" : "Zombie1";
            GameObject obj = ObjectPool.Get(zombieKey);

            if (obj == null)
            {
                Debug.LogWarning($"[ZombieSpawner] 풀 부족 또는 ObjectPool 미할당 - {zombieKey} 생성 실패");
                continue;
            }

            obj.transform.position = randomPos + Vector3.up * 0.05f;
            obj.transform.rotation = Quaternion.identity;

            ZombieAI ai = obj.GetComponent<ZombieAI>();
            if (ai != null)
            {
                ai.EnableAgent();
            }
            else
            {
                Debug.LogWarning($"[ZombieSpawner] ZombieAI 컴포넌트 없음: {obj.name}");
            }

            if (isWeak)
            {
                ZombieStatHandler statHandler = obj.GetComponent<ZombieStatHandler>();
                if (statHandler != null)
                    statHandler.ApplyWeakPenalty();
                else
                    Debug.LogWarning($"[ZombieSpawner] ZombieStatHandler 없음: {obj.name}");
            }

            spawnSuccess++;
        }

        return spawnSuccess;
    }

    private Vector3 GetRandomPositionInArea()
    {
        Vector3 center = spawnAreaCenter.position;
        float halfX = spawnAreaSize.x / 2f;
        float halfZ = spawnAreaSize.y / 2f;

        float randX = Random.Range(-halfX, halfX);
        float randZ = Random.Range(-halfZ, halfZ);

        return new Vector3(center.x + randX, center.y, center.z + randZ);
    }

    private void OnDrawGizmos()
    {
        if (spawnAreaCenter == null) return;

        Gizmos.color = Color.green;
        Vector3 size = new Vector3(spawnAreaSize.x, 0.1f, spawnAreaSize.y);
        Gizmos.DrawWireCube(spawnAreaCenter.position, size);
    }
}
