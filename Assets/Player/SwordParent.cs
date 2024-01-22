using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordParent : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    bool controllable = true;
    [SerializeField]
    bool isAttacking;
    public bool IsAttacking {  get { return isAttacking; } }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessAttack();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{name} has hit {other.gameObject.name}");
    }

    public void ProcessAttack()
    {
        if (Input.GetButtonDown("Fire1") && controllable)
        {
            animator.Play("Attack");
        }
    }
}
