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

        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        transform.KillDoTween();
        gameObject.SetActive(false);
    }
}
