using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordAttack : Attack
{
    Animator animator;
    BaseContoller controller;
    [SerializeField]
    bool parryIndicator;
    public bool isAttacking;
    public bool IsAttacking { get { return isAttacking; } set { isAttacking = value; } }
    bool oldIsAttacking;
    PlayerMana playerMana;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = transform.parent.GetComponent<BaseContoller>();
        isAttacking = false;
        playerMana = transform.parent.GetComponent<PlayerMana>();
    }

    private void Update()
    {
        UpdateCooldown();
        ProcessParry();
        if (animator != null)
        {
            if (controller.IsBlocking())
            {
                animator.SetBool("IsBlocking", true);
            }
            else
            {
                animator.SetBool("IsBlocking", false);
            }
        }
        if (oldIsAttacking && !isAttacking) 
        {
            controller.IsAttacking = false;
        }
        oldIsAttacking = isAttacking;
    }

    private void ProcessParry()
    {
        if (parryWindowOn && !oldParryWindowOn)
        {
            parryWindow.x = Time.time;
            parryWindow.y = Mathf.Infinity;
            parryWindowOn = true;
        }
        else if (!parryWindowOn && oldParryWindowOn)
        {
            parryWindow.y = Time.time;
            parryWindowOn = false;
        }
        oldParryWindowOn = parryWindowOn;
    }

    override public void ProcessCollider(Collider other)
    {
        Health enemyHealth = other.GetComponent<Health>();
        if (enemyHealth == null) 
        {
            return;
        }
        BaseContoller enemyController = other.GetComponentInChildren<BaseContoller>();
        if (enemyController != null)
        {
            bool blocked = enemyController.IsBlocking();
            if (blocked && canBeBlocked)
            {
                if (CorrectParry(enemyController.StartBlockingTime))
                {
                    controller.GetStunned();
                }
                return;
            }
        }
        enemyHealth.TakeDamage(damage);
        if (playerMana != null)
        {
            playerMana.AddHitBonus();
        }
    }


    override public void UseAttack()
    {
        if (!controller.IsAttacking)
        {
            animator.Play("Attack");
            cooldownLeft = cooldown;
            isAttacking = true;
            controller.IsAttacking = true;
        }
    }
}
