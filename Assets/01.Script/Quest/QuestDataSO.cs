using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewQuestData", menuName = "Quests/QuestData")]
public class QuestDataSO : ScriptableObject
{
    public int id;
    public string title;
    public string description;
    public QuestType type;
    public int targetValue;

    public QuestData ToQuestData()
    {
        return new QuestData(id, title, description, type, targetValue);
    }
}


[CreateAssetMenu(fileName = "QuestDataList", menuName = "Quest/QuestDataList")]
public class QuestDataListSO : ScriptableObject
{
    public List<QuestDataSO> quests;
}
