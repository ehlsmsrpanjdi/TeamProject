using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkill : MonoBehaviour
{
    [SerializeField] OnClickImage SkillBtn;
    [SerializeField] Image SkillImage;
    const string Img_Skill = "Img_Skill";
    int index;

    private void Reset()
    {
        SkillImage = this.TryFindChild(Img_Skill).GetComponent<Image>();
        SkillBtn = GetComponent<OnClickImage>();
    }

    public void Start()
    {
        SkillBtn.OnClick = OnClickButton;
    }

    public void SetIndex(int _index)
    {
        index = _index;
    }

    public void OnClickButton()
    {
        UIManager Manager = UIManager.Instance;
        UISkillViewer Viewer = Manager.GetUI<UISkillViewer>(Manager.GetBattleCanvas());
        CharacterInstance Inst = Viewer.GetCharacterInst();

    }

    public void SetImage(Sprite _sprite)
    {
        SkillImage.sprite = _sprite;
    }
}
