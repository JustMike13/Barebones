using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordAttack : Attack
{
    public bool isAttacking;
    public bool IsAttacking { get { return isAttacking; } set { isAttacking = value; } }
    protected bool oldIsAttacking;

    private new void Start()
    {
        base.Start();
        isAttacking = false;
    }

    private void Update()
    {
        UpdateCooldown();
        ProcessParry();
        // TODO: Move blocking to controller
        if (animator != null)
        {
            //if (controller.IsBlocking())
            //{
            //    animator.SetBool("IsBlocking", true);
            //}
            //else
            //{
            //    animator.SetBool("IsBlocking", false);
            //}
        }
        if (oldIsAttacking && !isAttacking) 
        {
            NotBusy();
        }
        oldIsAttacking = isAttacking;
    }

    protected void OnTriggerEnter(Collider other)
    {
        CharacterManager enemy = other.GetComponent<CharacterManager>();
        if (enemy == null || enemy == characterManager)
        {
            return;
        }

        float hit = enemy.Hit(characterManager, damage, canBeBlocked);
        
        if (hit != SUCCESS && CorrectParry(hit))
        {
            controller.GetStunned();
        }

        if (hit == SUCCESS)
        {
            characterManager.AddHitBonus();
            characterManager.CameraShakeOnAttack();
        }
    }


    override public void UseAttack()
    {
        if (SetBusy(true))
        {
            animator.Play("Attack");
            cooldownLeft = cooldown;
            isAttacking = true;
        }
    }

    override public void Interrupt() 
    {
        NotBusy();
    }
}
