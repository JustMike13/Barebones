using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordAttack : Attack
{
    const int IDLE = 0;
    const int PARRY = 1;
    const int BLOCK = 2;
    Animator animator;
    BaseContoller controller;
    [SerializeField]
    Vector2 parryTimeWindow = new Vector2(0.2f, 1f);
    float timeSinceBlocking;


    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = transform.parent.GetComponent<BaseContoller>();
        timeSinceBlocking = 0;
    }

    private void Update()
    {
        UpdateCooldown();
        if (animator != null)
        {
            if (controller.IsBlocking())
            {
                animator.SetBool("IsBlocking", true);
                timeSinceBlocking += Time.deltaTime;
            }
            else
            {
                animator.SetBool("IsBlocking", false);
                timeSinceBlocking = 0;
            }
        }
    }

    override public void ProcessCollider(Collider other)
    {
        Health enemyHealth = other.GetComponent<Health>();
        if (enemyHealth == null) 
        {
            return;
        }
        SwordAttack enemyAttack = other.GetComponentInChildren<SwordAttack>();
        if (enemyAttack != null)
        {
            int blocked = enemyAttack.Block();
            if (blocked == PARRY)
            {
                controller.GetStunned();
            }
            if (blocked != IDLE)
            {
                return;
            }
        }
        enemyHealth.TakeDamage(damage);
    }

    public int Block()
    {
        if (timeSinceBlocking > parryTimeWindow[0] && timeSinceBlocking < parryTimeWindow[1])
        {
            return PARRY;
        }
        if (timeSinceBlocking > 0)
        {
            return BLOCK;
        }
        return IDLE;
    }

    override public void UseAttack()
    {
        if (!controller.IsAttacking)
        {
            animator.Play("Attack");
            cooldownLeft = cooldown;
        }
    }
}
