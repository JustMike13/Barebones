using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public const float SUCCESS = 0;
    public const float FAIL = -1;
    
    public CharacterManager characterManager;
    protected Animator animator;
    //public Animator Animator { get { return animator; } set { animator = value; } }
    protected BaseContoller controller;
    public BaseContoller Controller { get { return controller; } set { controller = value; } } 
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected int manaCost;
    [SerializeField]
    Vector2 range;
    [SerializeField]
    protected bool canBeBlocked;
    [SerializeField]
    protected bool canBeParried;
    [SerializeField]
    protected float cooldown = 1f;
    protected float cooldownLeft;
    protected Vector2 parryWindow;
    [HideInInspector]
    public bool parryWindowOn;
    protected bool oldParryWindowOn;
    protected PlayerMana playerMana;
    bool setBusyByMe = false;

    protected void Start() 
    {
        animator = characterManager.Animator;
        playerMana = characterManager.PlayerMana;
    }
    private void Update()
    {
        UpdateCooldown();
    }

    protected void UpdateCooldown()
    {
        if (cooldownLeft > 0)
        {
            cooldownLeft -= Time.deltaTime;
        }
    }
    public virtual void UseAttack() { }
    public virtual void Interrupt() { }
    public virtual bool CanUse() { return cooldownLeft <= 0; }
    public bool InRange(float distance)
    {
        return (distance >= range.x && (distance <= range.y || range.y == 0)) ;
    }
    public virtual void ProcessCollider(Collider other) { }
    public bool CorrectParry(float blockingTime)
    {
        return canBeParried && blockingTime >= parryWindow.x && blockingTime <= parryWindow.y;
    }

    public bool SetBusy()
    {
        if (!controller.IsBusy)
        {
            controller.IsBusy = true;
            setBusyByMe = true;
            return true;
        }
        else if (setBusyByMe)
        {
            return true;
        }
        
        return false;
    }

    public void NotBusy()
    {
        if (setBusyByMe) 
        { 
            controller.IsBusy = false;
            setBusyByMe = false;
        }
    }
}
