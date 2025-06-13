using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleMemberComponent : MonoBehaviour
{
    [SerializeField] Image MemberImage;
    [SerializeField] TextMeshProUGUI MemberRankText;
    [SerializeField] TextMeshProUGUI MemberTypeText;

    const string Img_Member = "Img_Member";
    const string Rank_Text = "Rank_Text";
    const string Type_Text = "Type_Text";

    protected void Reset()
    {
        MemberImage = gameObject.transform.Find(Img_Member)?.GetComponent<Image>();
        MemberRankText = gameObject.transform.Find(Rank_Text)?.GetComponent<TextMeshProUGUI>();
        MemberTypeText = gameObject.transform.Find(Type_Text)?.GetComponent<TextMeshProUGUI>();
    }
}
