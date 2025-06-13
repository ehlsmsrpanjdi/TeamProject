using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    [SerializeField] OnClickImage ConfirmButton;

    const string QuestNameText = "QuestNameText";
    const string QuestDescriptionText = "QuestDescriptionText";

    [SerializeField] TextMeshProUGUI QuestName;
    [SerializeField] TextMeshProUGUI QuestDescription;

    private void Reset()
    {
        QuestName = transform.Find(QuestNameText).GetComponent<TextMeshProUGUI>();
        QuestDescription = transform.Find(QuestDescriptionText).GetComponent<TextMeshProUGUI>();
        ConfirmButton = GetComponentInChildren<OnClickImage>();
    }

    public void SetQuestName(string name)
    {
        QuestName.text = name;
    }

    public void SetQuestDescription(string description)
    {
        QuestDescription.text = description;
    }

    public void SetQuestAction(Action _Action)
    {
        ConfirmButton.OnClick = _Action;
    }
}
