using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class OpponentController : BaseContoller
{
    [SerializeField]
    Transform player;

    public float meleeRange = 5f;
    public float speed = 10f;
    [SerializeField]
    Vector2 attackDelayRange;
    float attackDelay = 1f;
    float timeSinceAttack;
    [SerializeField]
    List<Attack> attacks;
    [SerializeField]
    GameObject parryIndicator;
    [SerializeField]
    float rotationSpeed = 1f;
    bool isFacingPlayer = false;
    [SerializeField]
    float facingAngle = 10f;
    [SerializeField]
    OverlayMenu menu;

    void Awake()
    {
        GetAnimator();
        timeSinceAttack = 0;
        foreach (var attack in attacks)
        {
            attack.Controller = this;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!menu.IsGameON())
        {
            return;
        }

        timeSinceAttack += Time.deltaTime;
        if (!isStunned && !IsAttacking)
        {
            LookAtPlayer();
            if (timeSinceAttack > attackDelay)
            {
                UpdateAttackDelay();
                Attack attack = ChooseAttack();
                if (attack != null)
                {
                    attack.UseAttack();
                }
                timeSinceAttack = 0;
            }
            else
            {
                FollowPlayer();
            }
        }
        ProcessParryIndicator();
        GetComponent<CharacterController>().enabled = !IsAttacking;
    }

    private void ProcessParryIndicator()
    {
        parryIndicator.SetActive(attacks.Any(a => a.parryWindowOn));
    }

    override public void LookAtPlayer()
    {
        Vector3 playerPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        if (player == null)
        {
            return;
        }
        Vector3 relativePos = playerPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativePos, Vector3.up);
        Quaternion rotationDifference = Quaternion.Inverse(targetRotation) * transform.rotation;
        float angle = rotationDifference.eulerAngles.y;
        while (angle < 0 )
        {
            angle += 360;
        }
        while (angle > 360)
        {
            angle -= 360;
        }
        isFacingPlayer = angle < facingAngle || angle > 360 - facingAngle;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void UpdateAttackDelay()
    {
        attackDelay = UnityEngine.Random.Range(attackDelayRange.x, attackDelayRange.y);
    }

    private Attack ChooseAttack()
    {
        IEnumerable<Attack> localAttacks = attacks.Where(a => a.InRange(Vector3.Distance(transform.position, player.position)) && a.CanUse());
        if (localAttacks.Count() == 0)
        {
            return null;
        }
        int position = UnityEngine.Random.Range(0, localAttacks.Count());
        return localAttacks.ElementAt(position);
    }

    void FollowPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) > meleeRange && isFacingPlayer)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
