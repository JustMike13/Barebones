using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitCollider : MonoBehaviour
{
    [SerializeField]
    SwordAttack sword;
    private void OnTriggerEnter(Collider other)
    {
        sword.ProcessCollider(other);
    }

}
