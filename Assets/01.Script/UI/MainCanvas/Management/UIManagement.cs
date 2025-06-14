using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagement : UIBase
{
    [SerializeField] private OnClickImage Close_Button;

    [SerializeField] UIInventory Inventory;
    [SerializeField] CharacterStatus characterStatus;

    List<Sprite> sprites = new List<Sprite>();

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

    public override void Open()
    {
        base.Open();
        List<CharacterInstance> list = CharacterManager.instance.GetAllCharacters();

        foreach (CharacterInstance character in list)
        {
            CharacterDataSO So = CharacterData.instance.GetData(character.key);
            sprites.Add(So.characterImage);
            Inventory.OnInventoryOpen(sprites);
        }
    }


    void CloseButtonOn()
    {
        Close();
        UIManager.Instance.OpenUI<UILobby>();
    }

}
