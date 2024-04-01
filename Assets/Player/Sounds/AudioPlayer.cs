using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField]
    List<AudioSrc> sources;

    private void Start()
    {
        StopAll();
    }

    public void Play(string soundName)
    {
        foreach (AudioSrc src in sources)
        {
            if (src.audioSourceName == soundName)
            {
                src.Play();
            }
        }
    }

    public void Stop(string soundName)
    {
        foreach (AudioSrc src in sources)
        {
            if (src.audioSourceName == soundName)
            {
                src.Stop();
            }
        }
    }

    public void StopAll() 
    {
        foreach (AudioSrc src in sources)
        {
            src.Stop();
        }
    }
}
