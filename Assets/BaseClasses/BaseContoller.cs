using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseContoller : MonoBehaviour
{
    [HideInInspector]
    public bool isStunned;
    public bool IsStunned { get { return isStunned; } }
    protected bool isBlocking;
    protected Animator animator;
    public Animator Animator {  get { return animator; } set { animator = value; } }
    bool isAttacking;
    public bool IsAttacking { get { return isAttacking; } set { isAttacking = value; } }
    float startBlockingTime;
    public float StartBlockingTime { get { return startBlockingTime; } set { startBlockingTime = value; } }
    [SerializeField]
    bool isBusy;
    public bool IsBusy { 
        get { return isBusy; } 
        set { isBusy = value; } }

    protected bool isInvulnerable = false;
    public bool IsInvulnerable { get {  return isInvulnerable; } set {  IsInvulnerable = value; } }
    
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

    public virtual void Interrupt() { }
}
