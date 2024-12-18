using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource gameMusic;
    [SerializeField] private AudioSource restingMusic;

    [Header("Scenes")]
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject gameLevel;
    [SerializeField] private GameObject restingLevel;

    private void Update()
    {
        if (gameLevel.activeSelf == true)
        {
            if (!gameMusic.isPlaying)
            {
                gameMusic.Play();
                restingMusic.Stop();
            }
        }
        else if (startMenu.activeSelf || restingLevel.activeSelf)
        {
            if (!restingMusic.isPlaying)
            {
                restingMusic.Play();
                gameMusic.Stop();
            }

        }
    }

    private void ChangeMusic()
    {

    }
}
