using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    // Inspector variables
    [Header("Object References")]
    [SerializeField]
    List<Attack> attacks;
    [SerializeField]
    GameObject mesh = null;
    [SerializeField] bool HitsStopAttacks = false;
    [SerializeField] AudioSource shieldBlockSound;
    public List<Attack> Attacks { get { return attacks; } }
    [SerializeField]
    GameObject parryIndicator;

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
        if (parryIndicator != null)
        {
            parryIndicator.SetActive(false);
        }
    }

    public float Hit(CharacterManager Opponent, float Damage, bool CanBeBlocked = false, Vector3 position = default(Vector3))
    {
        if (controller != null && !controller.IsInvulnerable)
        {
            // TODO: Send CanBeBlocked to animator, if false play the hit animation, not blocking animation
            if (!controller.IsAttacking || HitsStopAttacks)
            {
                animator.SetTrigger("Hit");
                CameraShake.Shake(0.2f, 0.3f);
            }
            bool blocked = controller.IsBlocking();
            bool isFacingAttack = position == default(Vector3) ? IsFacing(Opponent.gameObject) : IsFacing(position);
            if (blocked && CanBeBlocked && isFacingAttack)
            {
                if (shieldBlockSound != null)
                {
                    shieldBlockSound.Play();
                }
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

    private bool IsFacing(GameObject Opponent)
    {
        return IsFacing(Opponent.transform.position);
    }

    private bool IsFacing(Vector3 Position)
    {
        return (Vector3.Angle(mesh.transform.forward, Position - mesh.transform.position) < blockAngle);
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
        CameraShake.Shake(0.2f, 0.1f);
    }

    public void CameraShakeOnHit()
    {
        // TODO: Add SerializeField variables for this values;
        CameraShake.Shake(0.3f, 0.1f);
    }
    public void MoveCharacter(Vector3 movement)
    {
        controller.MoveCharacter(movement);
    }

    internal void ParryIndicatorOn(bool parryWindowOn)
    {
        if ( parryIndicator != null )
        {
            parryIndicator.SetActive(parryWindowOn);
        }
    }
}
