using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Damageable : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;

    [SerializeField] protected int _maxHealth = 3;
    [SerializeField] protected int _health = 3;
    [SerializeField] protected bool _isAlive = true;
    [SerializeField] private bool isInvincible = false;
    [SerializeField] private float invincibilityTime = 0.25f;
    private float timeSinceHit = 0.0f;

    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = Mathf.Clamp(value, 0, 99);

            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
        }
    }

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected void Update()
    {
        if (isInvincible && IsAlive)
        {
            if (timeSinceHit > invincibilityTime)
            {
                // Reset invincibility
                isInvincible = false;
                timeSinceHit = 0.0f;
            }

            timeSinceHit += Time.deltaTime;
        }

        if (!IsAlive)
        {
            Death();
        }
    }

    public void Hit(int damage)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;
        }
    }

    protected void Death()
    {
        animator.SetBool("IsAlive", _isAlive);
        rb.velocity = Vector2.zero;

    }
}
