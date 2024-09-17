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

    public static PlayerMovement instance;
    public bool isAttacking = false;

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
        float horizontalInput = Input.GetAxis("Horizontal");


        CheckDirection();
        CheckCollision();

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
}