using DG.Tweening;
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

    public void IsJoin(bool _Value)
    {
        characterStatus.IsJoin(_Value);
    }

    private void Awake()
    {
        Close_Button.OnClick = CloseButtonOn;
    }

    public override void Open()
    {
        base.Open();
        List<CharacterInstance> list = CharacterManager.Instance.GetAllCharacters();

        foreach (CharacterInstance character in list)
        {
            CharacterDataSO So = CharacterData.instance.GetData(character.key);
            sprites.Add(So.characterImage);
        }
        Inventory.OnInventoryOpen(sprites);
        sprites.Clear();
        transform.FadeOutXY();
    }

    public override void Close()
    {
        Tween tween = transform.FadeInXY();
        tween.OnComplete(() =>
        {
            base.Close();
        });
    }

    public void InventoryReset(int _index)
    {
        List<CharacterInstance> list = CharacterManager.Instance.GetAllCharacters();

        foreach (CharacterInstance character in list)
        {
            CharacterDataSO So = CharacterData.instance.GetData(character.key);
            sprites.Add(So.characterImage);
        }
        Inventory.OnInventoryOpen(sprites, _index);
        sprites.Clear();
    }


    public void SetStatusView(int _index)
    {
        characterStatus.SetStatusView(_index);
    }

    void CloseButtonOn()
    {
        Close();
    }

    public void OnClickJoin(int _index)
    {
        Inventory.OnParticipate(_index);
    }

    public void OffClickJoin(int _index)
    {
        Inventory.OffParticipate(_index);
    }

    public void NoneView()
    {
        characterStatus.NoneView();
    }
}
