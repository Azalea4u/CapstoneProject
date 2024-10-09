using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CircleCollider2D ledgeCheck;
    [SerializeField] private LayerMask whatIsGround;

    private bool canDetectLedge = true;

    private void Update()
    {
        if (canDetectLedge)
        {
            playerMovement.ledgeDetected = Physics2D.OverlapCircle(transform.position, ledgeCheck.radius, whatIsGround);
        }
    }

    // check the BoxCollider2D OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDetectLedge = false;
            //playerMovement.ledgeDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDetectLedge = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, radius);
    }
}
