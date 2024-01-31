using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseContoller : MonoBehaviour
{
    [NonSerialized]
    public bool isStunned;
    public bool IsStunned { get { return isStunned; } }
    protected bool isBlocking;
    protected Animator animator;
    bool isAttacking;
    public bool IsAttacking { get { return isAttacking; } set { isAttacking = value; } }
    float startBlockingTime;
    public float StartBlockingTime { get { return startBlockingTime; } set { startBlockingTime = value; } }
    bool isBusy;
    public bool IsBusy { get { return isBusy; } set { isBusy = value; } }

    protected void GetAnimator()
    {
        animator = transform.GetComponent<Animator>();
    }

    public bool IsBlocking()
    { 
        if (!isStunned)
        {
            return isBlocking;
        }
        return false;
    }

    public void GetStunned()
    {
        if (animator != null)
        {
            animator.Play("Stunned");
        }
    }

    public virtual void LookAtPlayer() { }

    public virtual void Death()
    {
        Destroy(this);
    }
}
