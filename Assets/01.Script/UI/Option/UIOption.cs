using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOption : UIBase
{
    const string Img_Continue = "Img_Continue";
    const string Img_Exit = "Img_Exit";
    const string Img_Option = "Img_Option";

    [SerializeField] UIOptionImage Continue;
    [SerializeField] UIOptionImage Exit;
    [SerializeField] UIOptionImage Option;

    private void Reset()
    {
        Continue = this.TryFindChild(Img_Continue).GetComponent<UIOptionImage>();
        Exit = this.TryFindChild(Img_Exit).GetComponent<UIOptionImage>();
        Option = this.TryFindChild(Img_Option).GetComponent<UIOptionImage>();
    }

    private void Awake()
    {
        Continue.OnClick = UIManager.Instance.CloseUI<UIOption>;
        Option.OnClick = OnOptionClick;

        Continue.Init();
        Exit.Init();
        Option.Init();
    }

    void OnOptionClick()
    {
        UIManager.Instance.CloseUI<UIOption>();
        UIManager.Instance.OpenUI<UISoundOption>();
    }

}
