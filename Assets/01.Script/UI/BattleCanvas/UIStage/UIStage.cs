using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIStage : UIBase
{
    [SerializeField] TextMeshProUGUI stageText;

    private void Reset()
    {
        stageText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetStageText(int stage)
    {
        if (stageText != null)
        {
            stageText.text = $"Stage : {stage}";
        }
    }
}
