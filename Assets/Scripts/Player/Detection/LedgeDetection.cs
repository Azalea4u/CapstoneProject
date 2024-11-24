using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private BoxCollider2D frontCheck;
    [SerializeField] private CircleCollider2D ledgeCheck;
    [SerializeField] private LayerMask whatIsGround;

    public bool canDetectLedge = true;
    public bool wallInFront;

    private void Update()
    {
        playerMovement.canGrabLedge = canDetectLedge;

        if (WallDetection.instance.wallAboveLedgeDetected)
        {
            canDetectLedge = false; 
        }
        else
        {
            canDetectLedge = true;
        }

        float area = frontCheck.size.x * frontCheck.size.y;

        wallInFront = Physics2D.Raycast(frontCheck.bounds.center, Vector2.right, area, whatIsGround);
    }

    // check the BoxCollider2D OnTriggerEnter
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            PlayerMovement.instance.wallDetected = true;

            if (!WallDetection.instance.wallAboveLedgeDetected && FrontColliderDetection.instance.frontWallDetected)
            {
                playerMovement.ledgeDetected = true;
                PlayerMovement.instance.wallDetected = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerMovement.ledgeDetected = false;
        }
    }
}                                                 