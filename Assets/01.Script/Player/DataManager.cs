using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public PlayerData data = new PlayerData(); // 실제 데이터 보관

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddGold(int amount)
    {
        data.gold += amount;
        Debug.Log($"[DataManager] 골드 +{amount} → 총 {data.gold}");
    }

    public void AddDiamond(int amount)
    {
        data.diamond += amount;
        Debug.Log($"[DataManager] 다이아 +{amount} → 총 {data.diamond}");
    }

    public void SetStage(int stage)
    {
        data.currentStage = stage;
        Debug.Log($"[DataManager] 현재 스테이지 설정: {stage}");
    }

    public int GetGold() => data.gold;
    public int GetDiamond() => data.diamond;
    public int GetStage() => data.currentStage;
}
