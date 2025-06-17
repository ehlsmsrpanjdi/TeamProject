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
            OnCloseAction?.Invoke();
            StartCoroutine(CloseCoroutine());
        });
    }

    IEnumerator CloseCoroutine()
    {
        yield return new WaitForSeconds(2f);
        Close();
    }

    public override void Close()
    {
        OnOpenAction?.Invoke();
        LeftDoorImg.gameObject.transform.MoveX(0, LeftPos);
        Tween tween = RightDoorImg.gameObject.transform.MoveX(0, RightPos);
        tween.OnComplete(() =>
        {
            OnCloseAction = null;
            OnOpenAction = null;
            base.Close();
        });
    }
}
