using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float explosionDelay = 2.0f;
    [SerializeField] private Vector2Int bombRange = new Vector2Int(2, 2);

    private void Start()
    {
        // Automatically trigger explosion after a delay
        //Invoke(nameof(TriggerExplosion), explosionDelay);
    }

    public void TriggerExplosion()
    {
        // Find all Tilemap instances in the scene
        Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();

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
}
