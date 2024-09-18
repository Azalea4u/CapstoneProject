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
}
