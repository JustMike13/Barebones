using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    Animator myAnimator;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) 
        {
            myAnimator.SetTrigger("Start");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            myAnimator.SetTrigger("Reset");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            myAnimator.SetFloat("Speed", 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            myAnimator.SetFloat("Speed", 1f);
        }
    }
}