using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    int state;
    List<Health> hitList;
    SwordParent parent;
    [SerializeField]
    int swordDamage = 10;

    void Start()
    {
        hitList = new List<Health>();
        parent = transform.parent.GetComponent<SwordParent>();
    }

    // Update is called once per frame
    void Update()
    {
        state = parent.SwordState;

        if (state == SwordParent.RETRIEVING)
        {
            foreach (Health enemyHealth in hitList)
            {
                enemyHealth.TakeDamage(swordDamage);
            }
            hitList.Clear();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Health enemyHealth = other.GetComponent<Health>();
        if (enemyHealth == null) 
        {
            return;
        }
        state = parent.SwordState;
        if (state == SwordParent.ATTACKING)
        {
            hitList.Add(enemyHealth);
        }
    }
}
