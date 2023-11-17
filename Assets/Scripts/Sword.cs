using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sword : MonoBehaviour
{
    int IDLE = 0;
    int ATTACKING = 1;
    int RETRIEVING = 2;
    public int swordState;
    private Quaternion swordIdleRotation;
    private Quaternion swordEndRotation = Quaternion.Euler(-64.487f, -127.796f, -134.9f);
    private Vector3 originalPosition;
    [SerializeField]
    GameObject HitCollider;
    private float hitRadius = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        swordIdleRotation = transform.rotation;
        swordState = IDLE;
        originalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{name} has hit {other.gameObject.name}");
    }

    public void ProcessAttack()
    {
        if (swordState == IDLE)
        {
            if (Input.GetAxis("Fire1") != 0)
            {
                swordState = ATTACKING;
                ProcessDamage();
            }
        }
        ProcessAnimation();
    }

    private void ProcessDamage()
    {
        Vector3 sphereCenter = HitCollider.transform.position;
        Collider[] hits = Physics.OverlapSphere(sphereCenter, hitRadius, -1);
        int hitCount = 0;
        foreach(Collider collider in hits)
        {
            //Debug.Log($"I hit something {collider.gameObject.name}");
            if (collider.gameObject.GetComponent<Enemy>() != null)
            {
                hitCount++;
                collider.gameObject.GetComponent<Enemy>().Damage();
                Debug.Log($"I hit  {collider.gameObject.name}");
            }
        }

        hits = null;
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
