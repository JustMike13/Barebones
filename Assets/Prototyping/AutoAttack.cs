using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    bool autoAttack = false;
    Animator animator;
    bool prevValueA;
    bool prevValueS;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine("AutoAttackCoroutine");
    }

    // Update is called once per frame
    void Update()
    {
        bool currValueA = Input.GetKeyDown(KeyCode.Z);
        if (currValueA && currValueA != prevValueA)
        {
            autoAttack = !autoAttack;
        }
        prevValueA = currValueA;


        bool currValueS = Input.GetKeyDown(KeyCode.X);
        if (currValueS && currValueS != prevValueS)
        {
            animator.Play("Attack");
        }
        prevValueS = currValueS;
    }

    IEnumerable AutoAttackCoroutine()
    {
        while (true)
        {
            if (autoAttack)
            {
                animator.Play("Attack");
            }
            yield return new WaitForSeconds(1);
        }
    }
}
