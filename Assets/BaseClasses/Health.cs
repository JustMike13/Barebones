using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : UIStatusBar
{
    public void TakeDamage(int damageTaken = 5)
    {
        currentValue -= damageTaken;
        SetBarFill(currentValue / maxValue);
        if (currentValue <= 0)
        {
            Destroy(gameObject);
        }
    }
}
