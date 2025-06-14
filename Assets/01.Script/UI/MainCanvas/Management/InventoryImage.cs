using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryImage : BaseImage, IPointerClickHandler
{
    Sprite currentSprite;
    public Action OnClickAction;

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
            currentSprite = this.sprite;
            this.sprite = sprite;
            SetNativeSize();
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.sprite = currentSprite;
    }
}
