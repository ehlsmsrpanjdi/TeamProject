using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatus : MonoBehaviour
{
    const string Hp_Value_Text = "Hp_Value_Text";
    const string Attack_Value_Text = "Attack_Value_Text";
    const string Lv_Value_Text = "Lv_Value_Text";
    const string Status_Text = "Status_Text";


    const string Img_Weapon = "Img_Weapon";
    const string Img_Awaken = "Img_Awaken";

    const string Img_WeaponImage = "Img_WeaponImage";
    const string Img_AwakenImage = "Img_AwakenImage";

    const string Img_Character = "Img_Character";

    const string Img_Join = "Img_Join";

    const string Join_Text = "Join_Text";

    [SerializeField] TextMeshProUGUI Attack_Text;
    [SerializeField] TextMeshProUGUI HP_Text;
    [SerializeField] TextMeshProUGUI CharacterLv;
    [SerializeField] TextMeshProUGUI StatusText;

    [SerializeField] TextMeshProUGUI JoinText;

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
        CharacterLv = this.TryFindChild(Lv_Value_Text).GetComponent<TextMeshProUGUI>();
        StatusText = this.TryFindChild(Status_Text).GetComponent<TextMeshProUGUI>();

        WeaponUpgrade_Button = this.TryFindChild(Img_Weapon).GetComponent<OnClickImage>();
        AwakenUpgrade_Button = this.TryFindChild(Img_Awaken).GetComponent<OnClickImage>();

        WeaponImage = this.TryFindChild(Img_WeaponImage).GetComponent<Image>();
        AwakenImage = this.TryFindChild(Img_AwakenImage).GetComponent<Image>();

        CharacterImage = this.TryFindChild(Img_Character).GetComponent<Image>();

        Join_Button = this.TryFindChild(Img_Join).GetComponent<OnClickImage>();
        JoinText = this.TryFindChild(Join_Text).GetComponent<TextMeshProUGUI>();

    }

    private void Start()
    {
        Join_Button.OnClick = OnClickJoin;
        WeaponUpgrade_Button.OnClick = OnClickLvUpgrade;
        AwakenUpgrade_Button.OnClick = OnClickRankUp;
    }

    public void SetAttackValue(int value)
    {
        Attack_Text.text = value.ToString();
    }

    public void SetHPValue(int value)
    {
        HP_Text.text = value.ToString();
    }

    public void SetLvValue(int Value)
    {
        CharacterLv.text = Value.ToString();
    }

    public void SetStatus(string _Status)
    {
        StatusText.text = _Status;
    }

    public void IsJoin(bool _Value)
    {
        if (true == _Value)
        {
            JoinText.SetText("Pop");
        }
        else
        {
            JoinText.SetText("Join");
        }
    }

    //여기를 보고 필요한 이미지를 넣으시고, 추가로 너무 심심한데 이것좀 넣어주세요
    public void SetStatusView(CharacterInstance _instance)
    {
        CharacterImage.sprite = _instance.characterImage;
        AwakenImage.sprite = _instance.characterImage;
        WeaponImage.sprite = _instance.characterImage;

        StatusReset();
    }

    public void NoneView()
    {
        CharacterImage.sprite = null;
        AwakenImage.sprite = null;
        WeaponImage.sprite = null;

        SetAttackValue(0);
        SetHPValue(0);
    }

    public void SetStatusView(int _index)
    {
        Selected_Index = _index;
        CharacterInstance instance = CharacterManager.Instance.GetCharacter(_index);
        if (instance != null)
        {
            SetStatusView(instance);
        }
    }

    public void OnClickLvUpgrade()
    {
        CharacterInstance instance = CharacterManager.Instance.GetCharacter(Selected_Index);
        instance.Enhance();
        StatusReset();
    }

    public void OnClickRankUp()
    {
        CharacterInstance instance = CharacterManager.Instance.GetCharacter(Selected_Index);
        UIManager Manager = UIManager.Instance;
        if (true == instance.RankUp())
        {
            UIManagement Management = Manager.GetUI<UIManagement>(Manager.GetMainCanvas());
            Management.InventoryReset(Selected_Index);
            StatusReset();
            SetStatusView(Selected_Index);
        }
        else
        {
            UIPopup Popup = UIManager.Instance.GetUI<UIPopup>(Manager.GetMainCanvas());
            Popup.Open();
            Popup.SetText("같은 등급의 동일한 캐릭터가 없습니다");
        }
    }

    public void StatusReset()
    {
        CharacterInstance instance = CharacterManager.Instance.GetCharacter(Selected_Index);
        SetStatus(instance.charcterName);
        SetLvValue(instance.GetEnhancementLevel());
        SetAttackValue((int)instance.GetCurrentAttack());
        SetHPValue((int)instance.GetCurrentHealth());
    }

    public void OnClickJoin()
    {
        bool IsParticipated = CharacterManager.Instance.IsParticipating(Selected_Index);
        if (true == IsParticipated)
        {
            IsJoin(!IsParticipated);
            CharacterManager.Instance.RemoveParticipate(Selected_Index);
            UIManager.Instance.GetUI<UIManagement>(UIManager.Instance.GetMainCanvas()).OffClickJoin(Selected_Index);
        }
        else
        {
            CharacterManager.Instance.SelectParticipate(Selected_Index);
            UIManager.Instance.GetUI<UIManagement>(UIManager.Instance.GetMainCanvas()).OnClickJoin(Selected_Index);
            IsJoin(!IsParticipated);
        }
    }


}
