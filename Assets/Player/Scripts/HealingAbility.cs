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
    [SerializeField] AudioSource healingSound;
    [SerializeField] AudioSrc healingChargeSound;
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
    bool alreadyHealing = false;
    new void Start()
    {
        base.Start();
        playerHealth = GetComponent<Health>();
        healingAura.SetActive(false);
    }

    private void Update()
    {
        float step = duration / steps;
        if (isHealing && Time.time - lastHeal >= healDelay 
            && (playerMana.IsAvailable(manaCost) || alreadyHealing) && !playerHealth.IsFull())
        {
            alreadyHealing=true;
            animator.SetBool("IsWalking", false);
            if (SetBusy())
            {
                healingAura.SetActive(true);
                if (stepsCompleted < steps) 
                {
                    if (Time.time - lastUpdateTime >= step)
                    {
                        healingChargeSound.Play();
                        playerMana.ConsumeMana(manaCost / steps);
                        lastUpdateTime = Time.time;
                        stepsCompleted += 1;
                    }
                }
                if (stepsCompleted == steps)
                {
                    healingChargeSound.Stop();
                    lastHeal = Time.time;
                    healingSound.Play();
                    playerHealth.Refill(damage);
                }
            }
        }
        else
        {
            healingChargeSound.Stop();
            alreadyHealing = false;
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
