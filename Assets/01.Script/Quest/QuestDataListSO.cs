using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestDataList", menuName = "Quests/QuestDataList")]
public class QuestDataListSO : ScriptableObject
{
    public List<QuestDataSO> quests;
}
