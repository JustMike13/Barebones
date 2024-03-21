using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class LongRangeAttack : Attack
{
    [SerializeField]
    GameObject projectilePrefab;
    [SerializeField]
    float projectileSpeed;
    [SerializeField]
    float projectileMaxDistance;
    GameObject projectileInstance;
    Vector3 startingPosition;
    float timePassed;
    [SerializeField]
    Vector2 chargeTimeRange;
    float chargeTime;
    [SerializeField]
    GameObject chargingFlames;
    [SerializeField]
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        controller = transform.parent.GetComponent<BaseContoller>();
        projectileInstance = null;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCooldown();
        if (timePassed < chargeTime)
        {
            timePassed += Time.deltaTime;
            controller.LookAtPlayer();
        }
        else if (chargeTime > 0)
        {
            chargeTime = -1;
            startingPosition = transform.position;
            cooldownLeft = cooldown;
            projectileInstance = Instantiate(projectilePrefab, startingPosition, Quaternion.identity);
            projectileInstance.GetComponent<AttackCollider>().attack = this;
            projectileInstance.GetComponent<LongRangeProjectile>().SetMembers(projectileSpeed, projectileMaxDistance, player);
            chargingFlames.gameObject.SetActive(false);
            controller.IsAttacking = false;
        }
    }

    override public void ProcessCollider(Collider other)
    {
        // TODO: Replace with CharacterManager
        Health playerHealth = other.GetComponent<Health>();
        if (playerHealth == null)
        {
            return;
        }
        playerHealth.TakeDamage(damage);
    }

    override public void UseAttack()
    {
        if (!controller.IsAttacking)
        {
            controller.IsAttacking = true;
            timePassed = 0;
            chargeTime = Random.Range(chargeTimeRange.x, chargeTimeRange.y);
            chargingFlames.gameObject.SetActive(true);
        }
    }
}
