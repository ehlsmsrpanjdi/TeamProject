using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryImage : BaseImage, IPointerClickHandler
{
    public Action OnClickAction;
    [field : SerializeField] public Sprite NoneSprite { get; set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickAction?.Invoke();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }

    public void SetSprite(Sprite sprite)
    {
        if (sprite != null)
        {
            this.sprite = sprite;
        }
    }
}
