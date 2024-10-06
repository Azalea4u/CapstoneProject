using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    [Header("Player Components")]
    [SerializeField] public Animator animator;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public TrailRenderer trailRenderer;
    [SerializeField] private float speed;
    [SerializeField] private float jumpingPower;
    [SerializeField] private float knockbackForce = 10.0f;
    [SerializeField] private float staggerDuration = 0.5f;
    [SerializeField] private float facingDirection;

    public bool isAttacking = false;
    public bool isAlive = true;

    private bool isStaggered = false;
    private bool canMove = true;
    private bool canFlip = true;

    [Header("Dash")]
    [SerializeField] private float dashDistance = 3.0f;
    [SerializeField] private float dashSpeed = 20.0f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashRechargeTime = 1.0f;
    private Vector2 dashingDirection;
    private bool canDash = true;
    private bool isDashing;
    private float dashStartTime;
    private float currentDashRechargeTime;
    private Vector2 dashStartPosition;

    [Header("Crouch")]
    [SerializeField] private BoxCollider2D standingCollider;
    [SerializeField] private BoxCollider2D crouchCollider;
    public bool isCrouching;
    private bool canCrouch = true;

    [Header("Collision")]
    [SerializeField] public CircleCollider2D GroundCollider;
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] public CircleCollider2D FrontCollider;
    [SerializeField] public CircleCollider2D BackCollider;
    [SerializeField] public CircleCollider2D LedgeCollider;

    public bool isGrounded;
    public bool wallDetected;
    public bool ledgeDetected;

    private Rigidbody2D rb;


    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Check to allow Player to move
        if (!isAlive)
        {
            animator.SetBool("IsDead", true);
            return;
        }

        facingDirection = Input.GetAxis("Horizontal");
        var dashInput = Input.GetButtonDown("Dash");

        if (isGrounded)
        {
            canDash = true;
            canCrouch = true;
        }

        // Check for collisions
        CheckDirection();
        CheckCollision();
        CheckInputs();
        CheckDash();

        // Set animator parameters
        animator.SetBool("IsMoving", facingDirection != 0);
        animator.SetBool("IsDashing", isDashing);
        animator.SetBool("OnGround", isGrounded);
        animator.SetBool("IsCrouching", isCrouching);
    }

    private void FixedUpdate()
    {
        // If the player is dead, stop all movement
        if (!isAlive || isStaggered)
        {
            StopMovement();
            return;
        }
        else
        {
            ContinueMovement();
        }

        if (!isDashing && !isCrouching)
        {
            rb.velocity = new Vector2(facingDirection * speed, rb.velocity.y);
        }
        else
        {
            // Set the velocity to the dashing speed
            rb.velocity = dashingDirection * dashSpeed;
        }

        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    private void StopMovement()
    {
        rb.velocity = Vector2.zero;
        canFlip = false;
        canMove = false;
    }

    private void ContinueMovement()
    {
        canFlip = true;
        canMove = true;
    }

    private void CheckInputs()
    {
        if (canMove)
        {
            // JUMP
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded)
                    Jump();
            }

            // ATTACK
            if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking)
            {
                Attack();
            }

            // DASH
            if (Input.GetButtonDown("Dash"))
            {
                if (canDash)
                    Dash();
            }

            // CROUCH
            if (Input.GetButtonDown("Crouch"))
            {
                if (canCrouch)
                    Crouch();
            }
            else if (Input.GetButtonUp("Crouch"))
            {
                ContinueMovement();
                isCrouching = false;
                animator.SetTrigger("Stand");

                standingCollider.enabled = true;
                crouchCollider.enabled = false;

            }
        }
    }

    private void Attack()
    {
        isAttacking = true;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        isGrounded = false;
    }

    private void Dash()
    {
        animator.SetTrigger("Dash");

        isDashing = true;
        canDash = false;
        dashStartTime = Time.time;
        dashStartPosition = transform.position;
        trailRenderer.emitting = true;
        dashingDirection = new Vector2(facingDirection, Input.GetAxisRaw("Vertical"));

        if (dashingDirection == Vector2.zero)
        {
            dashingDirection = new Vector2(transform.localScale.x, 0);
        }

        // Normalize the dashing direction
        dashingDirection.Normalize();
    }

    private void Crouch()
    {
        isCrouching = !isCrouching;
        StopMovement();

        if (isCrouching)
        {
            animator.SetTrigger("Crouch");

            standingCollider.enabled = false;
            crouchCollider.enabled = true;
        }
    }

    private void CheckDash()
    {
        if (isDashing)
        {
            // Calculate the current position based on the dash progress
            float dashProgress = (Time.time - dashStartTime) / dashDuration;
            Vector2 targetPosition = Vector2.Lerp(dashStartPosition, dashStartPosition + dashingDirection * dashDistance, dashProgress);
            //transform.position = Vector2.Lerp(dashStartPosition, dashStartPosition + dashingDirection * dashDistance, dashProgress);

            RaycastHit2D hit = Physics2D.Linecast(transform.position, targetPosition, whatIsGround);

            if (hit.collider != null)
            {
                // Stop the dash if a collision is detected
                isDashing = false;
                currentDashRechargeTime = dashRechargeTime;
                trailRenderer.emitting = false;
                transform.position = hit.point; // Set the player's position to the collision point
            }
            else
            {
                transform.position = targetPosition;
            }

            if (dashProgress >= 1f)
            {
                isDashing = false;
                currentDashRechargeTime = dashRechargeTime;
                trailRenderer.emitting = false; // Stop the trail renderer when the dash is completed
            }
        }
        else
        {
            if (currentDashRechargeTime > 0f)
            {
                currentDashRechargeTime -= Time.deltaTime;
                if (currentDashRechargeTime <= 0f)
                {
                    canDash = true;
                }
            }
        }
    }

    private void CheckDirection()
    {
        // Detect direction change and trigger the direction change animation
        if (facingDirection > 0.01f && canFlip)
        {
            // Moving right
            transform.localScale = Vector3.one;
        }
        else if (facingDirection < -0.01f && canFlip)
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

    #region STAGGER
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
    #endregion
}