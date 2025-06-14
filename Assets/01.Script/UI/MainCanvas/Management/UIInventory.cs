using System;
using System.Collections.Generic;
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


    public void OnInventoryOpen(List<Sprite> _Sprites)
    {
        for (int index = 0; index < _Sprites.Count; ++index) {
            inventorySlots[index].SetSprite(_Sprites[index]);
        }

    }

}
