using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : UIStatusBar
{
    void Start()
    {
        Refill();
    }
    public void TakeDamage(float damageTaken = 5)
    {
        currentValue -= damageTaken;
        SetBarFill(currentValue / maxValue);
        if (currentValue <= 0)
        {
            gameObject.GetComponent<CharacterManager>().Death();
        }
    }
}
