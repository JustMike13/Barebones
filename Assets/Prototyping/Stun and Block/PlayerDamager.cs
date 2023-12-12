using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamager : MonoBehaviour
{
    Health playerHealthObject;
    ThirdPersonController playerController;
    bool prevValue = false;
    bool prevValueF = false;

    void Start()
    {
        playerHealthObject = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonController>();
    }

    
    void Update()
    {
        bool currValue = Input.GetKeyDown(KeyCode.E);
        if (currValue && currValue != prevValue)
        {
            playerHealthObject.TakeDamage(10);
        }
        prevValue = currValue;


        bool currValueF = Input.GetKeyDown(KeyCode.F);
        if (currValueF && currValueF != prevValueF)
        {
            playerController.GetStunned();
        }
        prevValueF = currValueF;
    }
}
