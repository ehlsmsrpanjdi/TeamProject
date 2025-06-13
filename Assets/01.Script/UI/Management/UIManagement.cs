using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagement : UIBase
{
    [SerializeField] private OnClickImage Close_Button;

    [SerializeField] UIInventory Inventory;
    [SerializeField] CharacterStatus characterStatus;

    const string Img_Close = "Img_Close";

    private void Reset()
    {
        Close_Button = this.TryFindChild(Img_Close).GetComponent<OnClickImage>();
        Inventory = GetComponentInChildren<UIInventory>(true);
        characterStatus = GetComponentInChildren<CharacterStatus>(true);
    }

    private void Awake()
    {
        Close_Button.OnClick = CloseButtonOn;
    }

    void CloseButtonOn()
    {
        Close();
        UIManager.Instance.OpenUI<UILobby>();
    }

}
