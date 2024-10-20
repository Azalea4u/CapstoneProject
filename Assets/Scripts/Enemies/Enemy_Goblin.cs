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

    public bool canAttack = true;
    private Transform player;
    private bool playerFound = false;
    private bool playerHit = false;

    // IAttackable implementation
    public int AttackDamage => attackDamage;
    public float AttackCooldown => attackCooldown;
    public float AttackRange => attackRange;
    public Collider2D AttackPoint => attackPoint;
    public bool CanAttack { get => canAttack; set => canAttack = value; }

    protected override void Start()
    {
        base.Start();
        //player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Update()
    {
        if (!playerFound)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            Debug.Log("Player found: " + player);
            playerFound = player != null;
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

    public void Attack()
    {
        animator.SetTrigger("Attack");
        canAttack = false;

    }

    private IEnumerator WaitForAttackCooldown()
    {
        yield return new WaitForSeconds(AttackCooldown);
        canAttack = true;
        playerHit = false;
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
        yield return new WaitForSeconds(6.0f); // Adjust time based on your death animation length

        // Optionally, drop loot here
        DropLoot();

        // Destroy the goblin object
        Destroy(gameObject);
    }

    private void DropLoot()
    {
        Debug.Log("Dropped 10 gold");
        // Implement loot dropping logic here
        // For example:
        // Instantiate(goldCoinPrefab, transform.position, Quaternion.identity);
    }


    public override void Stagger(Vector2 knockbackDirection)
    {
        base.Stagger(knockbackDirection);
        animator.SetTrigger("TakeDamage");
    }

    public void OnAttackPointTriggered(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (collision.CompareTag("Player") && !playerHit)
        {
            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage);
                playerHit = true;
            }

            if (playerHit)
            {
                StartCoroutine(WaitForAttackCooldown());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && attackPoint.IsTouching(collision))
        {
            OnAttackPointTriggered(collision);
        }
    }
}
