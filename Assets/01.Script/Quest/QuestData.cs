using UnityEngine;


public enum QuestType
{
    IncreaseAttackLevel,
    DailyLogin,
    Play30Minute,
    AllClear,
}

[System.Serializable]
public class QuestData // 퀘스트 데이터
{
    public int Id;
    public string Title;         // 퀘스트 제목
    public string Description;  // 퀘스트 설명
    public QuestType Type;      // 퀘스트 타입
    public int TargetValue;     // 목표치(공격력 퀘스트에서, 달성 목표 = 시작값 + 목표치 * n번째 퀘스트 진행중)
    public int CurrentValue;    // 현재 퀘스트 수치.

    public bool IsCompleted;    // 조건 만족 여부
    public bool IsClaimed;      // 보상 수령 여부
}
