using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamager : MonoBehaviour
{
    Health playerHealthObject;
    bool prevValue = false;

    void Start()
    {
        playerHealthObject = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    
    void Update()
    {
        bool currValue = Input.GetKeyDown(KeyCode.E);
        if (currValue && currValue != prevValue)
        {
            playerHealthObject.TakeDamage(10);
        }
        prevValue = currValue;
    }
}
