using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseImage : Image, IPointerEnterHandler, IPointerExitHandler
{

    Color prevColor;
    Color ChangeColor = Color.yellow;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        prevColor = color;
        color = ChangeColor;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        color = prevColor;
    }
}
