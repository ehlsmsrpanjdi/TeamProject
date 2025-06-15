using DG.Tweening;
using UnityEngine;

public static class DoTweenHelper
{
    public static void RotationLoop(this Transform transform)
    {
        transform
        .DORotate(Vector3.forward * 360, 2f, RotateMode.FastBeyond360)
        .SetEase(Ease.Linear)
        .SetRelative()
        .SetLoops(-1, LoopType.Restart);
    }

    public static Tween FadeInX(this Transform transform)
    {
        return transform.DOScaleX(0, 0.5f).From(new Vector3(1,1,1)).SetEase(Ease.InBack);
    }

    public static Tween FadeOutX(this Transform transform)
    {
        return transform.DOScaleX(1, 0.5f).From(new Vector3(0, 1, 1)).SetEase(Ease.OutBack);
    }

    public static Tween FadeInXY(this Transform transform)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOScaleX(0.1f, 0.5f).From(new Vector3(1,1,1)).SetEase(Ease.InBack));
        seq.Append(transform.DOScaleY(0, 0.5f).From(new Vector3(0.1f, 1f, 1f)).SetEase(Ease.InBack));

        return seq;
    }

    public static Tween FadeOutXY(this Transform transform)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOScaleX(1f, 0.5f).From(new Vector3(0, 0.1f, 1)).SetEase(Ease.InBack));
        seq.Append(transform.DOScaleY(1f, 0.5f).From(new Vector3(1f, 0.1f, 1f)).SetEase(Ease.InBack));

        return seq;
    }

    public static void KillDoTween(this Transform transform)
    {
        DG.Tweening.DOTween.Kill(transform);
    }

}
