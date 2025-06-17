using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundHelper : MonoBehaviour
{
    GameObject Child;

    private void Awake()
    {
        Child = gameObject.transform.GetChild(0).gameObject;
    }

}
