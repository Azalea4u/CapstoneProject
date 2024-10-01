using UnityEngine;
using System.Collections;

public class TileEdgeDetection : MonoBehaviour
{
    [System.Serializable]
    public struct EdgeTileSet
    {
        public GameObject[] leftEdge;
        public GameObject[] rightEdge;
        public GameObject[] topEdge;
        public GameObject[] bottomEdge;
    }

    public EdgeTileSet edgeTiles;
    public GameObject topLeftCorner;
    public GameObject topRightCorner;
    public GameObject bottomLeftCorner;
    public GameObject bottomRightCorner;
    public string tileTag = "LevelTile";

    private void Awake()
    {
        Debug.Log("TileEdgeDetection: Awake called");
    }

    private void Start()
    {
        Debug.Log("TileEdgeDetection: Start called");
    }

    public void ProcessLevelTiles()
    {
        Debug.Log("TileEdgeDetection: ProcessLevelTiles called");
        StartCoroutine(DetectAndReplaceEdgesCoroutine());
    }

    private IEnumerator DetectAndReplaceEdgesCoroutine()
    {
        Debug.Log("Starting edge detection...");
        yield return new WaitForSeconds(0.1f); // Short delay to ensure all tiles are placed

        int processedTiles = 0;
        int replacedTiles = 0;

        foreach (Transform child in transform)
        {
            if (child.CompareTag(tileTag))
            {
                processedTiles++;
                Vector2 pos = child.position;
                bool left = CheckForTile(pos + Vector2.left);
                bool right = CheckForTile(pos + Vector2.right);
                bool up = CheckForTile(pos + Vector2.up);
                bool down = CheckForTile(pos + Vector2.down);

                GameObject newTile = null;

                // Check for corner tiles
                if (!left && !up) newTile = topLeftCorner;
                else if (!right && !up) newTile = topRightCorner;
                else if (!left && !down) newTile = bottomLeftCorner;
                else if (!right && !down) newTile = bottomRightCorner;

                // Check for edge tiles
                else if (!left) newTile = GetRandomTile(edgeTiles.leftEdge);
                else if (!right) newTile = GetRandomTile(edgeTiles.rightEdge);
                else if (!up) newTile = GetRandomTile(edgeTiles.topEdge);
                else if (!down) newTile = GetRandomTile(edgeTiles.bottomEdge);

                // Replace the tile if it's an edge or corner
                if (newTile != null)
                {
                    GameObject instance = Instantiate(newTile, child.position, Quaternion.identity);
                    instance.transform.SetParent(transform);
                    Destroy(child.gameObject);
                    replacedTiles++;
                }
            }
        }

        Debug.Log($"Edge detection complete. Processed {processedTiles} tiles, replaced {replacedTiles} tiles.");
    }

    private bool CheckForTile(Vector2 position)
    {
        Collider2D hit = Physics2D.OverlapCircle(position, 0.1f);
        return hit != null && hit.CompareTag(tileTag);
    }

    private GameObject GetRandomTile(GameObject[] tileSet)
    {
        if (tileSet != null && tileSet.Length > 0)
        {
            int randomIndex = Random.Range(0, tileSet.Length);
            return tileSet[randomIndex];
        }
        return null;
    }
}