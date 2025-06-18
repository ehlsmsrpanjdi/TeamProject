using UnityEngine;

public class UIBase : MonoBehaviour
{
    public virtual void Open()
    {
        if (true == gameObject.activeSelf)
        {
            //transform.KillDoTween();
            //gameObject.SetActive(false);
            return;
        }
        SoundManager.Instance.PlaySFX(SfxType.UI, 0);
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        transform.KillDoTween();
        SoundManager.Instance.PlaySFX(SfxType.UI, 1);
        gameObject.SetActive(false);
    }
}
