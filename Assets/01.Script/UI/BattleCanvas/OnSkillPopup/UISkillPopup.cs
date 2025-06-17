using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UISkillPopup : UIBase
{
    [SerializeField] Image CharacterImage;

    const string Img_Character = "Img_Character";

    private void Reset()
    {
        CharacterImage = this.TryFindChild(Img_Character).GetComponent<Image>();
    }

    public override void Open()
    {
        if(null != CoroutineValue)
        {
            StopCoroutine(CoroutineValue);
        }
        base.Open();
        Tween tween = transform.FadeOutXY();
        tween.OnComplete(() =>
        {
            CoroutineValue = StartCoroutine(CloseCoroutine());
        });
    }

    Coroutine CoroutineValue;

    public void SetSprite(Sprite _sprite)
    {
        CharacterImage.sprite = _sprite;
    }

    IEnumerator CloseCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        Close();
    }

    public override void Close()
    {
        Tween tween = transform.FadeInXY();
        tween.OnComplete(() =>
        {
            base.Close();
        });
    }
}
