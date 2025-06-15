using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISoundOption : UIBase
{
    [SerializeField] Slider[] MusicSlider;

    [SerializeField] OnClickImage Return;

    private void Reset()
    {
        MusicSlider = GetComponentsInChildren<Slider>();

        Return = this.TryFindChild("Img_Return").GetComponent<OnClickImage>();
    }

    private void Awake()
    {
        Return.Init();
        Return.OnClick = ReturnButtonOn;
    }

    public override void Open()
    {
        base.Open();
        transform.FadeOutXY();
    }

    void ReturnButtonOn()
    {
        UIManager.Instance.CloseUI<UISoundOption>();
        UIManager.Instance.OpenUI<UIOption>();
    }

    public override void Close()
    {
        base.Close();
    }
}
