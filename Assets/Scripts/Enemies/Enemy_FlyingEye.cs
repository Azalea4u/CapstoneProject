using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_FlyingEye : EnemyBase, IAttackable
{
    [Header("FlyingEye Specific")]
    [SerializeField] DetectionZone attackZone;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private Collider2D attackPoint;
    [SerializeField] private Collider2D RangeToAttack;
    [SerializeField] private LayerMask playerLayer;

    public bool canAttack = true;
    private Transform player;
    private bool playerFound = false;

    [Header("Life Collider")]
    [SerializeField] private CapsuleCollider2D aliveCollider;
    [SerializeField] private BoxCollider2D deadCollider;

    [Header("Waypoints")]
    [SerializeField] List<Transform> waypoints = new List<Transform>();
    private Transform nextWaypoint;
    public int waypointNum = 0;
    public float waypointReachedDistance = 0.1f;

    #region IAttackable
    // IAttackable implementation
    public int AttackDamage => attackDamage;
    public float AttackCooldown => attackCooldown;
    public float AttackRange => attackRange;
    public Collider2D AttackPoint => attackPoint;
    public bool CanAttack { get => canAttack; set => canAttack = value; }
    #endregion

    protected override void Start()
    {
        base.Start();
        movementSpeed = 2.0f;
        nextWaypoint = waypoints[waypointNum];

        aliveCollider.enabled = true;
        deadCollider.enabled = false;
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
            playerFound = player != null;
        }

        base.Update();

        hasTarget = attackZone.DetectedColliders.Count > 0;
        CheckPlayerInRange();
    }

    protected override void FixedUpdate()
    {
        if (isAlive)
        {
            if (canMove && !GameManager.instance.isGamePaused)
            {
                canFlip = true;
                if (!hasTarget)
                    Flight();
                else if (hasTarget && canAttack)
                    FlyTowardsPlayer();

            }
            else
            {
                rb.linearVelocity = new Vector2(0, 0);
                canFlip = false;
            }
        }
    }

    private void Flight()
    {
        // fly to next waypoint
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        // check if we have reached the waypoint
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.linearVelocity = directionToWaypoint * movementSpeed;
        UpdateDirection();

        // see if we need to switch waypoints
        if (distance <= waypointReachedDistance)
        {
            // switch to random waypoint
            waypointNum++;

            if (waypointNum >= waypoints.Count)
            {
                waypointNum = 0;
            }

            nextWaypoint = waypoints[waypointNum];
        }
    }

    private void UpdateDirection()
    {
        // if going right
        if (rb.linearVelocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (rb.linearVelocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void CheckPlayerInRange()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            //hasTarget = distanceToPlayer <= attackRange;

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

    private void FlyTowardsPlayer()
    {
        if (player == null) return; // Ensure the player exists

        // Calculate direction to player
        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        // Move toward the player
        rb.linearVelocity = directionToPlayer * movementSpeed;

        // Flip the sprite to face the player
        UpdateDirection();

        if (RangeToAttack.GetComponent<DetectionZone>().DetectedColliders.Count > 0)
        {
            Attack();
        }
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
        // move back a bit before going to attack
        if (player != null)
        {
            // Allow some time for the attack animation to finish
            yield return new WaitForSeconds(0.5f);
            // move away from player
            Vector2 directionAwayFromPlayer = ((Vector2)(transform.position - player.position).normalized + Vector2.up).normalized;
            rb.linearVelocity = directionAwayFromPlayer * movementSpeed;
        }

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

    #region DEATH
    public void HandleDeath()
    {
        StopMovement();
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        aliveCollider.enabled = false;
        deadCollider.enabled = true;
        rb.gravityScale = 2.0f;

        // Optionally, drop loot here
        if (!base.lootDropped)
            base.DropLoot();

        // Wait for the animation to finish
        // Adjust time based on your death animation length
        yield return new WaitForSeconds(3.0f);

        // Destroy the goblin object
        Destroy(gameObject);
    }
    #endregion

    public override void Stagger(Vector2 knockbackDirection)
    {
        base.Stagger(knockbackDirection);
        animator.SetTrigger("TakeDamage");
    }

}
