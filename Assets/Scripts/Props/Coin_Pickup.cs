using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_Pickup : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] public int gold;
    [SerializeField] private GoldData playerGold;
    [SerializeField] private AudioSource collectCoin;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (playerGold.IsDoubleGoldActive)
            {
                gold = gold * 2;
            }

            playerGold.CurrentGold += gold;
            Debug.Log("Player got " + gold.ToString() + " gold!");
            Debug.Log("Player Gold: " + playerGold.CurrentGold);
            StartCoroutine(LootDrop());
        }
    }

    private IEnumerator LootDrop()
    {
        collectCoin.Play();
        yield return new WaitForSeconds(0.4f);

        Destroy(gameObject);
    }

}
