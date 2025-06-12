using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagement : UIBase
{
    [SerializeField] private OnClickImage Close_Button;

    const string Img_Close = "Img_Close";

    private void Reset()
    {
        Close_Button = this.TryFindChild(Img_Close).GetComponent<OnClickImage>();
    }

    private void Awake()
    {
        Close_Button.OnClick = Close;
    }
}
