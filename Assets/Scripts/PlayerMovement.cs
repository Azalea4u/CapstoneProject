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

    public static PlayerMovement instance;
    public bool FacingRight = true;
    public bool isAttacking = false;

    [Header("Collision")]
    [SerializeField] public CircleCollider2D GroundCollider;
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] public CircleCollider2D RightCollider;
    [SerializeField] public CircleCollider2D LeftCollider;
    [SerializeField] public CircleCollider2D Right_LedgeCollider;
    [SerializeField] public CircleCollider2D Left_LedgeCollider;

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
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        CheckDirection();
        CheckCollision();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
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
        animator.SetBool("OnGround", isGrounded);
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

    private void CheckDirection()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // Detect direction change and trigger the direction change animation
        if (horizontalInput > 0.01f && !animator.GetBool("FacingRight"))
        {
            // Moving right
            FacingRight = true;
        }
        else if (horizontalInput < -0.01f && animator.GetBool("FacingRight"))
        {
            // Moving left
            FacingRight = false;
        }
    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(GroundCollider.bounds.center, Vector2.down, GroundCollider.radius, whatIsGround);
        wallDetected = Physics2D.Raycast(RightCollider.bounds.center, Vector2.right, RightCollider.radius, whatIsGround) ||
                      Physics2D.Raycast(LeftCollider.bounds.center, Vector2.left, LeftCollider.radius, whatIsGround);
    }
}