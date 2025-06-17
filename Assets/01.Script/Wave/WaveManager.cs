using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    public Action OnWaveClearAction;
    public Action OnWaveStartAction;
    private static WaveManager instance;

    bool IsWeak = false;

    public static WaveManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<WaveManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject("WaveManager");
                    instance = obj.AddComponent<WaveManager>();
                }
            }

            return instance;
        }
    }


    [Header("웨이브 설정")]
    public int zombiesPerWave = 5;
    public float waveInterval = 5f;
    public ZombieSpawner spawner;
    [SerializeField] private GameObject Barricade;

    [Header("UI")]
    private Coroutine currentWaveCoroutine;

    static bool IsFailed = false;

    private void Start()
    {
        OnWaveStartAction = StartUIFunction;
        OnWaveClearAction = WaveClearUIFunction;

        if (IsFailed == false)
        {
            currentWaveCoroutine = StartCoroutine(StartNormalWave());
        }
        else
        {
            OnWaveStartAction?.Invoke();
            currentWaveCoroutine = StartCoroutine(StartRepeatWave());
            OnWaveClearAction.Invoke();
            IsFailed = false;
        }
    }

    public void RunAwayStage()
    {
        IsFailed = true;
        UIManager Manager = UIManager.Instance;
        UIDoor Door = Manager.GetUI<UIDoor>(Manager.GetMainCanvas());
        Door.OnCloseAction = GoNextStageDelayOnClose;
        Door.OnOpenAction = GoPrevStageDelayOnStart;
        Door.Open();
    }

    void StartUIFunction()
    {
        UIManager Manager = UIManager.Instance;
        UIStage Stage = Manager.GetUI<UIStage>(Manager.GetBattleCanvas());
        Stage.SetStageText(Player.Instance.Data.currentStage);
        CharacterManager.Instance.SpawnParticipateCharacters();
    }

    void WaveClearUIFunction()
    {
        UIManager Manager = UIManager.Instance;
        UINextStage NextStage = Manager.GetUI<UINextStage>(Manager.GetBattleCanvas());
        NextStage.OnClickAction(GoNextStage);
        NextStage.Open();
    }

    void GoNextStage()
    {
        UIManager Manager = UIManager.Instance;
        UIDoor Door = Manager.GetUI<UIDoor>(Manager.GetMainCanvas());
        Manager.CloseUI<UINextStage>(Manager.GetMainCanvas());
        Door.OnCloseAction = GoNextStageDelayOnClose;
        Door.OnOpenAction = GoNextStageDelayOnStart;
        Door.Open();
    }

    void GoNextStageDelayOnClose()
    {
        ClearAllZombies();
        ObjectPool.ResetPool();
    }

    void GoPrevStageDelayOnStart()
    {
        UIManager Manager = UIManager.Instance;
        SceneManager.LoadScene("BattleScene");
    }

    void GoNextStageDelayOnStart()
    {
        ++Player.Instance.Data.currentStage;
        UIManager Manager = UIManager.Instance;
        SceneManager.LoadScene("BattleScene");
    }
    int zombieTotalCount;
    int zombieRepeatCount;

    public bool IsWeakMode() => IsWeak;

    private IEnumerator StartNormalWave()
    {
        IsWeak = false;
        OnWaveStartAction?.Invoke();
        int stage = Player.Instance.Data.currentStage;
        zombieTotalCount = stage + zombiesPerWave;
        int spawnBatch = 5; //몇번 나눠서 올것인가
        int zombiesPerBatch = Mathf.CeilToInt((float)zombieTotalCount / spawnBatch);

        yield return new WaitForSeconds(waveInterval);

        int totalSpawned = 0;
        for (int i = 0; i < spawnBatch; i++)
        {
            int remaining = zombieTotalCount - (i * zombiesPerBatch);
            int count = Mathf.Min(zombiesPerBatch, remaining);
            int spawned = spawner.SpawnWave(count, false);
            totalSpawned += spawned;
            yield return new WaitForSeconds(1.0f); //스폰간격
        }
        Debug.Log($"[WaveManager] 일반 웨이브 - 스테이지: {stage}, 생성된 좀비 수: {totalSpawned}");
    }

    private IEnumerator StartRepeatWave()
    {
        IsWeak = true;
        int stage = Player.Instance.Data.currentStage;
        zombieRepeatCount = stage + zombiesPerWave;
        int spawnBatch = 5; //몇번 나눠서 올것인가
        int zombiesPerBatch = Mathf.CeilToInt((float)zombieRepeatCount / spawnBatch);

        yield return new WaitForSeconds(waveInterval);

        int totalSpawned = 0;
        for (int i = 0; i < spawnBatch; i++)
        {
            int remaining = zombieRepeatCount - (i * zombiesPerBatch);
            int count = Mathf.Min(zombiesPerBatch, remaining);
            int spawned = spawner.SpawnWave(count, true);
            totalSpawned += spawned;
            yield return new WaitForSeconds(1.0f); //스폰간격
        }
        Debug.Log($"[WaveManager] 반복(약화) 웨이브 - 스테이지: {stage}, 생성된 좀비 수: {totalSpawned}");
    }

    public void OnZombieDied()
    {
        if (zombieTotalCount > 0)
        {
            --zombieTotalCount;
            if (zombieTotalCount == 0)
            {
                OnWaveClearAction?.Invoke();
                if (currentWaveCoroutine != null)
                {
                    StopCoroutine(currentWaveCoroutine);
                    currentWaveCoroutine = StartCoroutine(StartRepeatWave());
                }
            }
        }
        if (zombieRepeatCount > 0)
        {
            --zombieRepeatCount;
            if (currentWaveCoroutine != null)
            {
                StopCoroutine(currentWaveCoroutine);
                currentWaveCoroutine = StartCoroutine(StartRepeatWave());
            }
        }

    }

    public void OnPlayerDead()
    {
        Player.Instance.Data.currentStage = Mathf.Max(1, Player.Instance.Data.currentStage - 1);
    }

    //public void ProceedToNextStage()
    //{
    //    if (!isWaitingNextStage) return;

    //    Debug.Log("[WaveManager] 다음 스테이지로 진입");

    //    isWaitingNextStage = false;
    //    isRetryWeakMode = false;

    //    if (currentWaveCoroutine != null)
    //    {
    //        StopCoroutine(currentWaveCoroutine);
    //        currentWaveCoroutine = null;
    //    }

    //    ClearAllZombies();
    //    currentWaveCoroutine = StartCoroutine(NextWaveAfterIncrement());
    //}

    //private IEnumerator NextWaveAfterIncrement()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    Player.Instance.Data.currentStage++;
    //    currentWaveCoroutine = StartCoroutine(StartNormalWave());
    //}

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
        zombieTotalCount = 0;
    }
}
