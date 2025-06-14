using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [field: SerializeField] public int index { get; protected set; } = 0;
    [SerializeField] InventoryImage inventoryImage;
    public void Reset()
    {
        inventoryImage = GetComponent<InventoryImage>();
    }

    public void Init(int _index)
    {
        index = _index;
    }

    public void SetSprite(Sprite _sprite)
    {
        inventoryImage.SetSprite(_sprite);
    }
}
