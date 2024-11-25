using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [Header("Level Generation")]
    [SerializeField] public GameObject playerPrefab;
    [SerializeField] public Transform[] startingPositions;
    [SerializeField] public LayerMask room;
    [SerializeField] public GameObject[] RegularRooms;
    // index 0 --> LR, index 1 --> LRB, index 2 --> LRT, index 3 --> LRBT
    [SerializeField] public GameObject[] EntranceRooms;
    // index 4 --> EntranceLR, index 5 --> EntranceLRB, index 6 --> EntranceLRT, index 7 --> EntranceLRBT
    [SerializeField] public GameObject[] ExitRooms;
    // index 8 --> ExitLR, index 9 --> ExitLRB, index 10 --> ExitLRT, index 11 --> ExitLRBT
    //[SerializeField] public GameObject[] enemyRooms;
    [SerializeField] public GameObject[] HeartRooms;

    public float moveAmount;

    private int direction;
    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.25f;

    [SerializeField] public float minX;
    [SerializeField] public float maxX;
    [SerializeField] public float minY;
    public bool stopGeneration;

    private bool heartRoomPlaced = false;
    private bool isLastRoom = false;
    private int downCounter;

    private bool playerSpawned = false;

    private void Start()
    {
        int randStartPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartPos].position;

        // Instantiate a random entrance room as the first room
        int randomEntranceRoom = Random.Range(0, EntranceRooms.Length);
        Instantiate(EntranceRooms[randomEntranceRoom], transform.position, Quaternion.identity);

        direction = Random.Range(1, 6);

        playerPrefab.SetActive(false);
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

        if (stopGeneration && !playerSpawned)
        {
            ActivatePlayer();
            if (!heartRoomPlaced)
            {
                PlaceHeartRoom();
            }
        }
    }

    private void ActivatePlayer()
    {
        // spawn enemy
        GameObject entranceRoom = GameObject.FindGameObjectWithTag("EntranceRoom");

        if (entranceRoom != null)
        {
            // Find the PlayerSpawn game object within the entrance room
            GameObject playerSpawn = entranceRoom.transform.Find("PlayerSpawn").gameObject;

            if (playerSpawn != null)
            {
                // Set the enemy's position to the PlayerSpawn position
                playerPrefab.transform.position = playerSpawn.transform.position;

                // Set the enemy active
                playerPrefab.SetActive(true);
            }
            else
            {
                Debug.LogError("PlayerSpawn not found within the entrance room!");
            }

            playerSpawned = true;
        }
        else
        {
            Debug.LogError("Entrance room not found!");
        }
    }

    private void Move()
    {
        if (!isLastRoom)
        {
            MoveToNextRoom();
        }
        else
        {
            stopGeneration = true;
        }
    }

    private void MoveToNextRoom()
    {
        if (direction == 1 || direction == 2) // Move Right
        {
            if (transform.position.x < maxX) // Check if we can move right
            {
                downCounter = 0;

                Vector2 newPosition = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPosition;

                int random = Random.Range(0, RegularRooms.Length);
                GameObject newRoom = Instantiate(RegularRooms[random], transform.position, Quaternion.identity);

                // Check if the new room is the last room
                if (transform.position.y <= minY)
                {
                    isLastRoom = true;
                    ReplaceWithExitRoom(newRoom);
                }

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

                int random = Random.Range(0, RegularRooms.Length);
                GameObject newRoom = Instantiate(RegularRooms[random], transform.position, Quaternion.identity);

                // Check if the new room is the last room
                if (transform.position.y <= minY)
                {
                    isLastRoom = true;
                    ReplaceWithExitRoom(newRoom);
                }

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

                        Instantiate(RegularRooms[3], transform.position, Quaternion.identity);
                    }
                    else // else if the room is LRBT
                    {
                        roomDetection.GetComponent<RoomType>().RoomDestruction();

                        // Check if the previous room is an entrance room
                        if (roomDetection.CompareTag("EntranceRoom"))
                        {
                            int randomEntranceBottomRoom = Random.Range(0, 2) * 2 + 1; // Randomly select either the second or fourth room
                            Debug.Log($"Random entrance bottom room: {randomEntranceBottomRoom}");
                            Instantiate(EntranceRooms[randomEntranceBottomRoom], transform.position, Quaternion.identity);
                        }
                        else
                        {
                            int randomBottomRoom = Random.Range(1, 3);
                            if (randomBottomRoom == 2)
                            {
                                randomBottomRoom = 1;
                            }
                            Instantiate(RegularRooms[randomBottomRoom], transform.position, Quaternion.identity);
                        }
                    }
                }

                Vector2 newPosition = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPosition;

                int random = Random.Range(2, 4);
                GameObject newRoom = Instantiate(RegularRooms[random], transform.position, Quaternion.identity);

                // Check if the new room is the last room
                if (transform.position.y <= minY)
                {
                    isLastRoom = true;
                    ReplaceWithExitRoom(newRoom);
                }

                direction = Random.Range(1, 6);
            }
        }
    }

    private void ReplaceWithExitRoom(GameObject room)
    {
        Destroy(room);

        RoomType roomType = room.GetComponent<RoomType>();
        Instantiate(ExitRooms[roomType.type], transform.position, Quaternion.identity);
    }

    private void PlaceHeartRoom()
    {
        GameObject[] regularRoomsToReplace = GameObject.FindGameObjectsWithTag("Room");

        if (regularRoomsToReplace.Length > 0)
        {
            int randomIndex = Random.Range(0, regularRoomsToReplace.Length);
            GameObject roomToReplace = regularRoomsToReplace[randomIndex];

            // Check if the room has the RoomType component
            RoomType roomType = roomToReplace.GetComponent<RoomType>();
            if (roomType != null)
            {
                Vector3 roomPosition = roomToReplace.transform.position;
                Quaternion roomRotation = roomToReplace.transform.rotation;

                int typeIndex = roomType.type;

                // Validate the type index
                if (typeIndex >= 0 && typeIndex < HeartRooms.Length)
                {
                    // Destroy the current room and replace it
                    Destroy(roomToReplace);
                    Instantiate(HeartRooms[typeIndex], roomPosition, roomRotation);
                    heartRoomPlaced = true;
                }
                else
                {
                    Debug.LogError($"Room type index {typeIndex} is out of bounds for HeartRooms array.");
                }
            }
            else
            {
                Debug.LogWarning("Selected room does not have a RoomType component!");
            }
        }
        else
        {
            Debug.LogWarning("No regular rooms found to replace with a heart room.");
        }
    }
}