using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource source;
    [SerializeField] AudioClip m_Back1;
    [SerializeField] AudioClip m_Back2;
    [SerializeField] AudioClip m_Back3;
    int i;
    void Start()
    {
    }

    void Update()
    {
        PlayAudio();
    }

    void PlayAudio()
    {
        if (source.isPlaying)
        { return; }

        else if (!source.isPlaying)
        {
            source.PlayOneShot(m_Back1);
        }
    }
}
