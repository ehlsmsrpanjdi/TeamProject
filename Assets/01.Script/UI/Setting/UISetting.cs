using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : UIBase
{
    [SerializeField] Slider[] MusicSlider;

    private void Reset()
    {
        MusicSlider = GetComponentsInChildren<Slider>();
    }

    public void Start()
    {
        MusicSlider[0].onValueChanged.AddListener(Test0);
        MusicSlider[1].onValueChanged.AddListener(Test1);
        MusicSlider[2].onValueChanged.AddListener(Test2);
    }

    public void Test0(float value)
    {
        Debug.Log("total: " + value);
    }

    public void Test1(float value)
    {
        Debug.Log("bgm: " + value);
    }

    public void Test2(float value)
    {
        Debug.Log("sfx: " + value);
    }

}
