using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickAndStuccoShader : MonoBehaviour
{
    [SerializeField] bool applyScale = false;
    void Start()
    {
        Material material = GetComponent<Renderer>().material;
        float seed;

        seed = transform.position.x * transform.position.y * transform.position.z
            + transform.rotation.x * transform.rotation.z;
        
        material.SetFloat("_Seed", seed);
        if (applyScale )
        {
            material.SetFloat("_ScaleX", transform.localScale.x);
            material.SetFloat("_ScaleZ", transform.localScale.z);
        }
    }

}
