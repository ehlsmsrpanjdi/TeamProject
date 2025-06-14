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

    public override void Open()
    {
        base.Open();
        List<QuestDisplayInfo> questInfos = QuestManager.Instance.GetQuestDisplayInfos();
        foreach (QuestDisplayInfo info in questInfos)
        {
            AddQuest(info);
        }
    }

    public void AddQuest(QuestDisplayInfo _info)
    {
        GameObject logObject = Instantiate(questLogPrefab);
        logObject.transform.SetParent(contentObject.transform);
        QuestLog log = logObject.GetComponent<QuestLog>();
        log.SetQuestName(_info.Title);
        log.SetQuestDescription(_info.Description);
        questList.Add(log);
    }
}
