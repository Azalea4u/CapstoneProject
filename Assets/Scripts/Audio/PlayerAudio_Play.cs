
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio_Play : MonoBehaviour
{
    public static PlayerAudio_Play instance;

    [SerializeField] public AudioSource footstep_Audio;
    [SerializeField] public AudioSource dashing_Audio;
    [SerializeField] public AudioSource jump_Audio;
    [SerializeField] public AudioSource landing_Audio;
    [SerializeField] public AudioSource clingCLimb_Audio;
    [SerializeField] public AudioSource damage_Audio;
    [SerializeField] public AudioSource attack01_Audio;
    [SerializeField] public AudioSource attack01Miss_Audio;
    [SerializeField] public AudioSource attack02_Audio;
    [SerializeField] public AudioSource attack02Miss_Audio;
    [SerializeField] public AudioSource attack03_Audio;
    [SerializeField] public AudioSource attack03Miss_Audio;

    private void Awake()
    {
        instance = this;
    }

    public void PlayFootstep()
    {
        footstep_Audio.Play();
    }

    public void Play_Dash()
    {
        dashing_Audio.Play();
    }

    public void PlayJump()
    {
        jump_Audio.Play();
    }

    public void PlayLanding()
    {
        landing_Audio.Play();
    }

    public void PlayClingClimb()
    {
        clingCLimb_Audio.Play();
    }

    public void PlayDamage()
    {
        damage_Audio.Play();
    }

    #region ATTACK
    public void PlayAttack01()
    {
        if (EnemyBase.instance.isHit)
            attack01_Audio.Play();
        else
            attack01Miss_Audio.Play();
    }

    public void PlayAttack02()
    {
        if (EnemyBase.instance.isHit)
            attack02_Audio.Play();
        else
            attack02Miss_Audio.Play();
    }

    public void PlayAttack03()
    {
        if (EnemyBase.instance.isHit)
            attack03_Audio.Play();
        else
            attack03Miss_Audio.Play();
    }
    #endregion
}
