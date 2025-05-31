using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    [Header("Player Components")]
    [SerializeField] protected PlayerHealth playerHealth;
    [SerializeField] public Animator animator;
    [SerializeField] public TrailRenderer trailRenderer;
    [SerializeField] private float speed;
    [SerializeField] private float jumpingPower;
    [SerializeField] private float knockbackForce = 10.0f;
    [SerializeField] private float staggerDuration = 0.5f;
    [SerializeField] private float facingDirection;
    private float inputHorizontal;

    // REQUIRED TO BE PUBLIC
    public bool isAttacking = false;
    public bool isAlive = true;
    public bool canMove = true;

    private bool isStaggered = false;
    private bool canFlip = true;
    private bool facingRight = true;

    [Header("Dash")]
    [SerializeField] private float dashDistance = 3.0f;
    [SerializeField] private float dashSpeed = 20.0f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashRechargeTime = 1.0f;
    public bool isDashing;
    private bool canDash = true;
    private float dashStartTime;
    private float currentDashRechargeTime;
    private Vector2 dashStartPosition;
    private Vector2 dashingDirection;

    [Header("Crouch")]
    [SerializeField] private BoxCollider2D standingCollider;
    [SerializeField] private BoxCollider2D crouchCollider;
    public bool isCrouching;
    private bool canCrouch = true;

    [Header("Fall")]
    [SerializeField] private float fallMultiplier = 5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Collision")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private CapsuleCollider2D GroundCollider;
    [SerializeField] private BoxCollider2D FrontCollider;
    [SerializeField] private CircleCollider2D BackCollider;
    // REQUIRED TO BE PUBLIC
    public bool isGrounded;
    public bool wallDetected;

    [Header("Ledge Detection")]
    [SerializeField] private Vector2 Right_offset1;
    [SerializeField] private Vector2 Right_offset2;
    [SerializeField] private Vector2 Left_offset1;
    [SerializeField] private Vector2 Left_offset2;
    private Vector2 climbBeginPosition;
    private Vector2 climbOverPosition;
    // REQUIRED TO BE PUBLIC
    public bool canGrabLedge = true;
    public bool isClimbing; // Check if the enemy can climb
    public bool climbingAllowed = true;
    public bool ledgeDetected;
    private bool ledgePositionSet = false;

    [Header("Bomb")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private Transform bombSpawnPoint;

    [HideInInspector] public Rigidbody2D rb;

    private void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        crouchCollider.enabled = false;
    }

    private void Update()
    {
        // SET DEATH
        if (!isAlive)
        {
            StartCoroutine(Death());
            return;
        }

        if (canMove)
        {
            inputHorizontal = Input.GetAxis("Horizontal");
            var dashInput = Input.GetButtonDown("Dash");

            if (isGrounded)
            {
                canDash = true;
                canCrouch = true;
                isClimbing = false;
            }

            // ANIMATOR
            animator.SetBool("IsMoving", facingDirection != 0);
            animator.SetBool("IsDashing", isDashing);
            animator.SetBool("OnGround", isGrounded);
            animator.SetBool("IsCrouching", isCrouching);
            animator.SetBool("IsClimbing", isClimbing);
            animator.SetBool("ClimbingAllowed", climbingAllowed);
            animator.SetBool("IsAttacking", isAttacking);
        }

        CheckInputs();
    }

    private void FixedUpdate()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying || GameManager.instance.isGamePaused)
        {
            StopMovement();
            animator.Play("Idle");
            rb.gravityScale = 2;
            //StopMovement();
            return;
        }

        // CHECK COLLISION
        if (!isClimbing)
        {
            CheckDirection();
        }
        CheckCollision();
        CheckDash();
        CheckForLedge();
        facingDirection = inputHorizontal;

        canFlip = !isAttacking;

        // STOP MOVEMENT
        if (!isAlive || isStaggered)
        {
            StopMovement();
            return;
        }
        else // CONTINUE MOVEMENT
        {
            ContinueMovement();
        }

        if (!isDashing && !isCrouching)
        {
            rb.linearVelocity = new Vector2(facingDirection * speed, rb.linearVelocity.y);
        }
        else if (isDashing)
        {
            // Set the x velocity to the dashing speed
            rb.linearVelocity = dashingDirection * dashSpeed;
        }
        else if (isCrouching)
        {
            rb.linearVelocity = Vector2.zero;
        }

        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        ApplyFallMultiplier();
    }

    public void StopMovement()
    {
        canFlip = false;
        canMove = false;

        facingDirection = 0;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 1;
    }

    public void ContinueMovement()
    {
        canFlip = true;
        canMove = true;
    }

    private void CheckInputs()
    {
        if (canMove)
        {
            // JUMP
            if (Input.GetKeyDown(KeyCode.Space) && !isCrouching
                && !DialogueManager.instance.dialogueIsPlaying)
            {
                if (isGrounded)
                    Jump();
            }

            // ATTACK
            if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking 
                && (SceneManager.GetActiveScene().name != "Rest_Level"))
            {
                Attack();
            }

            // DASH
            if (Input.GetButtonDown("Dash") && !isClimbing)
            {
                if (canDash)
                    Dash();
            }

            // CROUCH
            if (!isClimbing && isGrounded)
            {
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
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        // LEDGE
        if (isClimbing)
        {
            // if right mouse button is clicked
            if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.LeftShift)))
            {
                animator.SetTrigger("Climbing");
            }
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftControl))
            {
                LedgeFallDown();
            }
        }
    }

    private void Attack()
    {
        isAttacking = true;
    }

    private void Crouch()
    {
        isCrouching = !isCrouching;

        if (isCrouching)
        {
            animator.SetTrigger("Crouch");

            standingCollider.enabled = false;
            crouchCollider.enabled = true;
        }
    }

    #region JUMP
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        isGrounded = false;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
    }

    private void ApplyFallMultiplier()
    {
        if (!isGrounded && rb.linearVelocity.y < 0)
        {
            // Player is falling
            float multiplier = fallMultiplier;

            if (isDashing && dashingDirection.y > 0)
            {
                multiplier *= 10.0f;
            }

            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (multiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    #endregion

    #region DASH
    private void Dash()
    {
        isDashing = true;
        canDash = false;
        dashStartTime = Time.time;
        dashStartPosition = transform.position;
        trailRenderer.emitting = true;
        dashingDirection = new Vector2(facingDirection, Input.GetAxisRaw("Vertical"));

        // if pressing up at all, don't dash up
        if (dashingDirection.y > 0)
        {
            dashingDirection.y = 0;
        }

        if (dashingDirection == Vector2.zero)
        {
            dashingDirection = new Vector2(transform.localScale.x, 0);
        }

        // Normalize the dashing direction
        dashingDirection.Normalize();
    }

    private void CheckDash()
    {
        if (isDashing)
        {
            // Calculate the current position based on the dash progress
            float dashProgress = (Time.time - dashStartTime) / dashDuration;
            Vector2 targetPosition = Vector2.Lerp(dashStartPosition, dashStartPosition + dashingDirection * dashDistance, dashProgress);

            RaycastHit2D hit = Physics2D.Linecast(transform.position, targetPosition, whatIsGround);

            if (hit.collider != null)
            {
                EndDash(hit.point);
            }
            else
            {
                if (dashProgress >= 1f)
                {
                    EndDash(targetPosition);
                }

                rb.MovePosition(targetPosition);
            }
        }
        else
        {
            if (currentDashRechargeTime > 0f)
            {
                currentDashRechargeTime -= Time.deltaTime;
                if (currentDashRechargeTime <= 0f && isGrounded)
                {
                    canDash = true;
                }
            }
        }
    }

    private void EndDash(Vector2 endPosition)
    {
        // Stop the dash if a collision is detected
        isDashing = false;
        currentDashRechargeTime = dashRechargeTime;
        trailRenderer.emitting = false;
        rb.MovePosition(endPosition);
    }
    #endregion

    private void CheckDirection()
    {
        // Detect direction change and trigger the direction change animation
        if (facingDirection > 0.01f && canFlip)
        {
            // Moving right
            transform.localScale = Vector3.one;
            facingRight = true;
        }
        else if (facingDirection < -0.01f && canFlip)
        {
            // Moving left
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
        }
    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.OverlapArea(
            GroundCollider.bounds.min,
            GroundCollider.bounds.max,
            whatIsGround
        );

        wallDetected = Physics2D.OverlapArea(
            FrontCollider.bounds.min,
            FrontCollider.bounds.max,
            whatIsGround
        );

        if (isClimbing)
        {
            playerHealth.isInvincible = true;
        }
    }

    #region LEDGE
    private void CheckForLedge()
    {
        if (ledgeDetected && canGrabLedge && !ledgePositionSet)
        {
            // Validate ledge with additional ground check
            Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;
            RaycastHit2D groundHit = Physics2D.Raycast(ledgePosition + Vector2.down * 0.1f, Vector2.down, 0.2f, whatIsGround);

            if (groundHit.collider == null)
            {
                Debug.Log("Invalid ledge detected. No ground below.");
                ledgeDetected = false;
                return;
            }

            canGrabLedge = false;
            ledgePositionSet = true;

            standingCollider.enabled = false;
            crouchCollider.enabled = true;

            if (facingRight) // FACING RIGHT
            {
                climbBeginPosition = ledgePosition + Right_offset1;
                climbOverPosition = ledgePosition + Right_offset2;
            }
            else // FACING LEFT
            {
                climbBeginPosition = new Vector2(ledgePosition.x - Left_offset1.x, ledgePosition.y + Left_offset1.y);
                climbOverPosition = new Vector2(ledgePosition.x - Left_offset2.x, ledgePosition.y + Left_offset2.y);
            }

            isClimbing = true;
        }

        if (isClimbing)
        {
            isGrounded = false;
            transform.position = climbBeginPosition;
            ledgeDetected = false;
        }
    }


    public void LedgeClimbOver()
    {
        // Validate climb over position
        RaycastHit2D hit = Physics2D.Raycast(climbOverPosition, Vector2.down, 1f, whatIsGround);
        if (hit.collider == null)
        {
            Debug.LogWarning("No ground detected at climb over position!");
            return;
        }

        canGrabLedge = false;
        climbingAllowed = true;
        isClimbing = false;
        transform.position = climbOverPosition;
        rb.gravityScale = 1;

        climbBeginPosition = Vector2.zero;
        climbOverPosition = Vector2.zero;
        ledgePositionSet = false;

        Invoke(nameof(AllowLedgeGrab), 0.5f);
    }

    public void LedgeFallDown()
    {
        isClimbing = false;
        ledgePositionSet = false;
        rb.gravityScale = 1;

        // Apply a small downward force to initiate the fall
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -0.5f);

        // Reset the player's position slightly away from the wall to avoid getting stuck
        transform.position -= new Vector3(facingRight ? 0.3f : -0.3f, 0.3f, 0f);
        climbingAllowed = true;

        // Allow ledge grab after a short delay
        ledgeDetected = false;
            Invoke(nameof(AllowLedgeGrab), 1.5f);
    }

    private void AllowLedgeGrab()
    {
        canGrabLedge = true;

        standingCollider.enabled = true;
        crouchCollider.enabled = false;
    }
    #endregion

    #region BOMB
    public void PlaceBomb()
    {
        if (bombPrefab != null && bombSpawnPoint != null)
        {
            Vector3 bombSpawn = new Vector3(bombSpawnPoint.position.x, (bombSpawnPoint.position.y + 0.25f));

            Instantiate(bombPrefab, bombSpawn, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("BombPrefab or BombSpawnPoint is not set!");
        }
    }
    #endregion

    #region STAGGER
    public void Stagger(Vector2 knockbackDirection)
    {
        if (!isAlive) return; // Don't stagger if the enemy is dead

        // Apply knockback force in the specified direction
        rb.linearVelocity = new Vector2(knockbackDirection.x * knockbackForce, rb.linearVelocity.y);
        PlayerAudio_Play.instance.PlayDamage();
        // Start stagger coroutine to disable movement for a duration
        StartCoroutine(StaggerCoroutine());
    }

    // Coroutine to handle stagger recovery
    private IEnumerator StaggerCoroutine()
    {
        isStaggered = true; // Disable enemy input
        yield return new WaitForSeconds(staggerDuration); // Wait for stagger duration
        isStaggered = false; // Re-enable enemy input
    }
    #endregion

    private IEnumerator Death()
    {
        StopMovement();
        animator.Play("Death");
        animator.SetBool("IsAlive", false);

        yield return new WaitForSeconds(0.75f);
        GameManager.instance.playerUI.GameOver_Panel.SetActive(true);
    }
}