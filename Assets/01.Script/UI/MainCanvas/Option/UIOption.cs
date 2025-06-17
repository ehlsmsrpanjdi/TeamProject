using UnityEngine;
using UnityEngine.SceneManagement;

public class UIOption : UIBase
{
    const string Img_Continue = "Img_Continue";
    const string Img_Exit = "Img_Exit";
    const string Img_Option = "Img_Option";

    [SerializeField] OnClickImage Continue;
    [SerializeField] OnClickImage Exit;
    [SerializeField] OnClickImage Option;

    private void Reset()
    {
        Continue = this.TryFindChild(Img_Continue).GetComponent<OnClickImage>();
        Exit = this.TryFindChild(Img_Exit).GetComponent<OnClickImage>();
        Option = this.TryFindChild(Img_Option).GetComponent<OnClickImage>();
    }

    private void Awake()
    {
        Continue.OnClick = OnContinueButtonClick;
        Option.OnClick = OnOptionClick;
        Exit.OnClick = OnExitClick;

        Continue.Init();
        Exit.Init();
        Option.Init();
    }

    const string BattleScene = "BattleScene";
    const string SampleScene = "SampleScene";

    void OnExitClick()
    {
        UIManager Manager = UIManager.Instance;
        UIDoor uiDoor = Manager.GetUI<UIDoor>(Manager.GetMainCanvas());
        string SceneName = SceneManager.GetActiveScene().name;
        Close();
        if (SceneName == BattleScene)
        {
            uiDoor.OnCloseAction = () =>
            {
                SceneManager.LoadScene(SampleScene);
                Manager.OpenUI<UILobby>(Manager.GetMainCanvas());
                Manager.GetUI<UIStatus>(Manager.GetMainCanvas()).transform.SetAsLastSibling();
                Manager.CloseUI<UISkillViewer>(Manager.GetBattleCanvas());
                Manager.CloseUI<UIStage>(Manager.GetBattleCanvas());
                Manager.CloseUI<UIBattleMemberViewer>(Manager.GetBattleCanvas());
            };
            uiDoor.Open();
        }

        if (SceneName == SampleScene)
        {
            uiDoor.OnCloseAction = () =>
            {
#if UNITY_EDITOR
                // 에디터에서는 Play 모드를 종료
                UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
            };
            uiDoor.Open();
        }
    }


    void OnContinueButtonClick()
    {
        UIManager.Instance.CloseUI<UIOption>(UIManager.Instance.GetMainCanvas());
    }

    public override void Open()
    {
        base.Open();
        transform.FadeOutXY();
    }

    public override void Close()
    {
        base.Close();
    }

    void OnOptionClick()
    {
        UIManager.Instance.CloseUI<UIOption>(UIManager.Instance.GetMainCanvas());
        UIManager.Instance.OpenUI<UISoundOption>(UIManager.Instance.GetMainCanvas());
    }

}
