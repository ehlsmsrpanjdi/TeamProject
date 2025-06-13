using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestScroll : UIBase
{
    List<QuestLog> questList = new List<QuestLog>();
    [SerializeField] GameObject questLogPrefab;

    [SerializeField] GameObject contentObject;

    [SerializeField] OnClickImage ReturnButton;

    const string QuestLog = "UI/QuestLog";

    private void Reset()
    {
        questLogPrefab = Resources.Load<GameObject>(QuestLog);
        contentObject = gameObject.GetComponentInChildren<HorizontalLayoutGroup>(true).gameObject;

        ReturnButton = GetComponentInChildren<OnClickImage>();
    }

    private void Awake()
    {
        ReturnButton.OnClick = UIManager.Instance.CloseUI<UIQuestScroll>;
    }

    private void Start()
    {
        AddQuest();
        AddQuest();
        AddQuest();
        AddQuest();
        AddQuest();
        AddQuest();
        AddQuest();
        AddQuest();
    }

    public void AddQuest()
    {
        GameObject logObject = Instantiate(questLogPrefab);
        logObject.transform.SetParent(contentObject.transform);
        QuestLog log = logObject.GetComponent<QuestLog>();
        questList.Add(log);
    }
}
