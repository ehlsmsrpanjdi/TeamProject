using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HireLog : MonoBehaviour
{
    [SerializeField] Image HireImage;

    const string Img_Hire = "Img_Hire";

    private void Reset()
    {
        HireImage = transform.Find(Img_Hire).GetComponent<Image>();
    }

    public void SetHireImage(Sprite sprite)
    {
        if (HireImage != null)
        {
            HireImage.sprite = sprite;
        }
        else
        {
            DebugHelper.LogError("HireImage is not assigned.", this);
        }
    }
}
