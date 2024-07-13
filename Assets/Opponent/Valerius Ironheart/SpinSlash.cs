using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinSlash : SwordAttack
{
    public int spinStage = 0;
    // Update is called once per frame
    void Update()
    {
        UpdateCooldown();
        ProcessParry();
        if (oldIsAttacking && !isAttacking)
        {
            NotBusy();
        }
        oldIsAttacking = isAttacking;
        MovementWhileAttacking();
    }

    override public void UseAttack()
    {
        if (SetBusy(true))
        {
            animator.Play("SpinSlash");
            cooldownLeft = cooldown;
            isAttacking = true;
        }
    }


    override public void MovementWhileAttacking()
    {
        float distance = 1.3f;
        float time = 0.8334f;
        if (spinStage == 1)
            characterManager.MoveCharacter(Vector3.forward * distance/time * Time.deltaTime);
    }
}
