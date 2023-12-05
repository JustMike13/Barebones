using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    int state;
    List<Enemy> hitList;
    SwordParent parent;
    [SerializeField]
    int swordDamage = 10;

    void Start()
    {
        hitList = new List<Enemy>();
        parent = transform.parent.GetComponent<SwordParent>();
    }

    // Update is called once per frame
    void Update()
    {
        state = parent.SwordState;

        if (state == SwordParent.RETRIEVING)
        {
            foreach (Enemy enemy in hitList)
            {
                enemy.Damage(swordDamage);
            }
            hitList.Clear();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy == null) 
        {
            return;
        }
        state = parent.SwordState;
        if (state == SwordParent.ATTACKING)
        {
            hitList.Add(enemy);
        }
    }
}
