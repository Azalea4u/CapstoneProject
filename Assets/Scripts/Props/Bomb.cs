using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float explosionDelay = 0.5f;
    [SerializeField] private Vector2Int bombRange = new Vector2Int(2, 2);
    [SerializeField] private int playerDamageAmount = 1;
    [SerializeField] private int enemyDamageAmount = 3;

    [Header("SFX")]
    [SerializeField] private AudioSource explosionSFX;
    [SerializeField] private AudioSource tickSFX;

    public void Explode()
    {
        Invoke(nameof(TriggerExplosion), explosionDelay);
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

        // Find all Tilemap instances in the scene
        Tilemap[] tilemaps = FindObjectsOfType<Tilemap>(); // Corrected the method to return an array of Tilemap objects

        foreach (Tilemap tilemap in tilemaps)
        {
            // Check if the Tilemap is tagged as destructible (or any other identifying method you use)
            if (tilemap.CompareTag("Destructible"))
            {
                // Convert bomb position from world coordinates to Tilemap coordinates
                Vector3Int bombCell = tilemap.WorldToCell(transform.position);

                // Iterate over the range around the bomb
                for (int x = -bombRange.x; x <= bombRange.x; x++)
                {
                    for (int y = -bombRange.y; y <= bombRange.y; y++)
                    {
                        Vector3Int currentCell = bombCell + new Vector3Int(x, y, 0);

                        // Check if the tile exists at the current cell
                        if (tilemap.HasTile(currentCell))
                        {
                            // Remove the tile
                            tilemap.SetTile(currentCell, null);
                        }
                    }
                }
            }
        }

        // Optionally destroy the bomb object itself
        Destroy(gameObject);
    }

    public void Play_Explosion()
    {
        explosionSFX.Play();
    }

    public void Play_Tick()
    {
        tickSFX.Play();
    }
}
