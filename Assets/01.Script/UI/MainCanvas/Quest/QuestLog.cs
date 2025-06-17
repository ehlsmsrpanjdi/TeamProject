using System;
using TMPro;
using UnityEngine;

public class QuestLog : MonoBehaviour
{
    [SerializeField] OnClickImage ConfirmButton;
    QuestDisplayInfo quest_info;

    const string QuestNameText = "QuestNameText";
    const string QuestDescriptionText = "QuestDescriptionText";
    const string Img_Btn = "Img_Btn";

    [SerializeField] TextMeshProUGUI QuestName;
    [SerializeField] TextMeshProUGUI QuestDescription;

    private void Reset()
    {
        QuestName = this.TryFindChild(QuestNameText).GetComponent<TextMeshProUGUI>();
        QuestDescription = this.TryFindChild(QuestDescriptionText).GetComponent<TextMeshProUGUI>();
        ConfirmButton = this.TryFindChild(Img_Btn).GetComponent<OnClickImage>();
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

    public void SetQuestAction()
    {
        QuestManager.Instance.ClaimReward(quest_info.Id);
    }
}
