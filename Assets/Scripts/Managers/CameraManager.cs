using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimations : MonoBehaviour
{
    private Animator cameraAnimator;

    void Awake()
    {
        cameraAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        cameraAnimator.SetBool("IsCrouching", PlayerMovement.instance.isCrouching);
        cameraAnimator.SetBool("IsTalking", DialogueManager.instance.dialogueIsPlaying);
    }
}
