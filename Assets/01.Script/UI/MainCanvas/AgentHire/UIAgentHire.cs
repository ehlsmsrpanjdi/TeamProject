using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAgentHire : UIBase
{
    const string Img_Hire = "Img_Hire";
    const string Img_Hire_Multi = "Img_Hire_Multi";
    const string Img_Return = "Img_Return";
    [SerializeField] OnClickImage HireButton;
    [SerializeField] OnClickImage HireMultiButton;
    [SerializeField] OnClickImage ReturnButton;
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

        HireButton.OnClick = Hire;
        HireMultiButton.OnClick = HireMulti;

        ReturnButton.OnClick = ReturnButtonOn;
    }

    void ReturnButtonOn()
    {
        UIManager.Instance.CloseUI<UIAgentHire>();
        UIManager.Instance.OpenUI<UILobby>();
    }

    void HireMulti()
    {
        UIManager.Instance.OpenUI<UIHireScroll>();
    }
    void Hire()
    {
        UIManager.Instance.OpenUI<UIHireScroll>();
    }
}
