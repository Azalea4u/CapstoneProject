using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> DetectedColliders = new List<Collider2D>();
    Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if colliders are on the player's layer
        if (collision.gameObject.layer == 3)
        {
            DetectedColliders.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        DetectedColliders.Remove(collision);
    }
}
