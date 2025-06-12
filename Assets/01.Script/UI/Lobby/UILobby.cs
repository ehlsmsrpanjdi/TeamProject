using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : UIBase
{
    [SerializeField] OnClickImage Agent_Button;
    [SerializeField] OnClickImage BattleField_Button;
    [SerializeField] OnClickImage Management_Button;
    [SerializeField] OnClickImage Shop_Button;
    [SerializeField] OnClickImage Quest_Button;

    const string Img_Agent = "Img_Agent";
    const string Img_BattleField = "Img_BattleField";
    const string Img_Management = "Img_Management";
    const string Img_Shop = "Img_Shop";
    const string Img_Quest = "Img_Quest";

    private void Reset()
    {
        Agent_Button = this.TryFindChild(Img_Agent).GetComponent<OnClickImage>();
        BattleField_Button = this.TryFindChild(Img_BattleField).GetComponent<OnClickImage>();
        Management_Button = this.TryFindChild(Img_Management).GetComponent<OnClickImage>();
        Shop_Button = this.TryFindChild(Img_Shop).GetComponent<OnClickImage>();
        Quest_Button = this.TryFindChild(Img_Quest).GetComponent<OnClickImage>();
    }

    private void Awake()
    {
        Agent_Button.OnClick = UIManager.Instance.OpenUI<UIAgentHire>;
        Quest_Button.OnClick = UIManager.Instance.OpenUI<UIQuestScroll>;
        //BattleField_Button .OnClick = UIManager.Instance.OpenUI<UIBattleField>;
        Management_Button.OnClick = OnManagementClick;
        //Shop_Button .OnClick = UIManager.Instance.OpenUI<UIShop>;
    }

    void OnManagementClick()
    {
        UIManager.Instance.OpenUI<UIManagement>();
        //UIManager.Instance.CloseUI<UIStatus>(); 

    }
}
