using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public QMovement mov;

    public AudioSource source;
    [SerializeField] AudioClip m_Back1;
    [SerializeField] AudioClip m_Back2;
    [SerializeField] AudioClip m_Back3;
    [SerializeField] AudioClip s_Jump;
    AudioClip[] clipAr;
    int rd;

    private void Awake()
    {
        clipAr = new AudioClip[] {m_Back1, m_Back2, m_Back3};
    }

    void Update()
    {
        PlayAudio();
        JumpAudio();
    }

    void PlayAudio()
    {
        if (source.isPlaying)
        { return; }

        else if (!source.isPlaying)
        {
            rd = Random.Range(0, 3);
            source.PlayOneShot(clipAr[rd]);
        }
    }

    private void JumpAudio()
    {
        if (mov.body.isGrounded && Input.GetKey(KeyCode.Space))
        {
            source.PlayOneShot(s_Jump);
        }
    }
}
