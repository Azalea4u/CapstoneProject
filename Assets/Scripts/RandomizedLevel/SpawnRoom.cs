using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoom : MonoBehaviour
{
    public LayerMask whatisRoom;
    public LevelGeneration levelGen;

    void Start()
    {
        // Calculate the mask for all layers except SpawnRooms
        int spawnRoomsLayer = LayerMask.NameToLayer("SpawnRooms");
        whatisRoom = ~(1 << spawnRoomsLayer);
    }

    void Update()
    {
        Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, whatisRoom);

        if (roomDetection == null && levelGen.stopGeneration)
        {
            int rand = Random.Range(0, levelGen.RegularRooms.Length);
            Instantiate(levelGen.RegularRooms[rand], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
