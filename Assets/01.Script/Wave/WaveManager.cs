using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    private bool isRetryWeakMode = false;       // 약화모드 여부
    public bool IsWeakRetryWave() => isRetryWeakMode; // 외부 접근용

    private void Start()
    {
        isWaitingNextStage = false;
        isRetryWeakMode = false;


        if (nextStageButtonUI != null)
        {
            Button btn = nextStageButtonUI.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(ProceedToNextStage);
                nextStageButtonUI.SetActive(false);
            }
        }

        StartCoroutine(StartNextWave());
    }

    // 다음 웨이브를 일정 간격 후 시작
    private IEnumerator StartNextWave()
    {
        isWaveSpawning = true;

        Debug.Log($"[WaveManager] 다음 웨이브 시작 대기: {waveInterval}초");
        yield return new WaitForSeconds(waveInterval);

        int stage = Player.Instance.Data.currentStage;
        int totalCount = stage * zombiesPerWave;
        int spawnBatch = 5;
        int zombiesPerBatch = Mathf.CeilToInt((float)totalCount / spawnBatch);

        for (int i = 0; i < spawnBatch; i++)
        {
            int remainingZombies = totalCount - (i * zombiesPerBatch);
            int countToSpawn = Mathf.Min(zombiesPerBatch, remainingZombies);

            spawner.SpawnWave(countToSpawn, false); // 정상 모드로 소환
            Debug.Log($"[WaveManager] {i + 1}/{spawnBatch}차 소환 - {countToSpawn}마리");
            yield return new WaitForSeconds(0.1f);
        }

        aliveZombies = totalCount;
        Debug.Log($"[WaveManager] 웨이브 {stage} 시작 - 총 좀비 수: {aliveZombies}");

        isWaveSpawning = false;
    }


    // 웨이브 반복 (약화모드)
    private IEnumerator StartRepeatWave()
    {
        isWaveSpawning = true;

        int repeatStage = Player.Instance.Data.currentStage;

        Debug.Log($"[WaveManager] 웨이브 {repeatStage} 반복 대기: {waveInterval}초");
        yield return new WaitForSeconds(waveInterval);

        if (Barricade != null && !Barricade.activeSelf)
            Barricade.SetActive(true);

        int totalCount = repeatStage * zombiesPerWave;
        int spawnBatch = 5;
        int zombiesPerBatch = Mathf.CeilToInt((float)totalCount / spawnBatch);

        for (int i = 0; i < spawnBatch; i++)
        {
            int remainingZombies = totalCount - (i * zombiesPerBatch);
            int countToSpawn = Mathf.Min(zombiesPerBatch, remainingZombies);

            spawner.SpawnWave(countToSpawn, true); // 약화모드로 소환
            Debug.Log($"[WaveManager] {i + 1}/{spawnBatch}차 반복 소환 - {countToSpawn}마리");
            yield return new WaitForSeconds(0.1f);
        }

        aliveZombies = totalCount;
        Debug.Log($"[WaveManager] 웨이브 {repeatStage} 반복 시작 - 총 좀비 수: {aliveZombies}");

        isWaveSpawning = false;
    }

    // 좀비 사망 시 호출
    public void OnZombieDied()
    {
        aliveZombies--;
        Debug.Log($"[WaveManager] 좀비 사망 → 남은 수: {aliveZombies}");

        if (aliveZombies <= 0)
        {
            if (!isWaitingNextStage)
            {
                // 버튼 누르기 전까지는 다음 웨이브로 넘어가지 않음
                isWaitingNextStage = true;
                isRetryWeakMode = true;
                nextStageButtonUI?.SetActive(true);
                StartCoroutine(StartRepeatWave());
            }
            else
            {
                StartCoroutine(StartRepeatWave());
            }
        }
    }

    // 플레이어가 죽었을 때
    public void OnPlayerDead()
    {
        Debug.Log("[WaveManager] 플레이어 사망 → 약화모드 진입 또는 유지");

        if (!isWaitingNextStage)
        {
            // 약화모드로 처음 진입하는 경우
            isWaitingNextStage = true;
            Player.Instance.Data.currentStage = Mathf.Max(1, Player.Instance.Data.currentStage - 1);
        }

        isRetryWeakMode = true;
        nextStageButtonUI?.SetActive(true);

        ClearAllZombies();
        StartCoroutine(StartRepeatWave());
    }

    // 버튼 눌러 다음 웨이브로 이동
    public void ProceedToNextStage()
    {
        if (!isWaitingNextStage) return;

        Debug.Log("[WaveManager] 다음 스테이지로 진입");
        isWaitingNextStage = false;
        isRetryWeakMode = false;

        Player.Instance.Data.currentStage++;
        nextStageButtonUI?.SetActive(false);

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
                    Object.Destroy(obj);
            }
        }

        aliveZombies = 0;
    }

    // 외부에서 현재 약화모드 여부 확인
    public bool IsWeakMode()
    {
        return isRetryWeakMode;
    }
}
