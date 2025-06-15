using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISoundOption : UIBase
{
    [SerializeField] Slider[] MusicSlider;

    [SerializeField] OnClickImage Return;
    [SerializeField] BackGroundHelper backGroundHelper;

    private void Reset()
    {
        MusicSlider = GetComponentsInChildren<Slider>();

        Return = this.TryFindChild("Img_Return").GetComponent<OnClickImage>();
        backGroundHelper = gameObject.transform.parent.GetComponent<BackGroundHelper>();
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
        backGroundHelper.gameObject.SetActive(true);
    }

    void ReturnButtonOn()
    {
        UIManager.Instance.CloseUI<UISoundOption>();
        UIManager.Instance.OpenUI<UIOption>();
    }

    public override void Close()
    {
        base.Close();
        backGroundHelper.gameObject.SetActive(false);
    }
}
