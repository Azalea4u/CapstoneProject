
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeAudio_Play : EnemyAudio_Play
{
    [Header("Flying Eye Info")]
    [SerializeField] public AudioSource flap_Audio;
    [SerializeField] public AudioSource bite_audio;
    [SerializeField] public AudioSource biteMiss_Audio;

    public void PlayFootstep()
    {
        flap_Audio.Play();
    }

    public void PlayAttack01()
    {
        if (EnemyBase.instance.playerHit)
            bite_audio.Play();
        else
            biteMiss_Audio.Play();
    }
}
