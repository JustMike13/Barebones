using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField]
    protected int damage;
    [SerializeField]
    Vector2 range;
    bool canBeBlocked;
    bool canBeParried;
    [SerializeField]
    protected float cooldown = 1f;
    protected float cooldownLeft;

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
    public virtual bool CanUse() { return cooldownLeft <= 0; }
    public bool InRange(float distance)
    {
        return (distance >= range.x && (distance <= range.y || range.y == 0)) ;
    }
    public virtual void ProcessCollider(Collider other) { }
}
