using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieParticle : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 3f);       
    }
}
