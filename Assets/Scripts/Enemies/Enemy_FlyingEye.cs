using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_FlyingEye : EnemyBase, IAttackable
{
    [Header("FlyingEye Specific")]
    [SerializeField] DetectionZone attackZone;
    [SerializeField] private float attackCooldown = 2.5f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private Collider2D attackPoint;
    [SerializeField] private LayerMask playerLayer;

    public bool canAttack = true;
    private Transform player;
    private bool playerFound = false;

    // IAttackable implementation
    public int AttackDamage => attackDamage;
    public float AttackCooldown => attackCooldown;
    public float AttackRange => attackRange;
    public Collider2D AttackPoint => attackPoint;
    public bool CanAttack { get => canAttack; set => canAttack = value; }

    [Header("Collider")]
    [SerializeField] private CapsuleCollider2D aliveCollider;
    [SerializeField] private BoxCollider2D deadCollider;

    [Header("Waypoints")]
    [SerializeField] List<Transform> waypoints = new List<Transform>();
    private Transform nextWaypoint;
    public int waypointNum = 0;
    public float waypointReachedDistance = 0.1f;

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
            //Debug.Log("Player found: " + player);
            playerFound = player != null;
        }

        base.Update();

        hasTarget = attackZone.DetectedColliders.Count > 0;
        //Debug.Log("Has target: " + hasTarget);
        //Debug.Log("Number of Colliders detected" + attackZone.DetectedColliders.Count);

        CheckPlayerInRange();
        if (hasTarget && canAttack && !playerHit)
        {
            Attack();
        }
    }

    protected override void FixedUpdate()
    {
        //base.FixedUpdate();

        if (isAlive)
        {
            if (canMove)
            {
                Flight();
                canFlip = true;
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
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

        rb.velocity = directionToWaypoint * movementSpeed;
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
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
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

    #region DEATH
    public void HandleDeath()
    {
        aliveCollider.enabled = false;
        deadCollider.enabled = true;
        rb.gravityScale = 2.0f;
        StopMovement();
        // Add any additional death logic, like dropping items
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        // Wait for the animation to finish
        yield return new WaitForSeconds(3.0f); // Adjust time based on your death animation length

        // Optionally, drop loot here
        DropLoot();

        // Destroy the goblin object
        Destroy(gameObject);
    }
    #endregion

    private void DropLoot()
    {
        Debug.Log("Dropped 20 gold");
        GameManager.instance.Gold += 20;
        // Implement loot dropping logic here
        // For example:
        // Instantiate(goldCoinPrefab, transform.position, Quaternion.identity);
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
