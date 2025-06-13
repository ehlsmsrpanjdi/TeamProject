using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkill : MonoBehaviour
{
    [SerializeField] Image SkillImage;
    const string Img_Skill = "Img_Skill";

    private void Reset()
    {
        SkillImage = gameObject.transform.Find(Img_Skill).GetComponent<Image>();
    }
}
