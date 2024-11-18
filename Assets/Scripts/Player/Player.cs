using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public InventoryManager inventoryManager;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerHealth playerHealth;

    [SerializeField] private GameObject inventoryMenu_UI;

    private void Awake()
    {
        inventoryManager = GetComponent<InventoryManager>();
    }

    private void Start()
    {
        inventoryMenu_UI.SetActive(false);
    }

    public void DropItem(Item item)
    {
        Vector2 spawmLocation = transform.position;
        Vector2 spawnOffset = Random.insideUnitCircle * 1.5f;

        Item droppedItem = Instantiate(item, spawmLocation + spawnOffset, Quaternion.identity);

        // Makes the dropped item slide
        droppedItem.rb2D.AddForce(spawnOffset * 0.2f, ForceMode2D.Impulse);
    }

    public void DropItem(Item item, int numToDrop)
    {
        for (int i = 0; i < numToDrop; i++)
        {
            DropItem(item);
        }
    }
}