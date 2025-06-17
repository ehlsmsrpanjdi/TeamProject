using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [Header("웨이브 설정")]
    public int zombiesPerWave = 5;
    public float waveInterval = 5f;
    public ZombieSpawner spawner;
    [SerializeField] private GameObject Barricade;

    [Header("UI")]
    [SerializeField] private GameObject nextStageButtonUI;

    private int aliveZombies = 0;
    private bool isWaveSpawning = false;
    private bool isRetryWeakMode = false;
    private bool isWaitingNextStage = false;
    private int retryStage = -1;
    private Coroutine currentWaveCoroutine;

    private void Start()
    {
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

        currentWaveCoroutine = StartCoroutine(StartNormalWave());
    }

    private IEnumerator StartNormalWave()
    {
        if (isWaveSpawning) yield break;
        isWaveSpawning = true;

        isRetryWeakMode = false;
        isWaitingNextStage = false;
        retryStage = -1;

        ClearAllZombies();

        int stage = Player.Instance.Data.currentStage;
        int totalCount = stage * zombiesPerWave;
        int spawnBatch = 5;
        int zombiesPerBatch = Mathf.CeilToInt((float)totalCount / spawnBatch);

        yield return new WaitForSeconds(waveInterval);

        int totalSpawned = 0;
        for (int i = 0; i < spawnBatch; i++)
        {
            int remaining = totalCount - (i * zombiesPerBatch);
            int count = Mathf.Min(zombiesPerBatch, remaining);
            int spawned = spawner.SpawnWave(count, false);
            totalSpawned += spawned;
            yield return new WaitForSeconds(0.1f);
        }

        aliveZombies = totalSpawned;
        Debug.Log($"[WaveManager] 일반 웨이브 - 스테이지: {stage}, 생성된 좀비 수: {totalSpawned}");
        isWaveSpawning = false;
    }

    private IEnumerator StartRepeatWave()
    {
        if (isWaveSpawning) yield break;
        isWaveSpawning = true;

        ClearAllZombies();

        int stage = retryStage;
        int totalCount = stage * zombiesPerWave;
        int spawnBatch = 5;
        int zombiesPerBatch = Mathf.CeilToInt((float)totalCount / spawnBatch);

        yield return new WaitForSeconds(waveInterval);

        int totalSpawned = 0;
        for (int i = 0; i < spawnBatch; i++)
        {
            int remaining = totalCount - (i * zombiesPerBatch);
            int count = Mathf.Min(zombiesPerBatch, remaining);
            int spawned = spawner.SpawnWave(count, true);
            totalSpawned += spawned;
            yield return new WaitForSeconds(0.1f);
        }

        aliveZombies = totalSpawned;
        Debug.Log($"[WaveManager] 반복(약화) 웨이브 - 스테이지: {stage}, 생성된 좀비 수: {totalSpawned}");

        nextStageButtonUI?.SetActive(true);

        isWaveSpawning = false;
    }

    public void OnZombieDied()
    {
        aliveZombies--;

        if (aliveZombies <= 0)
        {
            if (!isRetryWeakMode)
            {
                isRetryWeakMode = true;
                isWaitingNextStage = true;
                retryStage = Player.Instance.Data.currentStage;
                currentWaveCoroutine = StartCoroutine(StartRepeatWave());
            }
            else if (isRetryWeakMode && isWaitingNextStage)
            {
                currentWaveCoroutine = StartCoroutine(StartRepeatWave());
            }
        }
    }

    public void OnPlayerDead()
    {
        if (!isRetryWeakMode)
        {
            Player.Instance.Data.currentStage = Mathf.Max(1, Player.Instance.Data.currentStage - 1);
            retryStage = Player.Instance.Data.currentStage;
            isRetryWeakMode = true;
        }

        isWaitingNextStage = true;

        ClearAllZombies();
        currentWaveCoroutine = StartCoroutine(StartRepeatWave());
    }

    public void ProceedToNextStage()
    {
        if (!isWaitingNextStage) return;

        Debug.Log("[WaveManager] 다음 스테이지로 진입");

        isWaitingNextStage = false;
        isRetryWeakMode = false;

        if (currentWaveCoroutine != null)
        {
            StopCoroutine(currentWaveCoroutine);
            currentWaveCoroutine = null;
        }

        nextStageButtonUI?.SetActive(false);

        ClearAllZombies();
        currentWaveCoroutine = StartCoroutine(NextWaveAfterIncrement());
    }

    private IEnumerator NextWaveAfterIncrement()
    {
        yield return new WaitForSeconds(0.1f);
        Player.Instance.Data.currentStage++;
        currentWaveCoroutine = StartCoroutine(StartNormalWave());
    }

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

    public bool IsWeakMode() => isRetryWeakMode;
}
