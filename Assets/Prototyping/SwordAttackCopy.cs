using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttackCopy : MonoBehaviour
{
    int IDLE = 0;
    int PARRY = 1;
    int BLOCK = 2;
    [SerializeField]
    int swordDamage = 10;
    Animator animator;
    ThirdPersonControllerCopy controller;
    bool isBlocking;
    [SerializeField]
    Vector2 parryTimeWindow = new Vector2(0.2f, 1f);
    float timeSinceBlocking;


    private void Start()
    {
        animator = transform.parent.GetComponent<Animator>();
        controller = transform.parent.parent.GetComponent<ThirdPersonControllerCopy>();

        isBlocking = true;
        timeSinceBlocking = 0;
        animator.SetBool("IsBlocking", true);
        timeSinceBlocking = 5;
    }

    private void Update()
    {
        //isBlocking = true;
        //if (animator != null)
        //{
        //    if (Input.GetKey(KeyCode.Mouse1) && !controller.IsStunned )
        //    {
        //        animator.SetBool("IsBlocking", true);
        //        isBlocking = true;
        //        timeSinceBlocking += Time.deltaTime;
        //    }
        //    else
        //    {
        //        animator.SetBool("IsBlocking", false);
        //        isBlocking = true;
        //        timeSinceBlocking = 0;
        //    }
        //}
    }

    private void OnTriggerEnter(Collider other)
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
        enemyHealth.TakeDamage(swordDamage);
    }

    public int Block()
    {
        if (timeSinceBlocking > parryTimeWindow.x && timeSinceBlocking < parryTimeWindow.y)
        {
            return PARRY;
        }
        if (timeSinceBlocking > 0)
        {
            return BLOCK;
        }
        return IDLE;
    }
}
