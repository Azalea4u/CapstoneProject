using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField] public Transform[] startingPositions;
    [SerializeField] public LayerMask room;
    [SerializeField] public GameObject[] rooms; // index 0 --> LR, index 1 --> LRB, index 2 --> LRT, index 3 --> LRBT

    public float moveAmount;

    private int direction;
    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.25f;

    [SerializeField] public float minX;
    [SerializeField] public float maxX;
    [SerializeField] public float minY;
    public bool stopGeneration;

    private int downCounter;

    [Header("Tiles")]
    public EdgeTileSet edgeTiles;
    public GameObject topLeftCorner;
    public GameObject topRightCorner;
    public GameObject bottomLeftCorner;
    public GameObject bottomRightCorner;

    [System.Serializable]
    public struct EdgeTileSet
    {
        public GameObject[] leftEdge;
        public GameObject[] rightEdge;
        public GameObject[] topEdge;
        public GameObject[] bottomEdge;
    }

    [SerializeField] LayerMask tileLayer;
    public string tileTag = "LevelTile";

    private void Start()
    {
        int randStartPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartPos].position;
        Instantiate(rooms[0], transform.position, Quaternion.identity);

        direction = Random.Range(1, 6);
    }

    private void Update()
    {
        if (timeBtwRoom <= 0 && !stopGeneration)
        {
            Move();
            timeBtwRoom = startTimeBtwRoom;
        }
        else
        {
            timeBtwRoom -= Time.deltaTime;
        }

        if (stopGeneration && !IsInvoking("DetectAndReplaceEdges"))
        {
            Invoke("DetectAndReplaceEdges", 0.5f);
        }
    }

    private void Move()
    {
        if (direction == 1 || direction == 2) // Move Right
        {
            if (transform.position.x < maxX) // Check if we can move right
            {
                downCounter = 0;

                Vector2 newPosition = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPosition;

                int random = Random.Range(0, rooms.Length);
                Instantiate(rooms[random], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6);
                if (direction == 3)
                {
                    direction = 2;
                }
                else if (direction == 4)
                {
                    direction = 5;
                }
            }
            else
            {
                direction = 5;
            }
        }
        else if (direction == 3 || direction == 4) // Move Left
        {
            if (transform.position.x > minX) // Check if we can move left
            {
                downCounter = 0;

                Vector2 newPosition = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPosition;

                int random = Random.Range(0, rooms.Length);
                Instantiate(rooms[random], transform.position, Quaternion.identity);

                direction = Random.Range(3, 6);
            }
            else
            {
                direction = 5;
            }
        }
        else if (direction == 5) // Move Down
        {
            downCounter++;

            if (transform.position.y > minY) // Check if we can move down
            {
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);

                if (roomDetection.GetComponent<RoomType>().type != 1 || roomDetection.GetComponent<RoomType>().type != 3) // If the room is LR or LRT
                {
                    if (downCounter >= 2)
                    {
                        roomDetection.GetComponent<RoomType>().RoomDestruction();

                        Instantiate(rooms[3], transform.position, Quaternion.identity);
                    }
                    else
                    {
                        roomDetection.GetComponent<RoomType>().RoomDestruction();

                        int randomBottomRoom = Random.Range(1, 3);
                        if (randomBottomRoom == 2)
                        {
                            randomBottomRoom = 1;
                        }
                        Instantiate(rooms[randomBottomRoom], transform.position, Quaternion.identity);
                    }
                }

                Vector2 newPosition = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPosition;

                int random = Random.Range(2, 4);
                Instantiate(rooms[random], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6);
            }
            else
            {
                stopGeneration = true;
            }
        }
    }

    private void DetectAndReplaceEdges()
    {
        Debug.Log("Starting edge detection...");
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