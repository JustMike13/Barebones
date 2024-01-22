using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentHealth : Health
{
    // Start is called before the first frame update
    void Start()
    {
        healthBar = GameObject.FindGameObjectWithTag("OpponentHealthBar");
        RefillHealth();
    }
}
