using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    [SerializeField] List<InventorySlot> inventorySlots = new List<InventorySlot>();

    private void Reset()
    {
        inventorySlots = new List<InventorySlot>(GetComponentsInChildren<InventorySlot>(true));
        for (int i = 0; i < inventorySlots.Count; ++i)
        {
            inventorySlots[i].Init(i);
            inventorySlots[i].Reset();
        }
    }

    public void OnParticipate(int _index)
    {
        inventorySlots[_index].OnParticipate();
    }

    public void OffParticipate(int _index)
    {
        inventorySlots[_index].OffParticipate();
    }

    public void OnInventoryOpen(List<Sprite> _Sprites, int _index = 0)
    {
        List<int> participates = CharacterManager.Instance.GetParticipateCharactersAsDictionary().Keys.ToList<int>();

        for (int index = 0; index < _Sprites.Count; ++index)
        {
            inventorySlots[index].SetSprite(_Sprites[index]);
        }

        foreach (int participatedIndex in participates)
        {
            inventorySlots[participatedIndex].OnParticipate();
        }

        if (true == CharacterManager.Instance.IsParticipating(_index))
        {
            inventorySlots[_index].OnClickSlot();
        }
        else
        {
            UIManagement management = UIManager.Instance.GetUI<UIManagement>(UIManager.Instance.GetMainCanvas());
            management.NoneView();
        }
    }
}
