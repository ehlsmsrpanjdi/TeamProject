using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleMember : MonoBehaviour
{
    [SerializeField] OnClickImage ClickImage;
    [SerializeField] Image MemberImage;
    [SerializeField] TextMeshProUGUI MemberRankText;
    [SerializeField] TextMeshProUGUI MemberTypeText;

    CharacterInstance Inst;

    const string Img_Member = "Img_Member";
    const string Rank_Text = "Rank_Text";
    const string Type_Text = "Type_Text";

    UIManager Manager;

    protected void Reset()
    {
        ClickImage = GetComponent<OnClickImage>();
        MemberImage = this.TryFindChild(Img_Member)?.GetComponent<Image>();
        MemberRankText = this.TryFindChild(Rank_Text)?.GetComponent<TextMeshProUGUI>();
        MemberTypeText = this.TryFindChild(Type_Text)?.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Manager = UIManager.Instance;
        ClickImage.OnClick = OnClickButtonOn;
    }

    void OnClickButtonOn()
    {
        UISkillViewer SkillViewer = Manager.GetUI<UISkillViewer>(Manager.GetBattleCanvas());
        SkillViewer.ResetSkill();
        SkillViewer.ViewSkill(Inst);
    }

    public void OnMemberSet(CharacterInstance _instance)
    {
        Inst = _instance;
        MemberImage.sprite = _instance.characterImage;
        MemberRankText.text = RankToText.RankString(_instance.currentRank);
        MemberTypeText.text = _instance.charcterName;
    }
}

public static class RankToText
{
    public static string RankString(Rank _rank)
    {
        switch (_rank)
        {
            case Rank.C:
                return "C";
            case Rank.B:
                return "B";
            case Rank.A:
                return "A";
            case Rank.S:
                return "S";
            case Rank.SS:
                return "SS";
            case Rank.SSS:
                return "SSS";
            default:
                return "";
        }
    }

}
