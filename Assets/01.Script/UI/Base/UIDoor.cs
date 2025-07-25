using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIDoor : UIBase
{
    [SerializeField] Image LeftDoorImg;
    [SerializeField] Image RightDoorImg;

    const string Left_Door = "Left_Door";
    const string Right_Door = "Right_Door";

    private void Reset()
    {
        LeftDoorImg = this.TryFindChild(Left_Door).GetComponent<Image>();
        RightDoorImg = this.TryFindChild(Right_Door).GetComponent<Image>();
    }

    float LeftPos = -960;
    float RightPos = 960;

    private void Start()
    {
    }

    public Action OnCloseAction;
    public Action OnOpenAction;

    public override void Open()
    {
        base.Open();
        transform.SetAsLastSibling();
        LeftDoorImg.gameObject.transform.MoveX(LeftPos, 0);
        Tween tween = RightDoorImg.gameObject.transform.MoveX(RightPos, 0);
        tween.OnComplete(() =>
        {
            SoundManager.Instance.PlaySFX(SfxType.UI, 3);
            OnCloseAction?.Invoke();
            OnCloseAction = null;
            StartCoroutine(CloseCoroutine());
        });
    }

    IEnumerator CloseCoroutine()
    {
        yield return CoroutineHelper.GetTime(2f);
        Close();
    }

    public override void Close()
    {
        OnOpenAction?.Invoke();
        OnOpenAction = null;
        LeftDoorImg.gameObject.transform.MoveX(0, LeftPos);
        Tween tween = RightDoorImg.gameObject.transform.MoveX(0, RightPos);
        tween.OnComplete(() =>
        {

            base.Close();
        });
    }
}
