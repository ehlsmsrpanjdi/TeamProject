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

    [SerializeField] BackGroundHelper backGroundHelper;

    const string QuestLog = "UI/QuestLog";

    private void Reset()
    {
        questLogPrefab = Resources.Load<GameObject>(QuestLog);
        contentObject = gameObject.GetComponentInChildren<HorizontalLayoutGroup>(true).gameObject;

        ReturnButton = GetComponentInChildren<OnClickImage>();

        backGroundHelper = gameObject.transform.parent.GetComponent<BackGroundHelper>();
    }

    private void Awake()
    {
        if (null == questLogPrefab)
        {
            Resources.Load<GameObject>(QuestLog);
        }
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
        transform.FadeOutXY();
        backGroundHelper.gameObject.SetActive(true);
    }

    public override void Close()
    {
        Tween tween = transform.FadeInXY();
        tween.OnComplete(() =>
        {
            base.Close();
            backGroundHelper.gameObject.SetActive(false);
        });
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
