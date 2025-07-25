using UnityEngine;
using UnityEngine.SceneManagement;
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

    const string BattleSceneName = "BattleScene";

    private void Reset()
    {
        Agent_Button = this.TryFindChild(Img_Agent).GetComponent<OnClickImage>();
        BattleField_Button = this.TryFindChild(Img_BattleField).GetComponent<OnClickImage>();
        Management_Button = this.TryFindChild(Img_Management).GetComponent<OnClickImage>();
        Shop_Button = this.TryFindChild(Img_Shop).GetComponent<OnClickImage>();
        Quest_Button = this.TryFindChild(Img_Quest).GetComponent<OnClickImage>();

        AgentBackGround = this.TryFindChild(Img_Agent_BackGround).GetComponent<Image>();
        BattleFieldBackGround = this.TryFindChild(Img_BattleField_BackGround).GetComponent<Image>();
        ManagementBackGround = this.TryFindChild(Img_Management_BackGround).GetComponent<Image>();
        ShopBackGround = this.TryFindChild(Img_Shop_BackGround).GetComponent<Image>();
        QuestBackGround = this.TryFindChild(Img_Quest_BackGround).GetComponent<Image>();
    }

    private void Awake()
    {
        Agent_Button.OnClick = OnAgentClick;
        Quest_Button.OnClick = OnQuestClick;
        BattleField_Button.OnClick = OnBattleClick;
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
        UIManager.Instance.OpenUI<UIManagement>(UIManager.Instance.GetMainCanvas());
        Management_Button.transform.KillDoTween();
        ManagementBackGround.transform.KillDoTween();
        ManagementBackGround.transform.FadeInX();
    }

    void OnQuestClick()
    {
        UIManager Manager = UIManager.Instance;
        UIQuestScroll QuestScroll = Manager.GetUI<UIQuestScroll>(Manager.GetMainCanvas());
        if (true == QuestScroll.gameObject.activeSelf)
        {
            return;
        }
        QuestScroll.Open();
        Quest_Button.transform.KillDoTween();
        QuestBackGround.transform.KillDoTween();
        QuestBackGround.transform.FadeInX();
    }

    void OnAgentClick()
    {
        UIManager.Instance.OpenUI<UIAgentHire>(UIManager.Instance.GetMainCanvas());
        Agent_Button.transform.KillDoTween();
        AgentBackGround.transform.KillDoTween();
        AgentBackGround.transform.FadeInX();
    }

    void OnBattleClick()
    {
        UIManager Manager = UIManager.Instance;
        if (1 > CharacterManager.Instance.GetParticipateCharacters().Count)
        {
            UIPopup Popup = Manager.GetUI<UIPopup>(Manager.GetMainCanvas());
            Popup.SetText("참전한 캐릭터가 없습니다");
            Popup.Open();
            return;
        }


        UIDoor uiDoor = Manager.GetUI<UIDoor>(Manager.GetMainCanvas());
        uiDoor.OnCloseAction = DoorCloseAction;
        uiDoor.OnOpenAction = DoorOpenAction;
        uiDoor.Open();
    }

    void DoorOpenAction()
    {
        UIManager Manager = UIManager.Instance;
        Transform BattleCanvasTransform = Manager.GetBattleCanvas();
        UIStage stage = Manager.GetUI<UIStage>(BattleCanvasTransform);
        stage.SetStageText(Player.Instance.Data.currentStage);
        Manager.OpenUI<UIStage>(BattleCanvasTransform);
        Manager.OpenUI<UIBattleMemberViewer>(BattleCanvasTransform);
        Manager.OpenUI<UISkillViewer>(BattleCanvasTransform);
    }

    void DoorCloseAction()
    {
        SceneManager.LoadScene(BattleSceneName);
        UIManager Manager = UIManager.Instance;
        Manager.CloseUI<UILobby>(Manager.GetMainCanvas());
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
