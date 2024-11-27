using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnObject : MonoBehaviour
{
    public GameObject[] objects;

    void Start()
    {
        int random = Random.Range(0, objects.Length);
        GameObject instance = (GameObject)Instantiate(objects[random], transform.position, Quaternion.identity);
        instance.transform.parent = transform;

        foreach (GameObject obj in objects)
        {
            TilemapCollider2D tilemapCollider = obj.GetComponent<TilemapCollider2D>();
            if (tilemapCollider != null)
            {
                tilemapCollider.enabled = false; // Disable and re-enable to force refresh
                tilemapCollider.enabled = true;
            }

            Physics2D.SyncTransforms(); // Sync the physics system

            //Debug.Log($"Tilemap Collider Enabled: {tilemapCollider.enabled}");
            //Debug.Log($"Tilemap Rigidbody: {obj.GetComponent<Rigidbody2D>()}");
        }
    }
}
