using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DualGridTilemap : MonoBehaviour
{
    protected static Vector3Int[] NEIGHBOURS = new Vector3Int[] {
        new Vector3Int(0, 1, 0),   // Above
        new Vector3Int(1, 0, 0),   // Right
        new Vector3Int(0, -1, 0),  // Below
        new Vector3Int(-1, 0, 0)   // Left
    };

    protected static Dictionary<Tuple<TileType, TileType, TileType, TileType>, Tile> neighbourTupleToTile;

    // Provide references to each tilemap in the inspector
    public Tilemap placeholderTilemap;
    public Tilemap displayTilemap;

    // Provide the dirt and air placeholder tiles in the inspector
    public Tile airPlaceholderTile;
    public Tile dirtPlaceholderTile;

    // Provide the 16 edge tiles in the inspector
    public Tile[] edgeTiles;

    void Start()
    {
        // This dictionary stores the "rules" for displaying edge tiles
        // |_1_|_2_|
        // |_3_|_4_|
        neighbourTupleToTile = new() {
            // BOTTOM
            {new (TileType.Air, TileType.Dirt, TileType.Dirt, TileType.Dirt), edgeTiles[0]},    // Bottom Variant 1
            {new (TileType.Dirt, TileType.Air, TileType.Dirt, TileType.Dirt), edgeTiles[1]},    // Bottom Variant 2
            {new (TileType.Dirt, TileType.Dirt, TileType.Air, TileType.Dirt), edgeTiles[2]},    // Bottom Variant 3
            {new (TileType.Dirt, TileType.Dirt, TileType.Dirt, TileType.Air), edgeTiles[3]},    // Bottom Variant 4
            // BOTTOM LEFT CORNER
            {new (TileType.Air, TileType.Air, TileType.Dirt, TileType.Dirt), edgeTiles[4]},     // Bottom Left Corner
            {new (TileType.Air, TileType.Dirt, TileType.Dirt, TileType.Air), edgeTiles[5]},     // Bottom Left Corner Under Dirt || When there's an edge to the left and bottom
            {new (TileType.Dirt, TileType.Air, TileType.Air, TileType.Dirt), edgeTiles[6]},     // Bottom Left Slant
            // BOTTOM RIGHT CORNER
            {new (TileType.Dirt, TileType.Dirt, TileType.Air, TileType.Air), edgeTiles[7]},     // Bottom Right Corner
            {new (TileType.Air, TileType.Air, TileType.Air, TileType.Air), edgeTiles[8]},       // Bottom Right Corner Under Dirt || When there's an edge to the right and bottom
            {new (TileType.Air, TileType.Dirt, TileType.Air, TileType.Dirt), edgeTiles[9]},     // Bottom Right Slant
            // LEFT
            {new (TileType.Dirt, TileType.Air, TileType.Dirt, TileType.Air), edgeTiles[10]},    // Left Variant 1
            {new (TileType.Air, TileType.Dirt, TileType.Air, TileType.Air), edgeTiles[11]},     // Left Variant 2
            {new (TileType.Dirt, TileType.Air, TileType.Air, TileType.Air), edgeTiles[12]},     // Left Variant 3
            {new (TileType.Air, TileType.Air, TileType.Dirt, TileType.Air), edgeTiles[13]},     // Left Variant 4
            // RIGHT
            {new (TileType.Dirt, TileType.Dirt, TileType.Dirt, TileType.Dirt), edgeTiles[14]},  // Right Variant 1
            {new (TileType.Air, TileType.Air, TileType.Air, TileType.Dirt), edgeTiles[15]},      // Right Variant 2
            {new (TileType.Dirt, TileType.Air, TileType.Dirt, TileType.Dirt), edgeTiles[16]},   // Right Variant 3
            {new (TileType.Air, TileType.Dirt, TileType.Dirt, TileType.Dirt), edgeTiles[17]},   // Right Variant 4
            // TOP
            {new (TileType.Air, TileType.Dirt, TileType.Dirt, TileType.Air), edgeTiles[18]},    // Top Variant 1
            {new (TileType.Dirt, TileType.Air, TileType.Air, TileType.Air), edgeTiles[19]},     // Top Variant 2
            {new (TileType.Air, TileType.Air, TileType.Air, TileType.Dirt), edgeTiles[20]},     // Top Variant 3
            {new (TileType.Dirt, TileType.Dirt, TileType.Air, TileType.Air), edgeTiles[21]},    // Top Variant 4
            // TOP LEFT CORNER
            {new (TileType.Air, TileType.Air, TileType.Dirt, TileType.Air), edgeTiles[22]},     // Top Left Corner
            {new (TileType.Air, TileType.Dirt, TileType.Air, TileType.Dirt), edgeTiles[23]},    // Top Left Corner Under Dirt || When there's an edge to the left and top
            {new (TileType.Dirt, TileType.Air, TileType.Dirt, TileType.Dirt), edgeTiles[24]},   // Top Left Slant
            // TOP RIGHT CORNER
            {new (TileType.Dirt, TileType.Dirt, TileType.Air, TileType.Dirt), edgeTiles[25]},   // Top Right Corner
            {new (TileType.Air, TileType.Air, TileType.Air, TileType.Dirt), edgeTiles[26]},     // Top Right Corner Under Dirt || When there's an edge to the right and top
            {new (TileType.Air, TileType.Dirt, TileType.Air, TileType.Air), edgeTiles[27]},     // Top Right Slant
        };
        RefreshDisplayTilemap();
    }

    public void SetCell(Vector3Int coords, Tile tile)
    {
        placeholderTilemap.SetTile(coords, tile);
        SetDisplayTile(coords);
    }

    private TileType GetPlaceholderTileTypeAt(Vector3Int coords)
    {
        if (placeholderTilemap.GetTile(coords) == dirtPlaceholderTile)
            return TileType.Dirt;
        else
            return TileType.Air;  // Treat everything else as air
    }

    protected Tile CalculateDisplayTile(Vector3Int coords)
    {
        // Check the neighbors for edges
        TileType top = GetPlaceholderTileTypeAt(coords + NEIGHBOURS[0]);
        TileType right = GetPlaceholderTileTypeAt(coords + NEIGHBOURS[1]);
        TileType bottom = GetPlaceholderTileTypeAt(coords + NEIGHBOURS[2]);
        TileType left = GetPlaceholderTileTypeAt(coords + NEIGHBOURS[3]);

        Tuple<TileType, TileType, TileType, TileType> neighbourTuple = new(top, right, bottom, left);

        // Return the corresponding edge tile based on neighbors
        return neighbourTupleToTile.ContainsKey(neighbourTuple) ? neighbourTupleToTile[neighbourTuple] : null;
    }

    public void SetDisplayTile(Vector3Int pos)
    {
        displayTilemap.SetTile(pos, CalculateDisplayTile(pos));
    }

    // Refresh the display tilemap based on the placeholder tilemap
    public void RefreshDisplayTilemap()
    {
        for (int i = -50; i < 50; i++)
        {
            for (int j = -50; j < 50; j++)
            {
                SetDisplayTile(new Vector3Int(i, j, 0));
            }
        }
    }
}

public enum TileType
{
    None,
    Air,  // Replacing Grass with Air
    Dirt
}
