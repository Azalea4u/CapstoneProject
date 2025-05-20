using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Int_SO _currentHealthSO;

    [Header("Damagable")]
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private int _health = 3;
    [SerializeField] private bool _isAlive = true;
    [SerializeField] public bool isInvincible = false;
    [SerializeField] private float invincibilityTime = 0.25f;
    private float timeSinceHit = 0.0f;

    public int MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }

    public int Health
    {
        get => _health;
        set
        {
            _health = Mathf.Clamp(value, 0, 99);
            _currentHealthSO.value = _health;
            if (_health <= 0)
            {
                IsAlive = false;
            }
            UpdateHealthDisplay();
        }
    }

    public bool IsAlive
    {
        get => _isAlive;
        set
        {
            _isAlive = value;
            player.isAlive = value;
        }
    }

    private void Awake()
    {
        Health = _currentHealthSO.value;
    }

    private void Update()
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

        if (!IsAlive)
        {
            Death();
        }
        UpdateHealthDisplay();
    }

    public bool TakeDamage(int damage)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            if (IsAlive)
            {
                player.Stagger(new Vector2(-1, 0));
                animator.SetTrigger("TakeDamage");
            }
            return true;
        }

        // Fail to Hit
        return false;
    }

    public void Death()
    {
        animator.SetBool("IsAlive", IsAlive);
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

    public void Heal(int amount)
    {
        Health += amount;
    }

    private void UpdateHealthDisplay()
    {
        healthText.text = _currentHealthSO.value.ToString();
    }
}