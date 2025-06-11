using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    [SerializeField] Image ConfirmButton;

    const string QuestNameText = "QuestNameText";
    const string QuestDescriptionText = "QuestDescriptionText";

    [SerializeField] TextMeshProUGUI QuestName;
    [SerializeField] TextMeshProUGUI QuestDescription;

    private void Reset()
    {
        QuestName = GameObject.Find(QuestNameText).GetComponent<TextMeshProUGUI>();
        QuestDescription = GameObject.Find(QuestNameText).GetComponent<TextMeshProUGUI>();
        ConfirmButton = GetComponentInChildren<Image>();
    }

    public void SetQuestName(string name)
    {
        QuestName.text = name;
    }

    public void SetQuestDescription(string description)
    {
        QuestDescription.text = description;
    }

}
