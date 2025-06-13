using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryImage : BaseImage
{
    [SerializeField] public int Index { get; protected set; }

    public void Init(int _Index)
    {
        Index = _Index;
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
            SetNativeSize();
        }
    }
}
