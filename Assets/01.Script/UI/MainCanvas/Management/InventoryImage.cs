using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryImage : BaseImage
{
    Sprite currentSprite;

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
