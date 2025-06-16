using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class UIAgentHire : UIBase
{
    const string Img_Hire = "Img_Hire";
    const string Img_Hire_Multi = "Img_Hire_Multi";
    const string Img_Return = "Img_Return";
    [SerializeField] OnClickImage HireButton;
    [SerializeField] OnClickImage HireMultiButton;
    [SerializeField] OnClickImage ReturnButton;

    UIHireScroll uiHireScroll;

    private void Reset()
    {
        HireButton = transform.Find(Img_Hire).GetComponent<OnClickImage>();
        HireMultiButton = transform.Find(Img_Hire_Multi).GetComponent<OnClickImage>();
        ReturnButton = transform.Find(Img_Return).GetComponent<OnClickImage>();
    }
    private void Awake()
    {
        HireButton.Init();
        HireMultiButton.Init();

        HireButton.OnClick = ClickGachaOneTime;
        HireMultiButton.OnClick = ClickGachaTenTime;

        GachaManager.Instance.OnCharacterDraw += Hire;

        ReturnButton.OnClick = ReturnButtonOn;
    }

    private void Start()
    {
        uiHireScroll = UIManager.Instance.GetUI<UIHireScroll>();
        if(uiHireScroll == null)
        {
            DebugHelper.Log("uihirescroll is NONO", this);
        }
        CharacterManager inst = CharacterManager.Instance;
    }

    void ClickGachaOneTime()
    {
        if(true == uiHireScroll.gameObject.activeSelf)
        {
            return;
        }

        List<DrawResult> list = GachaManager.Instance.DrawCharacter(GachaType.Normal, 1);
        if(list.Count != 0)
        {
            uiHireScroll.Open();
        }
    }

    void ClickGachaTenTime()
    {
        if (true == uiHireScroll.gameObject.activeSelf)
        {
            return;
        }

        List<DrawResult> list = GachaManager.Instance.DrawCharacter(GachaType.Normal,10);
        if (list.Count != 0)
        {
            uiHireScroll.Open();
        }
    }

    void ReturnButtonOn()
    {
        UIManager.Instance.CloseUI<UIAgentHire>();
        UIManager.Instance.OpenUI<UILobby>();
    }

    void Hire(DrawResult _Result)
    {
        uiHireScroll.AddHire(_Result);
    }
}
