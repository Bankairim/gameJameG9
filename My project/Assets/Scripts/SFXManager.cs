using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VFXManager;

public class SFXManager : MonoBehaviour
{
    [Serializable]
    public class SFX
    {
        public string Id;
        public AudioClip Value;
    }

    public SFX[] Audios;

    public static SFXManager Instance;
    private AudioSource AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        Instance = this;    
    }

    public AudioSource GetAudioSource()
    {
        return AudioSource;
    }

    public void PlayAudio(string Id, float Volume = 0.25f)
    {
        AudioClip audio = null;
        foreach (SFX sfx in Audios)
        {
            if (sfx.Id == Id)
            {
                audio = sfx.Value; break;
            }
        }

        if (audio == null)
        {
            return;
        }

        AudioSource.PlayOneShot(audio, Volume);
    }
}
