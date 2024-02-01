using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordAttack : Attack
{
    [SerializeField]
    bool parryIndicator;
    [HideInInspector]
    public bool isAttacking;
    public bool IsAttacking { get { return isAttacking; } set { isAttacking = value; } }
    bool oldIsAttacking;

    private void Start()
    {
        // TODO: Use the same animator as character
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
        CharacterManager enemy = other.GetComponent<CharacterManager>();
        if (enemy == null) 
        {
            return;
        }
        float hit = enemy.Hit(damage, canBeBlocked);
        
        if (hit != SUCCESS && CorrectParry(hit))
        {
            controller.GetStunned();
        }

        if (hit == SUCCESS && playerMana != null)
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

    override public void Interrupt() 
    { 
        if (isAttacking)
        {
            // TODO: Replace with getting hit animation
            animator.Play("Idle");
        }
    }
}
