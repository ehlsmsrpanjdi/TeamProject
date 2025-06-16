using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimController
{
    private readonly int isMoving = Animator.StringToHash("IsMoving");
    private readonly int isSetAttack = Animator.StringToHash("IsSetAttack");
    private readonly int isAttacking = Animator.StringToHash("IsAttacking");

    public Animator animator;

    public CharAnimController(Animator animator)
    {
        this.animator = animator;
    }

    public void Moving()
    {
        animator.SetTrigger(isMoving);
    }

    public void SetAttack(bool isSet)
    {
        animator.SetBool(isSetAttack, isSet);
    }

    public void Attacking(bool isAttak)
    {
        animator.SetBool(isAttacking, isAttak);
    }


}
