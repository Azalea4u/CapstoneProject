using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyBase enemy;
    [SerializeField] private Animator animator;
    [SerializeField] private int maxHealth = 3;

    private int _health;
    private bool _isAlive = true;
    private bool isInvincible = false;
    private float invincibilityTime = 0.25f;
    private float timeSinceHit = 0.0f;

    public int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public int Health
    {
        get => _health;
        set
        {
            _health = Mathf.Clamp(value, 0, MaxHealth);
            if (_health == 0)
            {
                IsAlive = false;
            }
        }
    }

    public bool IsAlive
    {
        get => _isAlive;
        set
        {
            _isAlive = value;
            enemy.isAlive = value;
            animator.SetBool("IsAlive", value);

            if (!value)
            {
                Death();
            }
                enemy.enabled = value; // Disable EnemyBase component when not alive
        }
    }

    private void Awake()
    {
        Health = MaxHealth;
    }

    private void Update()
    {
        UpdateInvincibility();
        enemy.isAlive = IsAlive;
    }

    public bool TakeDamage(int amount)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= amount;
            isInvincible = true;

            if (IsAlive)
            {
                enemy.Stagger(new Vector2(-1, 0));
                animator.SetTrigger("TakeDamage");
            }
            else
            {
                IsAlive = false;
                Death();
            }
            return true;
        }

        return false;
    }

    public void Death()
    {
        if (enemy is Enemy_FlyingEye flyingEye)
        {
            flyingEye.HandleDeath();
        }
        animator.SetBool("IsAlive", false);
        enemy.enabled = false; // Disable the enemy behavior
    }

    private void UpdateInvincibility()
    {
        if (isInvincible && IsAlive)
        {
            if (timeSinceHit > invincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0.0f;
            }
            timeSinceHit += Time.deltaTime;
        }
    }
}
