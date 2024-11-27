using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float explosionDelay = 2.0f;
    [SerializeField] private Vector2Int bombRange = new Vector2Int(2, 2);
    [SerializeField] private int playerDamageAmount = 1;
    [SerializeField] private int enemyDamageAmount = 3;

    private void Start()
    {
        // Automatically trigger explosion after a delay
        //Invoke(nameof(TriggerExplosion), explosionDelay);
    }

    public void TriggerExplosion()
    {
        // Find all colliders within the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, Mathf.Max(bombRange.x, bombRange.y));

        foreach (Collider2D collider in colliders)
        {
            // Check if the collider belongs to a player
            if (collider.CompareTag("Player"))
            {
                IDamageable damageable = collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(playerDamageAmount);
                }
            }
            // Check if the collider belongs to an enemy
            else if (collider.CompareTag("Enemy"))
            {
                IDamageable damageable = collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(enemyDamageAmount);
                }
            }
        }

        // Optionally destroy the bomb object itself
        Destroy(gameObject);
    }
}
