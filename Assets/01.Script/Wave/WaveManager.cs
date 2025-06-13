using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("웨이브")]
    public int currentWave = 1;
    [Header("생성될 좀비 수")]
    public int zombiesPerWave = 5;
    [Header("웨이브 간 대기시간")]
    public float waveInterval = 5f;

    [Header("좀비 스포너")]
    public ZombieSpawner spawner;

    private int aliveZombies = 0;               // 현재 살아 있는 좀비 수
    private bool isWaveSpawning = false;        // 웨이브 생성 중 여부

    // 매 프레임마다 좀비 수를 확인하여 웨이브 조건 만족 시 시작
    private void Update()
    {
        if (!isWaveSpawning && aliveZombies <= 0)
            StartCoroutine(StartNextWave());
    }

    // 다음 웨이브를 일정 간격으로 나눠서 소환
    private IEnumerator StartNextWave()
    {
        isWaveSpawning = true;

        Debug.Log($"[WaveManager] 다음 웨이브 시작 대기: {waveInterval}초");
        yield return new WaitForSeconds(waveInterval);

        int totalCount = currentWave * zombiesPerWave;         // 전체 소환할 좀비 수
        int spawnBatch = 5;                                     // 몇 번에 나눠서 소환할지
        int zombiesPerBatch = Mathf.CeilToInt((float)totalCount / spawnBatch); // 한 번에 소환할 수

        for (int i = 0; i < spawnBatch; i++)
        {
            // 남은 좀비 수 계산
            int remainingZombies = totalCount - (i * zombiesPerBatch);
            // 마지막 배치일 경우 남은 좀비 수만큼 소환
            int countToSpawn = Mathf.Min(zombiesPerBatch, remainingZombies);

            spawner.SpawnWave(countToSpawn); // 좀비 소환
            Debug.Log($"[WaveManager] {i + 1}/{spawnBatch}차 소환 - {countToSpawn}마리");
            yield return new WaitForSeconds(0.1f); // 다음 소환까지 대기
        }

        aliveZombies = totalCount; // 총 좀비 수 등록
        Debug.Log($"[WaveManager] 웨이브 {currentWave} 시작 - 총 좀비 수: {aliveZombies}");

        currentWave++;
        isWaveSpawning = false;
    }

    // 좀비가 사망했을 때 호출
    public void OnZombieDied()
    {
        aliveZombies--;
        Debug.Log($"[WaveManager] 좀비 사망 → 남은 수: {aliveZombies}");
    }
}
