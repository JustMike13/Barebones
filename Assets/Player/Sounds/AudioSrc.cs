using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioSrc : MonoBehaviour
{
    [SerializeField]
    public string audioSourceName;
    AudioSource audioSource;
    bool isPlaying = false;
    public bool IsPlaying {  get { return isPlaying; } }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        if (!isPlaying)
        {
            audioSource.Play();
            isPlaying = true;
        }
    }

    public void Stop()
    {
        audioSource.Stop();
        isPlaying = false;
    }
}
