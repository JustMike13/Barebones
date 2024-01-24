using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;

public class OpponentController : BaseContoller
{
    Transform player;

    public float meleeRange = 5f;
    public float speed = 10f;
    [SerializeField]
    Vector2 attackDelayRange;
    float attackDelay = 4f;
    float timeSinceAttack;
    [SerializeField]
    List<Attack> attacks;

    void Awake()
    {
        GetAnimator();
        player = GameObject.Find("Player").transform;
        timeSinceAttack = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ///////////   -  REMOVE  -   //////////
        if (Input.GetKeyDown(KeyCode.H))
        {
            attacks.ElementAt(1).UseAttack();
        }
        ///////////////////////////////////////
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
    }

    override public void LookAtPlayer()
    {
        transform.LookAt(player.transform);
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
        if (Vector3.Distance(transform.position, player.position) > meleeRange)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
