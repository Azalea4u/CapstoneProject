using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontColliderDetection : MonoBehaviour
{
    public static FrontColliderDetection instance;
    public bool frontWallDetected;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        PlayerMovement.instance.wallDetected = frontWallDetected;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            frontWallDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            frontWallDetected = false;
        }
    }
}
