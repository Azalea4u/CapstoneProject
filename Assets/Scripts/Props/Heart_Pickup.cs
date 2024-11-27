using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart_Pickup : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int heal = 1;
    [SerializeField] private AudioSource pickupSFX;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.Heal(heal);
                StartCoroutine(Heal());
            }
        }
    }

    private IEnumerator Heal()
    {
        pickupSFX.Play();

        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }
}
