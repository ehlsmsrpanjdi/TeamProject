using UnityEngine;

public enum QuestType
{
    IncreaseAttackLevel,
    DailyLogin,
    Play10Minute,
    AllClear,
}

[System.Serializable]
public class QuestData
{
    [SerializeField] private int id;
    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private QuestType type;
    [SerializeField] private int targetValue;
    [SerializeField] private int currentValue;
    [SerializeField] private bool isCompleted;
    [SerializeField] private bool isClaimed;

    public int Id => id;
    public string Title => title;
    public string Description => description;
    public QuestType Type => type;
    public int TargetValue => targetValue;


    public int CurrentValue
    {
        get => currentValue;
        set => currentValue = value;
    }

    public bool IsCompleted
    {
        get => isCompleted;
        set => isCompleted = value;
    }

    public bool IsClaimed
    {
        get => isClaimed;
        set => isClaimed = value;
    }

    //기본생성자
    public QuestData(int id, string title, string description, QuestType type, int targetValue)
    {
        this.id = id;
        this.title = title;
        this.description = description;
        this.type = type;
        this.targetValue = targetValue;
        this.currentValue = 0;
        this.isCompleted = false;
        this.isClaimed = false;
    }

    // // PlayerPrefs에서 로드할 때 사용하는 생성자 (또는 로드 메서드)
    // public QuestData(int id, string title, string description, QuestType type, int targetValue, int currentValue, bool isCompleted, bool isClaimed)
    //     : this(id, title, description, type, targetValue) // 기본 생성자 호출
    // {
    //     this.currentValue = currentValue;
    //     this.isCompleted = isCompleted;
    //     this.isClaimed = isClaimed;
    // }
}

// [System.Serializable]
// public class QuestData // 퀘스트 데이터
// {
//     public int Id;
//     public string Title;         // 퀘스트 제목
//     public string Description;  // 퀘스트 설명
//     public QuestType Type;      // 퀘스트 타입
//     public int TargetValue;     // 목표치(공격력 퀘스트에서, 달성 목표 = 시작값 + 목표치 * n번째 퀘스트 진행중)
//     public int CurrentValue;    // 현재 퀘스트 수치.
//
//     public bool IsCompleted;    // 조건 만족 여부
//     public bool IsClaimed;      // 보상 수령 여부
// }
