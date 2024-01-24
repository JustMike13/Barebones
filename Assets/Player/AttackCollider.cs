using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField]
    public Attack attack;
    private void OnTriggerEnter(Collider other)
    {
        attack.ProcessCollider(other);
    }
}
