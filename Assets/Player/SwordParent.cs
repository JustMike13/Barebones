using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordParent : MonoBehaviour
{
    public static int IDLE = 0;
    public static int ATTACKING = 1;
    public static int RETRIEVING = 2;
    int swordState;
    public int SwordState {  get { return swordState; } }
    private Quaternion swordIdleRotation;
    private Quaternion swordEndRotation = Quaternion.Euler(-64.487f, -127.796f, -134.9f);
    private Vector3 originalPosition;
    private Animator animator;
    [SerializeField]
    bool controllable = true;

    // Start is called before the first frame update
    void Start()
    {
        swordIdleRotation = transform.rotation;
        swordState = IDLE;
        originalPosition = transform.localPosition;
        animator = GetComponent<Animator>();
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
        if (swordState == IDLE)
        {
            if (Input.GetButtonDown("Fire1") && controllable)
            {
                //swordState = ATTACKING;
                animator.Play("Attack");
            }
        }
        //ProcessAnimation();
    }

    public void ProcessAnimation()
    {
        float zRotation = transform.localEulerAngles.z < 0 ? transform.localEulerAngles.z + 360 : transform.localEulerAngles.z;
        
        if (swordState == RETRIEVING)
        {
            if (zRotation < 35 || zRotation > 225)
            {
                transform.Rotate(new Vector3(0, 0, 2000) * Time.deltaTime);
                transform.Translate(Vector3.left * 20 * Time.deltaTime);
            }
            else
            {
                transform.localRotation = swordIdleRotation;
                transform.localPosition = originalPosition;
                swordState = IDLE;
            }
        }
        if (swordState == ATTACKING)
        {
            if (zRotation < 35 || zRotation > 225)
            {
                transform.Rotate(new Vector3(0, 0, -2000) * Time.deltaTime);
                transform.Translate(Vector3.right * 20 * Time.deltaTime);
            }
            else
            {
                transform.localRotation = swordEndRotation;
                swordState = RETRIEVING;
            }
        }
    }
}
