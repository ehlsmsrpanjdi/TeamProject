using DG.Tweening;
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
    [SerializeField] BackGroundHelper backGroundHelper;

    UIHireScroll uiHireScroll;

    private void Reset()
    {
        HireButton = this.TryFindChild(Img_Hire).GetComponent<OnClickImage>();
        HireMultiButton = this.TryFindChild(Img_Hire_Multi).GetComponent<OnClickImage>();
        ReturnButton = this.TryFindChild(Img_Return).GetComponent<OnClickImage>();
        backGroundHelper = gameObject.transform.parent.GetComponent<BackGroundHelper>();
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
        if (uiHireScroll == null)
        {
            DebugHelper.Log("uihirescroll is NONO", this);
        }
        CharacterManager inst = CharacterManager.Instance;

        OnImageMouseEnter(HireButton);
        OnImageMouseEnter(HireMultiButton);

        OnImageMouseExit(HireButton);
        OnImageMouseExit(HireMultiButton);
    }

    void ClickGachaOneTime()
    {
        if (true == uiHireScroll.gameObject.activeSelf)
        {
            return;
        }

        List<DrawResult> list = GachaManager.Instance.DrawCharacter(1);

        if (list.Count != 0)
        {
            UIManager.Instance.OpenUI<UIGacha>();
        }

    }

    void ClickGachaTenTime()
    {
        if (true == uiHireScroll.gameObject.activeSelf)
        {
            return;
        }

        List<DrawResult> list = GachaManager.Instance.DrawCharacter(10);

        if (list.Count != 0)
        {
            UIManager.Instance.OpenUI<UIGacha>();
        }

        UIManager.Instance.OpenUI<UIGacha>();
    }

    void ReturnButtonOn()
    {
        UIManager.Instance.CloseUI<UIAgentHire>();
    }

    void Hire(DrawResult _Result)
    {
        uiHireScroll.AddHire(_Result);
    }

    void OnImageMouseEnter(BaseImage _Image)
    {
        void OnMouseOn()
        {
            _Image.transform.RotationLoop();
        }

        _Image.OnMouseEnterAction = OnMouseOn;
    }

    void OnImageMouseExit(BaseImage _Image)
    {
        void OnMouseOut()
        {
            _Image.transform.KillDoTween();
            _Image.transform.rotation = Quaternion.identity;
        }
        _Image.OnMouseExitAction = OnMouseOut;
    }

    public override void Open()
    {
        base.Open();
        backGroundHelper.gameObject.SetActive(true);
        transform.FadeOutXY();
    }

    public override void Close()
    {
        Tween tween = transform.FadeInXY();
        tween.OnComplete(() =>
        {
            base.Close();
            backGroundHelper.gameObject.SetActive(false);
        });
    }
}
