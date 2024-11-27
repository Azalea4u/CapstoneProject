using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleTerrain : MonoBehaviour
{
    [SerializeField] public List<Tilemap> tilemaps; // Assign all Tilemaps here
    [SerializeField] private Vector2Int bombRange = new Vector2Int(2, 2);

    public void Explode(Vector3 bombPosition)
    {
        foreach (Tilemap tilemap in tilemaps)
        {
            DestroyTiles(tilemap, bombPosition);
        }
    }

    private void DestroyTiles(Tilemap tilemap, Vector3 bombPosition)
    {
        // Convert bomb position from world coordinates to Tilemap coordinates
        Vector3Int bombCell = tilemap.WorldToCell(bombPosition);

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
