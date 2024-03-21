using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioSrc : MonoBehaviour
{
    [SerializeField]
    public string name;
    bool isPlaying = false;
    public bool IsPlaying {  get { return isPlaying; } }

    public void Play()
    {
        if (!isPlaying)
        {
            GetComponent<AudioSource>().Play();
            isPlaying = true;
        }
    }

    public void Stop()
    {
        GetComponent<AudioSource>().Stop();
    }
}
