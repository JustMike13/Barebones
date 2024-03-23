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
    [SerializeField]
    Animator animator;
    public Animator Animator { get { return animator; } }
    PlayerMana playerMana;
    public PlayerMana PlayerMana { get {  return playerMana ; } }
    [SerializeField]
    List<Attack> attacks;
    float blockAngle = 80.0f;

    void Start()
    {
        controller = GetComponent<BaseContoller>();
        health = GetComponent<Health>();
        playerMana = GetComponent<PlayerMana>();
        // TODO: Remove
        //foreach (Attack attack in attacks)
        //{
        //    attack.Controller = controller;
        //    attack.Animator = animator;
        //}
    }

    public float Hit(CharacterManager Opponent, float Damage, bool CanBeBlocked = false)
    {
        if (controller != null && !controller.IsInvulnerable)
        {
            bool blocked = controller.IsBlocking();
            if (blocked && CanBeBlocked && IsFacing(Opponent))
            {
                return controller.StartBlockingTime;
            }
            if (health != null)
            {
                controller.Interrupt();
                health.TakeDamage(Damage);
                return Attack.SUCCESS;
            }
        }
        return Attack.FAIL;
    }

    private bool IsFacing(CharacterManager Opponent)
    {
        return (Vector3.Angle(transform.forward, Opponent.transform.position - transform.position) < blockAngle);
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
