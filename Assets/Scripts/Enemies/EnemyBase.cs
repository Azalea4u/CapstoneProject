using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class EnemyBase : MonoBehaviour
{
    [Header("Enemy Components")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected float walkSpeed = 3.0f;
    [SerializeField] protected float facingDirection;

    public bool hasTarget;
    public bool isAlive = true;

    [Header("Colliders")]
    [SerializeField] protected CircleCollider2D GroundCollider;
    [SerializeField] protected CircleCollider2D FrontCollider;
    [SerializeField] protected CircleCollider2D CliffCollider;
    [SerializeField] protected BoxCollider2D DetectPlayerCollider;
    [SerializeField] protected LayerMask whatIsGround;

    protected bool isGrounded;

    protected bool canFlip = true;
    protected bool facingRight = true;

    // STAGGER
    [SerializeField] protected float knockbackForce = 10.0f;
    [SerializeField] protected float staggerDuration = 0.5f;
    protected bool isStaggered = false;

    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        //isAlive = damageable.IsAlive;

        if (!isAlive)
        {
            HandleDeath();
            return;
        }

        CheckCollision();

        animator.SetBool("IsMoving", facingDirection != 0);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("HasTarget", hasTarget);
        //animator.SetBool("IsAlive", damageable.IsAlive);
    }

    protected virtual void FixedUpdate()
    {
        UpdateFacingDirection();

        if (!isStaggered && isAlive)
        {
            Move();
        }
    }

    protected virtual void HandleDeath()
    {
        StopMovement();
        DropLoot();
    }

    private void DropLoot()
    {
        Debug.Log("Dropping loot");
        // Implement loot dropping logic here
        // For example:
        // Instantiate(goldCoinPrefab, transform.position, Quaternion.identity);
    }

    protected virtual void UpdateFacingDirection()
    {
        facingDirection = rb.velocity.x;
    }

    protected virtual void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(GroundCollider.bounds.center, Vector2.down, GroundCollider.radius, whatIsGround);

        if (FrontCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))
            || !CliffCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            FlipDirection();
        }
    }

    protected virtual void Move()
    {
        float direction = facingRight ? 1 : -1;
        rb.velocity = new Vector2(walkSpeed * direction, rb.velocity.y);
    }

    protected virtual void StopMovement()
    {
        rb.velocity = Vector2.zero;
        canFlip = false;
    }

    protected virtual void FlipDirection()
    {
        if (!canFlip) return;

        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    public virtual void Stagger(Vector2 knockbackDirection)
    {
        if (!isAlive) return;

        rb.velocity = new Vector2(knockbackDirection.x * knockbackForce, rb.velocity.y);
        StartCoroutine(StaggerCoroutine());
    }

    protected virtual IEnumerator StaggerCoroutine()
    {
        isStaggered = true;
        yield return new WaitForSeconds(staggerDuration);
        isStaggered = false;
    }

}
