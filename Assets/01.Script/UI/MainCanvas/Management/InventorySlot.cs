using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [field: SerializeField] public int index { get; protected set; } = 0;
    [SerializeField] InventoryImage inventoryImage;
    [SerializeField] Image EquipImg;
    [SerializeField] Sprite NoneSprite;

    const string Img_None = "UI/Img_None";

    const string Img_Equip = "Img_Equip";
    public void Reset()
    {
        inventoryImage = GetComponent<InventoryImage>();
        EquipImg = this.TryFindChild(Img_Equip).GetComponent<Image>();

        inventoryImage.SetSprite(NoneSprite);
    }

    public void Init(int _index)
    {
        index = _index;
    }

    private void Awake()
    {
        if(inventoryImage == null)
        {
            inventoryImage = GetComponent<InventoryImage>();
        }
        if (inventoryImage == null)
        {
            inventoryImage.NoneSprite = NoneSprite;
        }
        inventoryImage.SetSprite(NoneSprite);
    }

    public void ResetSprite()
    {
        inventoryImage.SetSprite(NoneSprite);
    }

    private void Start()
    {
        if(NoneSprite == null)
        {
            NoneSprite = Resources.Load<Sprite>(Img_None);
        }

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
        UIManagement management = UIManager.Instance.GetUI<UIManagement>(UIManager.Instance.GetMainCanvas());
        management.SetStatusView(index);
        bool IsParticipated = CharacterManager.Instance.IsParticipating(index);
        management.IsJoin(IsParticipated);
    }
}
