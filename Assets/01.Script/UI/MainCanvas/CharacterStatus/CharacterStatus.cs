using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatus : MonoBehaviour
{
    const string Attack_Value_Text = "Attack_Value_Text";
    const string Hp_Value_Text = "Hp_Value_Text";

    const string Img_Weapon = "Img_Weapon";
    const string Img_Awaken = "Img_Awaken";

    const string Img_WeaponImage = "Img_WeaponImage";
    const string Img_AwakenImage = "Img_AwakenImage";

    const string Img_Character = "Img_Character";

    const string Img_Join = "Img_Join";

    [SerializeField] TextMeshProUGUI Attack_Text;
    [SerializeField] TextMeshProUGUI HP_Text;

    [SerializeField] OnClickImage WeaponUpgrade_Button;
    [SerializeField] OnClickImage AwakenUpgrade_Button;

    [SerializeField] Image WeaponImage;
    [SerializeField] Image AwakenImage;

    [SerializeField] Image CharacterImage;

    [SerializeField] OnClickImage Join_Button;

    int Selected_Index;


    private void Reset()
    {
        Attack_Text = this.TryFindChild(Attack_Value_Text).GetComponent<TextMeshProUGUI>();
        HP_Text = this.TryFindChild(Hp_Value_Text).GetComponent<TextMeshProUGUI>();

        WeaponUpgrade_Button = this.TryFindChild(Img_Weapon).GetComponent<OnClickImage>();
        AwakenUpgrade_Button = this.TryFindChild(Img_Awaken).GetComponent<OnClickImage>();

        WeaponImage = this.TryFindChild(Img_WeaponImage).GetComponent<Image>();
        AwakenImage = this.TryFindChild(Img_AwakenImage).GetComponent<Image>();

        CharacterImage = this.TryFindChild(Img_Character).GetComponent<Image>();

        Join_Button = this.TryFindChild(Img_Join).GetComponent<OnClickImage>();
    }

    private void Awake()
    {
        Join_Button.OnClick = OnClickJoin;
    }

    public void SetAttackValue(int value)
    {
        Attack_Text.text = value.ToString();
    }

    public void SetHPValue(int value)
    {
        HP_Text.text = value.ToString();
    }

    //여기를 보고 필요한 이미지를 넣으시고, 추가로 너무 심심한데 이것좀 넣어주세요
    public void SetStatusView(CharacterInstance _instance)
    {
        CharacterImage.sprite = _instance.characterImage;
        AwakenImage.sprite = _instance.characterImage;
        WeaponImage.sprite = _instance.characterImage;

        SetAttackValue(((int)_instance.GetCurrentAttack()));
        SetHPValue(((int)_instance.GetCurrentHealth()));
    }

    public void SetStatusView(int _index)
    {
        Selected_Index = _index;
         CharacterInstance instance = CharacterManager.Instance.GetCharacter(_index);
        SetStatusView(instance);
    }

    public void OnClickJoin()
    {
        CharacterManager.Instance.SelectParticipate(Selected_Index);
    }

}
