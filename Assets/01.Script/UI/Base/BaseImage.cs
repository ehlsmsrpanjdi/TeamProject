using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseImage : Image, IPointerEnterHandler, IPointerExitHandler
{

    protected Color prevColor;
    protected Color ChangeColor = Color.yellow;

    public Action OnMouseEnterAction;
    public Action OnMouseExitAction;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        prevColor = color;
        color = ChangeColor;
        OnMouseEnterAction?.Invoke();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        color = prevColor;
        OnMouseExitAction?.Invoke();
    }
}
