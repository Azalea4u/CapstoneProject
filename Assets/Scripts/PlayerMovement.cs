using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public Animator animator;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public CircleCollider2D GroundCollider;
    [SerializeField] private float speed;

    public static PlayerMovement instance;
    public bool isAttacking = false;

    private Rigidbody2D rb;
    private bool FacingRight = true;
    private bool isChangingDirection = false;

    private void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        /*
        // Flip the player sprite Left to Right
        if (horizontalInput > 0.01f && spriteRenderer.flipX)
        {
            ChangeDirection(false);
        }
        else if (horizontalInput < -0.01f && !spriteRenderer.flipX)
        {
            ChangeDirection(true);
        }
        */

        // Detect direction change and trigger the direction change animation
        if (horizontalInput > 0.01f && !animator.GetBool("FacingRight") && !isChangingDirection)
        {
            FacingRight = true;
            StartCoroutine(ChangeDirection(true)); // Moving right
        }
        else if (horizontalInput < -0.01f && animator.GetBool("FacingRight") && !isChangingDirection)
        {
            FacingRight = false;
            StartCoroutine(ChangeDirection(false)); // Moving left
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking)
        {
            Attack();
        }

        // Set animator parameters
        animator.SetBool("IsMoving", horizontalInput != 0);
        animator.SetBool("FacingRight", FacingRight);
    }

    private void FixedUpdate()
    {
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    private void Attack()
    {
        isAttacking = true;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, speed);
        animator.SetBool("OnGround", false);
    }

    //private void ChangeDirection(bool flip)
    //{
    //    spriteRenderer.flipX = flip;
    //}

    // Coroutine to handle direction change animation
    private IEnumerator ChangeDirection(bool facingRight)
    {
        isChangingDirection = true;  // Set flag to prevent multiple direction changes

        // Trigger the ChangeDirection animation if you have one

        // Wait for the direction change animation to finish (assuming it's 0.5s, adjust as needed)
        yield return new WaitForSeconds(0.5f);

        // Update FacingRight bool in the animator
        animator.SetBool("FacingRight", facingRight);

        isChangingDirection = false;  // Reset flag after changing direction
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
