using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Components")]
    [SerializeField] public Animator animator;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] private float speed;
    [SerializeField] private float jumpingPower;
    [SerializeField] private float knockbackForce = 10.0f;
    [SerializeField] private float staggerDuration = 0.5f;

    public static PlayerMovement instance;
    public bool isAttacking = false;
    public bool isAlive = true;
    public bool isStaggered = false;

    [Header("Collision")]
    [SerializeField] public CircleCollider2D GroundCollider;
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] public CircleCollider2D FrontCollider;
    [SerializeField] public CircleCollider2D BackCollider;
    [SerializeField] public CircleCollider2D LedgeCollider;

    public bool isGrounded = true;
    public bool wallDetected = false;
    public bool ledgeDetected = false;

    private Rigidbody2D rb;


    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isAlive)
        {
            animator.SetBool("IsDead", true);
            rb.velocity = Vector2.zero;
            return;
        }

        if (isStaggered)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        float horizontalInput = Input.GetAxis("Horizontal");

        // Check for collisions
        CheckDirection();
        CheckCollision();

        // INPUTS
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
                Jump();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking)
        {
            Attack();
        }

        // Set animator parameters
        animator.SetBool("IsMoving", horizontalInput != 0);
        animator.SetBool("OnGround", isGrounded);
    }

    private void FixedUpdate()
    {
        // If the player is dead, stop all movement
        if (!isAlive || isStaggered)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    private void Attack()
    {
        isAttacking = true;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        animator.SetBool("OnGround", false);
    }

    private void CheckDirection()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // Detect direction change and trigger the direction change animation
        if (horizontalInput > 0.01f)
        {
            // Moving right
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            // Moving left
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(GroundCollider.bounds.center, Vector2.down, GroundCollider.radius, whatIsGround);

        wallDetected = Physics2D.Raycast(FrontCollider.bounds.center, Vector2.right, FrontCollider.radius, whatIsGround) ||
                      Physics2D.Raycast(BackCollider.bounds.center, Vector2.left, BackCollider.radius, whatIsGround);
    }

    public void Stagger(Vector2 knockbackDirection)
    {
        if (!isAlive) return; // Don't stagger if the player is dead

        // Apply knockback force in the specified direction
        rb.velocity = new Vector2(knockbackDirection.x * knockbackForce, rb.velocity.y);

        // Start stagger coroutine to disable movement for a duration
        StartCoroutine(StaggerCoroutine());
    }

    // Coroutine to handle stagger recovery
    private IEnumerator StaggerCoroutine()
    {
        isStaggered = true; // Disable player input
        yield return new WaitForSeconds(staggerDuration); // Wait for stagger duration
        isStaggered = false; // Re-enable player input
    }
}