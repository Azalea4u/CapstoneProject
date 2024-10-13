using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Goblin : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float facingDirection;
    [SerializeField] private CircleCollider2D FrontCollider;
    [SerializeField] private CircleCollider2D CliffCollider;

    private bool canFlip = true;
    private bool facingRight = true;


    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        facingDirection = rb.velocity.x;

        CheckForFlip();

        animator.SetBool("IsMoving", facingDirection != 0);
    }

    private void FixedUpdate()
    {
        if (facingRight)
        {
            //MoveRight();
            rb.velocity = new Vector2(walkSpeed * Vector2.right.x, rb.velocity.y);

        }
        else
        {
            //MoveLeft();
            rb.velocity = new Vector2(walkSpeed * Vector2.left.x, rb.velocity.y);

        }
    }

    private void CheckForFlip()
    {
        // if the FrontCollider is touching something, flip the direction
        if (FrontCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            FlipDirection();
        }

        // check if the CliffCollider is not touching anything, flip the direction
        if (!CliffCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            FlipDirection();
        }
    }

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

    private void FlipDirection()
    {
        if (!canFlip) return;

        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }
}
