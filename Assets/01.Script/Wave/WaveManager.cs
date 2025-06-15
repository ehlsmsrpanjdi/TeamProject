using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("생성될 좀비 수")]
    public int zombiesPerWave = 5;

    [Header("웨이브 간 대기시간")]
    public float waveInterval = 5f;

    [Header("좀비 스포너")]
    public ZombieSpawner spawner;

    [Header("바리게이트")]
    [SerializeField] private GameObject Barricade;

    [Header("UI")]
    [SerializeField] private GameObject nextStageButtonUI; // 다음 스테이지 버튼

    private int aliveZombies = 0;               // 현재 살아 있는 좀비 수
    private bool isWaveSpawning = false;        // 웨이브 생성 중 여부

    // 추가 상태 변수
    private bool isWaitingNextStage = false;    // 플레이어 사망 후 웨이브 대기 상태
    private int lastCompletedWave = 0;          // 마지막으로 완료한 웨이브

    private void Update()
    {
        // 웨이브가 진행 중이 아니고, 살아있는 좀비가 없을 때
        if (!isWaveSpawning && aliveZombies <= 0)
        {
            if (!isWaitingNextStage)
            {
                // 일반 웨이브 클리어 → 다음 웨이브 시작
                lastCompletedWave = DataManager.Instance.data.currentStage - 1;
                StartCoroutine(StartNextWave());
            }
            else
            {
                // 대기 상태에서는 웨이브 반복 (같은 웨이브 다시 실행)
                StartCoroutine(StartRepeatWave());
            }
        }
    }

    // 다음 웨이브를 일정 간격 후 시작
    private IEnumerator StartNextWave()
    {
        isWaveSpawning = true;

        Debug.Log($"[WaveManager] 다음 웨이브 시작 대기: {waveInterval}초");
        yield return new WaitForSeconds(waveInterval);

        int stage = DataManager.Instance.data.currentStage; // 스테이지 == 웨이브
        int totalCount = stage * zombiesPerWave;
        int spawnBatch = 5;
        int zombiesPerBatch = Mathf.CeilToInt((float)totalCount / spawnBatch);

        for (int i = 0; i < spawnBatch; i++)
        {
            int remainingZombies = totalCount - (i * zombiesPerBatch);
            int countToSpawn = Mathf.Min(zombiesPerBatch, remainingZombies);

            spawner.SpawnWave(countToSpawn); // 좀비 소환
            Debug.Log($"[WaveManager] {i + 1}/{spawnBatch}차 소환 - {countToSpawn}마리");
            yield return new WaitForSeconds(0.1f);
        }

        aliveZombies = totalCount;
        Debug.Log($"[WaveManager] 웨이브 {stage} 시작 - 총 좀비 수: {aliveZombies}");

        DataManager.Instance.data.currentStage++; // 다음 웨이브 진입 준비
        isWaveSpawning = false;
    }


    // 웨이브 반복 (플레이어 사망 후)
    private IEnumerator StartRepeatWave()
    {
        isWaveSpawning = true;

        int stage = DataManager.Instance.data.currentStage;

        Debug.Log($"[WaveManager] 웨이브 {stage} 반복 대기: {waveInterval}초");
        yield return new WaitForSeconds(waveInterval);

        if (Barricade != null && !Barricade.activeSelf)
            Barricade.SetActive(true);

        int totalCount = stage * zombiesPerWave;
        int spawnBatch = 5;
        int zombiesPerBatch = Mathf.CeilToInt((float)totalCount / spawnBatch);

        for (int i = 0; i < spawnBatch; i++)
        {
            int remainingZombies = totalCount - (i * zombiesPerBatch);
            int countToSpawn = Mathf.Min(zombiesPerBatch, remainingZombies);

            spawner.SpawnWave(countToSpawn); // 좀비 소환
            Debug.Log($"[WaveManager] {i + 1}/{spawnBatch}차 반복 소환 - {countToSpawn}마리");
            yield return new WaitForSeconds(0.1f);
        }

        aliveZombies = totalCount;
        Debug.Log($"[WaveManager] 웨이브 {stage} 반복 시작 - 총 좀비 수: {aliveZombies}");

        isWaveSpawning = false;
    }

    // 좀비 사망 시 호출
    public void OnZombieDied()
    {
        aliveZombies--;
        Debug.Log($"[WaveManager] 좀비 사망 → 남은 수: {aliveZombies}");
    }

    // 플레이어가 죽었을 때
    public void OnPlayerDead()
    {
        Debug.Log("[WaveManager] 플레이어 사망 → 웨이브 감소 및 반복 진입");

        if (!isWaitingNextStage)
        {
            isWaitingNextStage = true;
            int currentStage = Mathf.Max(1, DataManager.Instance.GetStage() - 1);
            DataManager.Instance.SetStage(currentStage);

            if (nextStageButtonUI != null)
                nextStageButtonUI.SetActive(true);
        }

        ClearAllZombies();

        // 반복 웨이브 강제 재시작
        StartCoroutine(StartRepeatWave());
    }

    // 버튼 눌러 다음 웨이브로 이동
    public void ProceedToNextStage()
    {
        if (!isWaitingNextStage) return;

        Debug.Log("[WaveManager] 다음 스테이지로 진입");
        isWaitingNextStage = false;

        isWaveSpawning = true; // Update에서 중복 호출 방지

        // 다음 스테이지 버튼 비활성화
        if (nextStageButtonUI != null)
            nextStageButtonUI.SetActive(false);

        ClearAllZombies();
        StartCoroutine(StartNextWave());
    }

    // 현재 맵에 남은 좀비 전부 제거
    private void ClearAllZombies()
    {
        var allZombies = GameObject.FindGameObjectsWithTag("Zombie");
        foreach (var obj in allZombies)
        {
            ZombieAI ai = obj.GetComponent<ZombieAI>();
            if (ai != null)
            {
                if (obj.name.Contains("Zombie1"))
                    ai.ResetAndReturnToPool("Zombie1");
                else if (obj.name.Contains("Zombie2"))
                    ai.ResetAndReturnToPool("Zombie2");
                else
                    Destroy(obj);
            }
        }

        aliveZombies = 0;
    }
}
