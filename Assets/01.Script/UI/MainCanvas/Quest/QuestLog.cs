using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    [SerializeField] OnClickImage ConfirmButton;
    QuestDisplayInfo quest_info;

    const string QuestNameText = "QuestNameText";
    const string QuestDescriptionText = "QuestDescriptionText";
    const string Img_Btn = "Img_Btn";
    const string RemainText = "RemainText";
    const string TotalText = "TotalText";

    [SerializeField] TextMeshProUGUI QuestName;
    [SerializeField] TextMeshProUGUI QuestDescription;
    [SerializeField] TextMeshProUGUI RemainValueText;
    [SerializeField] TextMeshProUGUI TotalValueText;

    private void Reset()
    {
        QuestName = this.TryFindChild(QuestNameText).GetComponent<TextMeshProUGUI>();
        QuestDescription = this.TryFindChild(QuestDescriptionText).GetComponent<TextMeshProUGUI>();
        ConfirmButton = this.TryFindChild(Img_Btn).GetComponent<OnClickImage>();
        RemainValueText = this.TryFindChild(RemainText).GetComponent<TextMeshProUGUI>();
        TotalValueText = this.TryFindChild(TotalText).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        if (quest_info != null)
        {
            ConfirmButton.OnClick = SetQuestAction;
            if (true == quest_info.IsClaimed)
            {
                GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            }
        }
    }

    public void SetClaimedColor()
    {
        GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
    }

    public void SetQuestName(string name)
    {
        QuestName.text = name;
    }

    public void SetQuestDescription(string description)
    {
        QuestDescription.text = description;
    }

    public void SetInfo(QuestDisplayInfo _info)
    {
        quest_info = _info;
    }

    public void SetValue(QuestDisplayInfo _info)
    {
        RemainValueText.text = _info.CurrentValue.ToString();
        TotalValueText.text = "/   " + _info.TargetValue.ToString();
    }

    public void SetQuestAction()
    {
        UIManager Manager = UIManager.Instance;
        if (!quest_info.IsCompleted)
        {
            UIPopup Popup = Manager.GetUI<UIPopup>(Manager.GetMainCanvas());
            Popup.SetText("조건을 만족하지 못했습니다");
            Popup.Open();
            return;
        }
        if (quest_info.IsClaimed)
        {
            UIPopup Popup = Manager.GetUI<UIPopup>(Manager.GetMainCanvas());
            Popup.SetText("이미 보상을 수령했습니다");
            Popup.Open();
            return;
        }
        QuestManager.Instance.ClaimReward(quest_info.Id);
        GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
    }
}
