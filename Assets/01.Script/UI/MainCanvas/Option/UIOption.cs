using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Continue.OnClick = UIManager.Instance.CloseUI<UIOption>;
        Option.OnClick = OnOptionClick;

        Continue.Init();
        Exit.Init();
        Option.Init();
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
        UIManager.Instance.CloseUI<UIOption>();
        UIManager.Instance.OpenUI<UISoundOption>();
    }

}
