using System;
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
}

[Serializable]
public class QuestDisplayInfo
{
    public int Id;
    public string Title;
    public string Description;
    public bool IsCompleted;
    public bool IsClaimed;
    public int TargetValue;
    public int CurrentValue;

    public QuestDisplayInfo(int id, string title, string description, bool isCompleted, bool isClaimed, int targetValue, int currentValue)
    {
        Id = id;
        Title = title;
        Description = description;
        IsCompleted = isCompleted;
        IsClaimed = isClaimed;
        TargetValue = targetValue;
        CurrentValue = currentValue;
    }
}
