using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetection : MonoBehaviour
{
    public static WallDetection instance;

    [SerializeField] private BoxCollider2D wallAboveCollider;
    //[SerializeField] public DetectionZone zone;
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] public float waitTime = 2.0f;
    public bool wallAboveLedgeDetected;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            wallAboveLedgeDetected = true;
        }
    }

    private Coroutine resetCoroutine;

    private void OnTriggerExit2D(Collider2D collision)
    {
        wallAboveLedgeDetected = false;

    }
}
