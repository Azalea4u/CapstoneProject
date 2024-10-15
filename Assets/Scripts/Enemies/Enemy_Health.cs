using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    [SerializeField] private EnemyBase enemy;
    [SerializeField] private Animator animator;
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth = 3;

    // Getters and Setters
    public int CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = Mathf.Clamp(value, 0, maxHealth);
    }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth > 0)
        {
            enemy.Stagger(new Vector2(-1, 0));
            animator.SetTrigger("TakeDamage");
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            enemy.isAlive = false;
        }
    }
}
