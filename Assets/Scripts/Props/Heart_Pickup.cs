using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart_Pickup : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int heal = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.Heal(heal);
                Destroy(gameObject);
            }
        }
    }
}
