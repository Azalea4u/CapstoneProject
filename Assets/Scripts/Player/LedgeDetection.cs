using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float radius = 0.15f;
    [SerializeField] private float ceilingCheckDistance = 1f;
    [SerializeField] private bool canDetectLedge = true;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = playerMovement.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (canDetectLedge)
        {
            // Check for potential ledge
            Collider2D ledgeCollider = Physics2D.OverlapCircle(transform.position, radius, whatIsGround);

            if (ledgeCollider != null && IsValidLedge())
            {
                playerMovement.ledgeDetected = true;
                LockPlayerOnLedge();
            }
            else
            {
                playerMovement.ledgeDetected = false;
                ReleasePlayerFromLedge();
            }
        }
    }

    private bool IsValidLedge()
    {
        RaycastHit2D ceilingHit = Physics2D.Raycast(transform.position, Vector2.up, ceilingCheckDistance, whatIsGround);
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position + Vector3.down * 0.1f, Vector2.down, 0.2f, whatIsGround);

        Debug.Log($"Ceiling Above: {ceilingHit.collider != null}, Ground Below: {groundHit.collider != null}");

        return groundHit.collider != null && ceilingHit.collider == null;
    }

    private void LockPlayerOnLedge()
    {
        if (rb != null)
        {
            rb.gravityScale = 0; // Disable gravity
            rb.velocity = Vector2.zero; // Stop all movement
        }
    }

    private void ReleasePlayerFromLedge()
    {
        if (rb != null)
        {
            rb.gravityScale = 1; // Restore gravity
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDetectLedge = false;
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
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * ceilingCheckDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + Vector3.down * 0.1f, transform.position + Vector3.down * 0.3f);
    }

}

