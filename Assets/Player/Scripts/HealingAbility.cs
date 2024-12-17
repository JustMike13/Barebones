using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class HealingAbility : Attack
{
    [SerializeField]
    GameObject healingAura;
    [SerializeField]
    float duration;
    bool isHealing = false;
    public bool IsHealing { get { return isHealing; } set { isHealing = value; } }
    float lastUpdateTime;
    float lastHeal;
    [SerializeField]
    float healDelay = 0.3f;
    [SerializeField]
    int steps = 10;
    Health playerHealth;
    int stepsCompleted = 0;

    new void Start()
    {
        playerMana = GetComponent<PlayerMana>();
        playerHealth = GetComponent<Health>();
        healingAura.SetActive(false);
        animator = characterManager.Animator;
    }

    private void Update()
    {
        float step = duration / steps;
        if (isHealing)
        if (isHealing && Time.time - lastHeal >= healDelay 
            && playerMana.IsAvailable(manaCost / steps) && !playerHealth.IsFull())
        {
            animator.SetBool("IsWalking", false);
            if (SetBusy())
            {
                healingAura.SetActive(true);
                if (stepsCompleted < steps) 
                {
                    if (Time.time - lastUpdateTime >= step)
                    {
                        playerMana.ConsumeMana(manaCost / steps);
                        lastUpdateTime = Time.time;
                        stepsCompleted += 1;
                    }
                }
                if (stepsCompleted == steps) 
                {
                    lastHeal = Time.time;
                    playerHealth.Refill(damage);
                }
            }
        }
        else
        {
            NotBusy();
            healingAura.SetActive(false);
            lastUpdateTime = Time.time;
            stepsCompleted = 0;
        }
    }

    override public void Interrupt()
    {
        if (isHealing)
        {
            lastHeal = Time.time;
            NotBusy();
            healingAura.SetActive(false);
            lastUpdateTime = Time.time;
            stepsCompleted = 0;
        }
    }
}
