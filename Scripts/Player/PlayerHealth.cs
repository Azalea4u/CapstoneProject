using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public PlayerMovement player;
    [SerializeField] public Animator animator;
    [SerializeField] public TMPro.TextMeshProUGUI healthText;
    public int currentHealth = 3;

    // Getters and Setters
    public int CurrentHealth
    {
        // max health to 99
        get => currentHealth;
        set => currentHealth = Mathf.Clamp(value, 0, 99);
    }

    public void Update()
    {
        healthText.text = currentHealth.ToString();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth > 0)
        {
            player.Stagger(new Vector2(-1,0));
            animator.SetTrigger("TakeDamage");
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            player.isAlive = false;
        }
    }
}
