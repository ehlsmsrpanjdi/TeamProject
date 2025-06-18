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
            foreach (QuestLog Log in questList)
            {
                Destroy(Log.gameObject);
            }
            questList.Clear();
            base.Close();
        });
    }
    //
    public void AddQuest(QuestDisplayInfo _info)
    {
        GameObject logObject = Instantiate(questLogPrefab);
        logObject.transform.SetParent(contentObject.transform);
        logObject.transform.localScale = new Vector3(1, 1, 1);
        QuestLog log = logObject.GetComponent<QuestLog>();
        log.SetQuestName(_info.Title);
        log.SetQuestDescription(_info.Description);
        log.SetInfo(_info);
        log.SetValue(_info);
        questList.Add(log);
    }

    public void SetQuest(QuestDisplayInfo _info, QuestLog _Log)
    {
        if(_info.IsClaimed == true)
        {
            _Log.SetClaimedColor();
        }
        _Log.SetQuestName(_info.Title);
        _Log.SetQuestDescription(_info.Description);
        _Log.SetInfo(_info);
        _Log.SetValue(_info);
    }

    public void ResetQuest()
    {
        List<QuestDisplayInfo> questInfos = QuestManager.Instance.GetQuestDisplayInfos();
    }
}
