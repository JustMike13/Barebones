using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    bool isPlayer;
    public bool IsPlayer {  set { isPlayer = value; } }
    LevelManager levelManager;
    public LevelManager LevelManager {  set { levelManager = value; } }
    BaseContoller controller;
    Health health;
    Animator animator;
    [SerializeField]
    List<Attack> attacks;

    void Start()
    {
        controller = GetComponent<BaseContoller>();
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        foreach (Attack attack in attacks)
        {
            attack.Controller = controller;
            attack.Animator = animator;
        }
    }

    public float Hit(float damage, bool canBeBlocked = false)
    {
        if (controller != null)
        {
            bool blocked = controller.IsBlocking();
            if (blocked && canBeBlocked)
            {
                return controller.StartBlockingTime;
            }
            if (health != null)
            {
                controller.Interrupt();
                health.TakeDamage(damage);
                return Attack.SUCCESS;
            }
        }
        return Attack.FAIL;
    }
    public void Enable()
    {
        controller.enabled = true;
    }

    public void Disable()
    {
        controller.enabled = false;
    }

    public void Death()
    {
        // TODO: Add death animation
        // animator.Play("Death");
        if (isPlayer)
        {
            levelManager.Defeat();
        }
        else
        {
            levelManager.Victory();
        }
    }
}
