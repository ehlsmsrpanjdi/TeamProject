using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnClickImage : BaseImage, IPointerClickHandler
{
    public Action OnClick;

    public void Init()
    {
        ChangeColor = Color.yellow;
    }

    public void NoHoverColor()
    {
        ChangeColor = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
        color = prevColor;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }
}
