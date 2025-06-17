using DG.Tweening;
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
        contentObject = gameObject.GetComponentInChildren<HorizontalLayoutGroup>(true).gameObject;

        ReturnButton = GetComponentInChildren<OnClickImage>();
    }

    private void Awake()
    {
        ReturnButton.OnClick = OnReturnButtonclick;
    }

    private void Start()
    {
        if (null == questLogPrefab)
        {
            Resources.Load<GameObject>(QuestLog);
        }
    }

    void OnReturnButtonclick()
    {
        UIManager.Instance.CloseUI<UIQuestScroll>(UIManager.Instance.GetMainCanvas());
    }

    public override void Open()
    {
        base.Open();
        List<QuestDisplayInfo> questInfos = QuestManager.Instance.GetQuestDisplayInfos();
        foreach (QuestDisplayInfo info in questInfos)
        {
            AddQuest(info);
        }
        transform.FadeOutXY();
    }

    public override void Close()
    {
        Tween tween = transform.FadeInXY();
        tween.OnComplete(() =>
        {
            base.Close();
        });
    }

    public void AddQuest(QuestDisplayInfo _info)
    {
        GameObject logObject = Instantiate(questLogPrefab);
        logObject.transform.SetParent(contentObject.transform);
        QuestLog log = logObject.GetComponent<QuestLog>();
        log.SetQuestName(_info.Title);
        log.SetQuestDescription(_info.Description);
        log.SetInfo(_info);
        questList.Add(log);
    }
}
