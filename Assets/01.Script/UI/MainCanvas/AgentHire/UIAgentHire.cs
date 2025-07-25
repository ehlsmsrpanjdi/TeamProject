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

    UIHireScroll uiHireScroll;

    private void Reset()
    {
        HireButton = this.TryFindChild(Img_Hire).GetComponent<OnClickImage>();
        HireMultiButton = this.TryFindChild(Img_Hire_Multi).GetComponent<OnClickImage>();
        ReturnButton = this.TryFindChild(Img_Return).GetComponent<OnClickImage>();
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
        uiHireScroll = UIManager.Instance.GetUI<UIHireScroll>(UIManager.Instance.GetMainCanvas());
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
        if (true == UIManager.Instance.GetUI<UIGacha>(UIManager.Instance.GetMainCanvas()).gameObject.activeSelf)
        {
            return;
        }
        if (true == uiHireScroll.gameObject.activeSelf)
        {
            return;
        }


        List<DrawResult> list = GachaManager.Instance.DrawCharacter(GachaType.Normal, 1);

        bool IsOverRank = false;
        foreach (DrawResult drawResult in list)
        {
            IsOverRank = GachaManager.Instance.IsOverSRank(drawResult);
        }
        UIManager Manager = UIManager.Instance;

        if (list.Count != 0)
        {
            UIGacha Gacha = Manager.GetUI<UIGacha>(Manager.GetMainCanvas());
            GachaCamera GachaComponent = Gacha.GetGachaCamera();
            if (true == IsOverRank)
            {
                Gacha.SetBox(GachaComponent.SpawnHighBox());
                Gacha.Open();
            }
            else
            {
                Gacha.SetBox(GachaComponent.SpawnNormalBox());
                Gacha.Open();
            }
        }
        else
        {
            UIPopup Popup = Manager.GetUI<UIPopup>(UIManager.Instance.GetMainCanvas());
            Popup.SetText("재화가 모자릅니다");
            Popup.Open();
        }

    }

    void ClickGachaTenTime()
    {
        if (true == UIManager.Instance.GetUI<UIGacha>(UIManager.Instance.GetMainCanvas()).gameObject.activeSelf)
        {
            return;
        }
        if (true == uiHireScroll.gameObject.activeSelf)
        {
            return;
        }

        List<DrawResult> list = GachaManager.Instance.DrawCharacter(GachaType.Normal, 10);

        bool IsOverRank = false;
        foreach (DrawResult drawResult in list)
        {
            IsOverRank = GachaManager.Instance.IsOverSRank(drawResult);
            if(true == IsOverRank)
            {
                break;
            }
        }
        UIManager Manager = UIManager.Instance;

        if (list.Count != 0)
        {
            UIGacha Gacha = Manager.GetUI<UIGacha>(Manager.GetMainCanvas());
            GachaCamera GachaComponent = Gacha.GetGachaCamera();
            if (true == IsOverRank)
            {
                Gacha.SetBox(GachaComponent.SpawnHighBox());
                Gacha.Open();
            }
            else
            {
                Gacha.SetBox(GachaComponent.SpawnNormalBox());
                Gacha.Open();
            }
        }
        else
        {
            UIPopup Popup = UIManager.Instance.GetUI<UIPopup>(UIManager.Instance.GetMainCanvas());
            Popup.SetText("재화가 모자릅니다");
            Popup.Open();
        }
    }

    void ReturnButtonOn()
    {
        UIManager.Instance.CloseUI<UIAgentHire>(UIManager.Instance.GetMainCanvas());
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
        transform.FadeOutXY();
    }

    public override void Close()
    {
        Tween tween = transform.FadeInXY();
        tween.OnComplete(() =>
        {
            base.Close();
        });
    }
}
