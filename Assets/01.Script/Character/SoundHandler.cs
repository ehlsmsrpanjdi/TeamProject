using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public void AttackSound()
    {
        SoundManager.Instance.PlaySFX(SfxType.Attack, -1);
    }

    public void RunningSound()
    {
        SoundManager.Instance.PlaySFX(SfxType.Running, -1);
    }
}
