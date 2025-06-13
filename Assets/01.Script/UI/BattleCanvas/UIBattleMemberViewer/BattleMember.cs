using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleMember : MonoBehaviour
{
    [SerializeField] Image MemberImage;
    [SerializeField] TextMeshProUGUI MemberRankText;
    [SerializeField] TextMeshProUGUI MemberTypeText;

    const string Img_Member = "Img_Member";
    const string Rank_Text = "Rank_Text";
    const string Type_Text = "Type_Text";

    protected void Reset()
    {
        MemberImage = this.TryFindChild(Img_Member)?.GetComponent<Image>();
        MemberRankText = this.TryFindChild(Rank_Text)?.GetComponent<TextMeshProUGUI>();
        MemberTypeText = this.TryFindChild(Type_Text)?.GetComponent<TextMeshProUGUI>();
    }
}

