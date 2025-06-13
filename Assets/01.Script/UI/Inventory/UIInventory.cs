using System;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    [SerializeField] List<InventoryImage> inventoryImages = new List<InventoryImage>();

    private void Reset()
    {
        inventoryImages = new List<InventoryImage>(GetComponentsInChildren<InventoryImage>(true));

        for(int i = 0; i < inventoryImages.Count; ++i)
        {
            inventoryImages[i].Init(i);
        }
    }

    public void SetSprite()
    {
        //List<DrawResult> CharacterDatas = CharacterInventory.Instance.GetOwnedCharacters();
        //for (int CharacterCount = 0; CharacterCount < CharacterDatas.Count; ++CharacterCount)
        //{
        //    inventoryImages[CharacterCount].SetSprite(CharacterDatas[CharacterCount].character.Sprite);
        //}
    }

    private void Awake()
    {

    }
}
