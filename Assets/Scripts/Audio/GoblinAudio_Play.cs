
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAudio_Play : EnemyAudio_Play
{
    [Header("Goblin Info")]
    [SerializeField] public AudioSource footstep_Audio;
    [SerializeField] public AudioSource slash_audio;
    [SerializeField] public AudioSource slashMiss_Audio;

    public void PlayFootstep()
    {
        footstep_Audio.Play();
    }

    public void PlayAttack01()
    {
        if (EnemyBase.instance.playerHit)
            slash_audio.Play();
        else
            slashMiss_Audio.Play();
    }
}
