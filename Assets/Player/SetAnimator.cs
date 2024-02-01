using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimator : MonoBehaviour
{
    [SerializeField]
    BaseContoller controller;

    void Start()
    {
        controller.Animator = GetComponent<Animator>();
    }
}
