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
    //public Animator Animator { set { animator = value; } }
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
    protected bool setBusyByThis = false;
    protected List<Collider> collisions;

    protected void Start() 
    {
        if (characterManager == null)
        {
            Debug.LogError("Missing Character Manager");
        }
        animator = characterManager.Animator;
        playerMana = characterManager.PlayerMana;
        collisions = new List<Collider>();
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

    public bool SetBusy(bool isAttacking = false)
    {
        if (!controller.IsBusy)
        {
            controller.IsBusy = true;
            controller.IsAttacking = isAttacking;
            setBusyByThis = true;
            return true;
        }
        else if (setBusyByThis)
        {
            return true;
        }
        
        return false;
    }

    public void NotBusy()
    {
        if (setBusyByThis) 
        { 
            controller.IsBusy = false;
            controller.IsAttacking = false;
            setBusyByThis = false;
            controller.ResetTimeSinceAttack();
            ClearCollisionList();
        }
    }

    public virtual void MovementWhileAttacking()
    {
    }
    protected void ProcessParry()
    {
        if (parryWindowOn && !oldParryWindowOn)
        {
            parryWindow.x = Time.time;
            parryWindow.y = Mathf.Infinity;
            parryWindowOn = true;
            characterManager.ParryIndicatorOn(parryWindowOn);
        }
        else if (!parryWindowOn && oldParryWindowOn)
        {
            parryWindow.y = Time.time;
            parryWindowOn = false;
            characterManager.ParryIndicatorOn(parryWindowOn); 
        }
        oldParryWindowOn = parryWindowOn;
    }

    protected void ClearCollisionList()
    {
        if (collisions != null)
        {
            collisions.Clear();
        }
    }
}
