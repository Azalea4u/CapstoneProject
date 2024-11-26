using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_Pickup : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] public int gold = 5;
    [SerializeField] private Int_SO playerGold;
    [SerializeField] private AudioSource collectCoin;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerGold.value += gold;
            //Player_UI.instance.GoldText.text = playerGold.value.ToString();
            Debug.Log("Player Gold: " + playerGold.value);
            //health.Heal(gold);
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
