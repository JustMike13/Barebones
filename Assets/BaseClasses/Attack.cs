using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    float damage;
    [SerializeField]
    Vector2 range;
    bool canBeBlocked;
    bool canBeParried;

    public virtual void UseAttack() { }
    public virtual bool CanUse() { return false; }
    public bool InRange(float distance)
    {
        return (distance >= range.x && (distance <= range.y || range.y == 0)) ;
    }
}
