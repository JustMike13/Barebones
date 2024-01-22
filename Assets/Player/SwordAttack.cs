using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    const int IDLE = 0;
    const int PARRY = 1;
    const int BLOCK = 2;
    [SerializeField]
    int swordDamage = 10;
    Animator animator;
    BaseContoller controller;
    [SerializeField]
    Vector2 parryTimeWindow = new Vector2(0.2f, 1f);
    float timeSinceBlocking;


    private void Start()
    {
        animator = transform.parent.GetComponent<Animator>();
        controller = transform.parent.parent.GetComponent<BaseContoller>();
        timeSinceBlocking = 0;
    }

    private void Update()
    {
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
}
