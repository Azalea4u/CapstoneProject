using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public PlayerMovement player;
    [SerializeField] public Animator animator;
    public int currentHealth = 3;

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
