using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseContoller : MonoBehaviour
{
    public bool isStunned;
    public bool IsStunned { get { return isStunned; } }
    protected bool isBlocking;
    protected Animator animator;

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
}
