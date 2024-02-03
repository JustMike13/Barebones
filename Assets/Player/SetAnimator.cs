using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimator : MonoBehaviour
{
    [SerializeField]
    BaseContoller controller;
    bool setBusyByMe = false;
    public bool SetBusy = false;

    void Start()
    {
        controller.Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (SetBusy)
        {
            controller.IsBusy = true;
            setBusyByMe = true;
        }
        else
        {
            NotBusy();
        }
    }

    public void NotBusy()
    {
        if (setBusyByMe)
        {
            controller.IsBusy = false;
            setBusyByMe = false;
        }
    }
}
