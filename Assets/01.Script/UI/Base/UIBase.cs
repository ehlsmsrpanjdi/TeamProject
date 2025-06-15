using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    public virtual void Open()
    {
        if(true == gameObject.activeSelf)
        {
            transform.KillDoTween();
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        if (false == gameObject.activeSelf)
        {
            transform.KillDoTween();
            gameObject.SetActive(true);
            return;
        }
        gameObject.SetActive(false);
    }
}
