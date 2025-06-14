using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [field: SerializeField] public int index { get; protected set; } = 0;
    [SerializeField] InventoryImage inventoryImage;
    [SerializeField] Image EquipImg;

    const string Img_Equip = "Img_Equip";
    public void Reset()
    {
        inventoryImage = GetComponent<InventoryImage>();
        EquipImg = this.TryFindChild(Img_Equip).GetComponent<Image>();
    }

    public void Init(int _index)
    {
        index = _index;
    }

    private void Awake()
    {
        inventoryImage.OnClickAction = OnClickSlot;
        EquipImg.gameObject.SetActive(false);
    }

    public void OnParticipate()
    {
        EquipImg.gameObject.SetActive(true);
    }

    public void OffParticipate()
    {
        EquipImg.gameObject.SetActive(false);
    }

    public void SetSprite(Sprite _sprite)
    {
        inventoryImage.SetSprite(_sprite);
    }

    public void OnClickSlot()
    {
        UIManager.Instance.GetUI<UIManagement>().SetStatusView(index);
    }
}
