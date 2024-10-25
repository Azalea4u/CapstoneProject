using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_Pickup : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int gold = 5;
    [SerializeField] private Int_SO playerGold;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerGold.value += gold;
            Debug.Log("Player Gold: " + playerGold.value);
            //health.Heal(gold);
            Destroy(gameObject);
        }
    }
}
