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

    float currentTime;
    float cooldownTime;
    bool isCooldown;

    private void Update()
    {
        if (isCooldown)
        {
            currentTime -= Time.deltaTime;
            SkillBtn.fillAmount = (cooldownTime - currentTime) / cooldownTime;

            if (currentTime <= 0f)
            {
                isCooldown = false;
                SkillBtn.fillAmount = 1f;  // 쿨타임 끝
            }
        }
    }

    public void SetCoolTime(float _CurrentTime, float _MaxTime)
    {
        isCooldown = true;
        currentTime = _CurrentTime;
        cooldownTime = _MaxTime;
        SkillBtn.fillAmount = (cooldownTime - currentTime) / cooldownTime;
    }
    public void OnClickButton()
    {
        UIManager Manager = UIManager.Instance;
        UISkillViewer Viewer = Manager.GetUI<UISkillViewer>(Manager.GetBattleCanvas());
        CharacterInstance Inst = Viewer.GetCharacterInst();
        if (true == Inst.ExcuteSkill(index))
        {
            Skill skill = Inst.GetActiveSkills()[index];
            SetCoolTime(skill.currentCooldown, skill.skillCooldown);
        }
    }

    public void SetImage(Sprite _sprite)
    {
        SkillImage.sprite = _sprite;
    }
}
