using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public Animator animator;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public CircleCollider2D GroundCollider;
    [SerializeField] private float speed;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        // Flip the player sprite Left to Right
        if (horizontalInput > 0.01f && spriteRenderer.flipX)
        {
            ChangeDirection(false);
        }
        else if (horizontalInput < -0.01f && !spriteRenderer.flipX)
        {
            ChangeDirection(true);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // Set animator parameters
        animator.SetBool("IsMoving", horizontalInput != 0);
    }

    private void FixedUpdate()
    {
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, speed);
        animator.SetBool("OnGround", false);
    }

    private void ChangeDirection(bool flip)
    {
        spriteRenderer.flipX = flip;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            animator.SetBool("OnGround", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            animator.SetBool("OnGround", false);
        }
    }
}
