using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Collectable : MonoBehaviour
{
    public AudioSource collect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player)
        {
            Item item = GetComponent<Item>();

            if (item != null)
            {
                if (player.inventoryManager.GetInventoryByName("Hotbar").IsFull())
                {
                    //player.inventoryManager.Add("Inventory", item);
                    Debug.Log("Your hotbar is full!");
                }
                else
                {
                    player.inventoryManager.Add("Hotbar", item);
                }
                collect.Play();
                Destroy(this.gameObject);
            }
        }
    }
}