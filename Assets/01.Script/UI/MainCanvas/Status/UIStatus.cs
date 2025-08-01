using TMPro;
using UnityEngine;

public class UIStatus : UIBase
{
    [SerializeField] TextMeshProUGUI GoldText;
    [SerializeField] TextMeshProUGUI DiaText;
    [SerializeField] OnClickImage OptionButton;

    const string Gold_Text = "Gold_Text";
    const string Dia_Text = "Dia_Text";

    private void Reset()
    {
        GoldText = this.TryFindChild(Gold_Text).GetComponent<TextMeshProUGUI>();
        DiaText = this.TryFindChild(Dia_Text).GetComponent<TextMeshProUGUI>();
        OptionButton = GetComponentInChildren<OnClickImage>();
    }

    private void Awake()
    {
        OptionButton.OnClick = OnOptionClick;
        Player player = Player.Instance;

        player.OnGoldChanged += SetGold;
        player.OnDiamondChanged += SetDia;

        SetGold(player.Data.gold);
        SetDia(player.Data.diamond);
    }

    public void OnOptionClick()
    {
        UIManager.Instance.OpenUI<UIOption>(UIManager.Instance.GetMainCanvas());
    }

    public void SetGold(int _Gold)
    {
        GoldText.text = _Gold.ToString();
    }

    public void SetDia(int _Dia)
    {
        DiaText.text = _Dia.ToString();
    }
}
