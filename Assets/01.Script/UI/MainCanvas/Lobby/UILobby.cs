using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : UIBase
{
    [SerializeField] OnClickImage Agent_Button;
    [SerializeField] OnClickImage BattleField_Button;
    [SerializeField] OnClickImage Management_Button;
    [SerializeField] OnClickImage Shop_Button;
    [SerializeField] OnClickImage Quest_Button;

    [SerializeField] Image AgentBackGround;
    [SerializeField] Image BattleFieldBackGround;
    [SerializeField] Image ManagementBackGround;
    [SerializeField] Image ShopBackGround;
    [SerializeField] Image QuestBackGround;

    const string Img_Agent = "Img_Agent";
    const string Img_BattleField = "Img_BattleField";
    const string Img_Management = "Img_Management";
    const string Img_Shop = "Img_Shop";
    const string Img_Quest = "Img_Quest";

    const string Img_Agent_BackGround = "Img_Agent_BackGround";
    const string Img_BattleField_BackGround = "Img_BattleField_BackGround";
    const string Img_Management_BackGround = "Img_Management_BackGround";
    const string Img_Shop_BackGround = "Img_Shop_BackGround";
    const string Img_Quest_BackGround = "Img_Quest_BackGround";

    private void Reset()
    {
        Agent_Button = this.TryFindChild(Img_Agent).GetComponent<OnClickImage>();
        BattleField_Button = this.TryFindChild(Img_BattleField).GetComponent<OnClickImage>();
        Management_Button = this.TryFindChild(Img_Management).GetComponent<OnClickImage>();
        Shop_Button = this.TryFindChild(Img_Shop).GetComponent<OnClickImage>();
        Quest_Button = this.TryFindChild(Img_Quest).GetComponent<OnClickImage>();

        AgentBackGround = this.TryFindChild(Img_Agent_BackGround).GetComponent<Image>();
        BattleFieldBackGround= this.TryFindChild(Img_BattleField_BackGround).GetComponent<Image>();
        ManagementBackGround= this.TryFindChild(Img_Management_BackGround).GetComponent<Image>();
        ShopBackGround= this.TryFindChild(Img_Shop_BackGround).GetComponent<Image>();
        QuestBackGround= this.TryFindChild(Img_Quest_BackGround).GetComponent<Image>();
    }

    private void Awake()
    {
        Agent_Button.OnClick = OnAgentClick;
        Quest_Button.OnClick = UIManager.Instance.OpenUI<UIQuestScroll>;
        //BattleField_Button .OnClick = UIManager.Instance.OpenUI<UIBattleField>;
        Management_Button.OnClick = OnManagementClick;
        //Shop_Button .OnClick = UIManager.Instance.OpenUI<UIShop>;

    }

    private void Start()
    {
        OnLobbyImageMouseEnter(Agent_Button, AgentBackGround);
        OnLobbyImageMouseEnter(BattleField_Button, BattleFieldBackGround);
        OnLobbyImageMouseEnter(Management_Button, ManagementBackGround);
        OnLobbyImageMouseEnter(Shop_Button, ShopBackGround);
        OnLobbyImageMouseEnter(Quest_Button, QuestBackGround);

        OnLobbyImageMouseExit(Agent_Button, AgentBackGround);
        OnLobbyImageMouseExit(BattleField_Button, BattleFieldBackGround);
        OnLobbyImageMouseExit(Management_Button, ManagementBackGround);
        OnLobbyImageMouseExit(Shop_Button, ShopBackGround);
        OnLobbyImageMouseExit(Quest_Button, QuestBackGround);
    }

    void OnManagementClick()
    {
        UIManager.Instance.OpenUI<UIManagement>();
        UIManager.Instance.CloseUI<UILobby>();
    }

    void OnAgentClick()
    {
        UIManager.Instance.OpenUI<UIAgentHire>();
        UIManager.Instance.CloseUI<UILobby>();
    }

    void OnLobbyImageMouseEnter(BaseImage _Image, Image _BackgroundImg)
    {
        void OnMouseOn()
        {
            _Image.transform.RotationLoop();
            _BackgroundImg.transform.KillDoTween();
            _BackgroundImg.transform.FadeOutX();
        }

        _Image.OnMouseEnterAction = OnMouseOn;
    }

    void OnLobbyImageMouseExit(BaseImage _Image, Image _BackgroundImg)
    {
        void OnMouseOut()
        {
            _Image.transform.KillDoTween();
            _BackgroundImg.transform.KillDoTween();
            _BackgroundImg.transform.FadeInX();
        }

        _Image.OnMouseExitAction = OnMouseOut;
    }
}
