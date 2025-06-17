using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINextStage : UIBase
{
    const string Img_Front = "Img_Front";


    [SerializeField] BaseImage SpinImage;
    [SerializeField] OnClickImage ClickButton;

    private void Reset()
    {
        ClickButton = GetComponent<OnClickImage>();
        SpinImage = this.TryFindChild(Img_Front).GetComponent<BaseImage>();
    }

    public void Start()
    {
        OnImageMouseEnter();
        OnImageMouseExit();
    }

    public void OnClickAction(Action _Action)
    {
        ClickButton.OnClick = _Action;
    }

    void OnImageMouseEnter()
    {
        void OnMouseOn()
        {
            SpinImage.transform.RotationLoop();
        }

        ClickButton.OnMouseEnterAction = OnMouseOn;
    }

    void OnImageMouseExit()
    {
        void OnMouseOut()
        {
            SpinImage.transform.KillDoTween();
        }
        ClickButton.OnMouseExitAction = OnMouseOut;
    }

}
