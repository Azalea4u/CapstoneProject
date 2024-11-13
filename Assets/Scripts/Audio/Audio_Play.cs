
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Play : MonoBehaviour
{
    [SerializeField] public AudioSource audioSource;

    // Start is called before the first frame update
    public virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
