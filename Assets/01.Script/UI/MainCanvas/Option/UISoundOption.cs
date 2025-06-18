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

    private void Start()
    {
        MusicSlider[0].value = SoundManager.Instance.MasterVolume;
        MusicSlider[1].value = SoundManager.Instance.BgmVolume;
        MusicSlider[2].value = SoundManager.Instance.SfxVolume;

        MusicSlider[0].onValueChanged.AddListener(SoundManager.Instance.SetMasterVolume);
        MusicSlider[1].onValueChanged.AddListener(SoundManager.Instance.SetBgmVolume);
        MusicSlider[2].onValueChanged.AddListener(SoundManager.Instance.SetSfxVolume);
    }

    public override void Open()
    {
        base.Open();
        transform.FadeOutXY();
        transform.SetAsLastSibling();
    }

    void ReturnButtonOn()
    {
        UIManager.Instance.CloseUI<UISoundOption>(UIManager.Instance.GetMainCanvas());
        UIManager.Instance.OpenUI<UIOption>(UIManager.Instance.GetMainCanvas());
    }

    public override void Close()
    {
        base.Close();
    }
}
