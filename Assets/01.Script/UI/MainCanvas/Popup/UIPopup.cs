using DG.Tweening;
using System.ComponentModel.Design.Serialization;
using TMPro;
using UnityEngine;

public class UIPopup : UIBase
{
    [SerializeField] OnClickImage ConfirmButton;
    [SerializeField] TextMeshProUGUI PopupDiscription;

    const string PopupText = "PopupText";
    const string Img_Confirm = "Img_Confirm";

    private void Reset()
    {
        PopupDiscription = this.TryFindChild(PopupText).GetComponent<TextMeshProUGUI>();
        ConfirmButton = this.TryFindChild(Img_Confirm).GetComponent<OnClickImage>();
    }

    private void Awake()
    {
        if (ConfirmButton != null)
        {
            ConfirmButton.OnClick = OnClickButton;
        }
    }

    public void SetText(string text)
    {
        PopupDiscription.text = text;
    }

    public override void Open()
    {
        base.Open();
        transform.FadeOutXY();
    }

    void OnClickButton()
    {
        Tween tween = transform.FadeInXY();
        tween.OnComplete(() =>
        {
            Close();
        });
    }
}
