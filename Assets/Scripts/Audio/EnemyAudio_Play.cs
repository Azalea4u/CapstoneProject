
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio_Play : MonoBehaviour
{
    public static EnemyAudio_Play instance;

    [SerializeField] public AudioSource isHit_Audio;


    private void Awake()
    {
        instance = this;
    }

    public void Play_IsHit()
    {
        isHit_Audio.Play();
    }
}
