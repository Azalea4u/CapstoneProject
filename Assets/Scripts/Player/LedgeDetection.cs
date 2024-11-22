using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CircleCollider2D ledgeCheck;
    [SerializeField] private LayerMask whatIsGround;

    public bool canDetectLedge = true;

    private void Update()
    {
        playerMovement.canGrabLedge = canDetectLedge;

        if (WallDetection.instance.wallAboveLedgeDetected)
        {
            //playerMovement.ledgeDetected = false;
            canDetectLedge = false; 
        }
        else
        {
            canDetectLedge = true;
        }
    }

    private void FixedUpdate()
    {
        if (canDetectLedge && !PlayerMovement.instance.isClimbing &&
            (!WallDetection.instance.wallAboveLedgeDetected || !PlayerMovement.instance.wallDetected))
        {
            playerMovement.ledgeDetected = Physics2D.OverlapCircle(transform.position, ledgeCheck.radius, whatIsGround);
        }
    }

    // check the BoxCollider2D OnTriggerEnter
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //canDetectLedge = false;
            //playerMovement.ledgeDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
           // canDetectLedge = true;
        }
    }
}                                                 