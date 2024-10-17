using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Goblin : EnemyBase
{
    [Header("GoblinSpecific")]
    [SerializeField] DetectionZone attackZone;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 5.0f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerLayer;

    private bool canAttack = true;
    private Transform player;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Update()
    {
        base.Update();

        hasTarget = attackZone.DetectedColliders.Count > 0;

        CheckPlayerInRange();
        if (hasTarget && canAttack)
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

    private void Attack()
    {
        animator.SetTrigger("Attack");
        canAttack = false;
        StartCoroutine(AttackCooldown());

        /* Perform the attack
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, 0.5f, playerLayer);
        foreach (Collider2D player in hitPlayers)
        {
            //player.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage);
        }
        */
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
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
    protected override void HandleDeath()
    {
        base.HandleDeath();
        // Add any additional death logic, like dropping items
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        // Wait for the animation to finish
        yield return new WaitForSeconds(5f); // Adjust time based on your death animation length

        // Optionally, drop loot here
        DropLoot();

        // Destroy the goblin object
        Destroy(gameObject);
    }

    private void DropLoot()
    {
        // Implement loot dropping logic here
        // For example:
        // Instantiate(goldCoinPrefab, transform.position, Quaternion.identity);
    }


    public override void Stagger(Vector2 knockbackDirection)
    {
        base.Stagger(knockbackDirection);
        animator.SetTrigger("TakeHit");
    }
}
