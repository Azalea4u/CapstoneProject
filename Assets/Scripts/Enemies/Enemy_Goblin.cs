using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Goblin : EnemyBase, IAttackable
{
    [Header("GoblinSpecific")]
    [SerializeField] DetectionZone attackZone;
    [SerializeField] private float attackCooldown = 2.5f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private Collider2D attackPoint;
    [SerializeField] private LayerMask playerLayer;

    public bool canAttack = false;
    private Transform player;
    private bool playerFound = false;

    // IAttackable implementation
    public int AttackDamage => attackDamage;
    public float AttackCooldown => attackCooldown;
    public float AttackRange => attackRange;
    public Collider2D AttackPoint => attackPoint;
    public bool CanAttack { get => canAttack; set => canAttack = value; }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (!isAlive)
        {
            HandleDeath();
            return;
        }

        if (!playerFound)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            //Debug.Log("Player found: " + player);
            playerFound = player != null;
            canAttack = playerFound;
        }

        base.Update();

        hasTarget = attackZone.DetectedColliders.Count > 0;

        CheckPlayerInRange();
        if (hasTarget && canAttack && !playerHit)
        {
            Attack();

        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!canAttack)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            canFlip = false;
        }
        else
        {
            canFlip = true;
        }
    }

    private void CheckPlayerInRange()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            hasTarget = distanceToPlayer <= attackRange;

            // Flip towards player if in range
            if (hasTarget)
            {
                bool playerOnRight = player.position.x > transform.position.x;
                if (facingRight != playerOnRight)
                {
                    FlipDirection();
                }
            }
        }
    }

    protected override void Move()
    {
        if (!hasTarget)
        {
            base.Move();
        }
        else
        {
            // Stop moving when in attack range
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    // Goblin-specific death behavior
    public void HandleDeath()
    {
        StopMovement();
        // Add any additional death logic, like dropping items
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        // Optionally, drop loot here
        if (!base.droppedLoot)
            base.DropLoot();

        // Wait for the animation to finish
        yield return new WaitForSeconds(6.0f); // Adjust time based on your death animation length

        // Destroy the goblin object
        Destroy(gameObject);
    }

    public override void Stagger(Vector2 knockbackDirection)
    {
        base.Stagger(knockbackDirection);
        animator.SetTrigger("TakeDamage");
    }

    #region ATTACK
    public void Attack()
    {
        if (!canAttack || playerHit) return;  // Early return if we can't attack

        animator.SetTrigger("Attack");
        canAttack = false;
        StartCoroutine(WaitForAttackCooldown());  // Start cooldown immediately when attack begins
    }

    private IEnumerator WaitForAttackCooldown()
    {
        yield return new WaitForSeconds(AttackCooldown);

        // Reset attack states
        canAttack = true;
        playerHit = false;

        // Debug logs to help track state
        Debug.Log($"Attack cooldown finished. CanAttack: {canAttack}, PlayerHit: {playerHit}");
    }

    public void CheckAttackCollision()
    {
        IDamageable damageable = attackPoint.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(attackDamage);
            playerHit = true;
        }
    }

    public void OnAttackPointTriggered(Collider2D collision)
    {
        // Only process if it's a player and we haven't hit them yet during this attack
        if (!collision.CompareTag("Player") || playerHit) return;

        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null && damageable.IsAlive)
        {
            damageable.TakeDamage(attackDamage);
            playerHit = true;
            Debug.Log("Player hit registered");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && attackPoint.IsTouching(collision))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null && damageable.IsAlive)
            {
                OnAttackPointTriggered(collision);
            }
        }
    }
    #endregion
}
