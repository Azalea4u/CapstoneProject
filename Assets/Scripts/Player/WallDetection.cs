using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetection : MonoBehaviour
{
    public static WallDetection instance;

    public bool wallAboveLedgeDetected;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            PlayerMovement.instance.wallDetected = true;
            wallAboveLedgeDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            PlayerMovement.instance.wallDetected = false;
            wallAboveLedgeDetected = false;
        }
    }
}
