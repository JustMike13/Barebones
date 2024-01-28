using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField]
    protected int damage;
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
    [DoNotSerialize]
    public bool parryWindowOn;
    protected bool oldParryWindowOn;

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
    public bool CorrectParry(float blockingTime)
    {
        return canBeParried && blockingTime >= parryWindow.x && blockingTime <= parryWindow.y;
    }
}
