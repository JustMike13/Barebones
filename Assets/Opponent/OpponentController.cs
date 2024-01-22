using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;

public class OpponentController : BaseContoller
{
    Transform player;

    public float meleeRange = 5f;
    public float speed = 10f;

    SwordParent sword;
    Animator swordAnimator;
    [SerializeField]
    float attackDelay = 4f;
    float timeSinceAttack;

    void Awake()
    {
        GetAnimator();
        player = GameObject.Find("Player").transform;
        sword = GetComponentInChildren<SwordParent>();
        swordAnimator = sword.transform.GetComponent<Animator>();
        timeSinceAttack = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStunned)
        {
            transform.LookAt(player.transform);
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        timeSinceAttack += Time.deltaTime;
        if (Vector3.Distance(transform.position, player.position) > meleeRange)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            if (timeSinceAttack > attackDelay)
            {
                swordAnimator.Play("Attack");
                timeSinceAttack = 0;
            }
        }
    }
}
