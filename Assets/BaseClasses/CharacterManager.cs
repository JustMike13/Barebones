using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    // Inspector variables
    [Header("Object References")]
    [SerializeField]
    CameraShake cameraShake;
    [SerializeField]
    List<Attack> attacks;
    [SerializeField] bool HitsStopAttacks = false;
    public List<Attack> Attacks { get { return attacks; } }

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
    float blockAngle = 80.0f;

    void Start()
    {
        controller = GetComponent<BaseContoller>();
        health = GetComponent<Health>();
        playerMana = GetComponent<PlayerMana>();
        foreach (var attack in attacks)
        {
            attack.Controller = controller;
        }
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
            if (!controller.IsAttacking || HitsStopAttacks)
            {
                animator.SetTrigger("Hit");
            }
            bool blocked = controller.IsBlocking();
            if (blocked && CanBeBlocked && IsFacing(Opponent))
            {
                return controller.StartBlockingTime;
            }
            if (health != null)
            {
                controller.Interrupt();
                health.TakeDamage(Damage);
                CameraShakeOnHit();
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

    public void AddHitBonus()
    {
        if (playerMana != null)
        {
            playerMana.AddHitBonus();
        }
    }

    public void CameraShakeOnAttack()
    {
        // TODO: Add SerializeField variables for this values;
        if (cameraShake != null)
        {
            cameraShake.ShakeCamera(0.5f, 0.1f);
        }
    }

    public void CameraShakeOnHit()
    {
        // TODO: Add SerializeField variables for this values;
        if (cameraShake != null)
        {
            cameraShake.ShakeCamera(2f, 0.1f);
        }
    }
    public void MoveCharacter(Vector3 movement)
    {
        controller.MoveCharacter(movement);
    }
}
