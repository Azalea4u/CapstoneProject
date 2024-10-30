using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store_UI : MonoBehaviour
{
    [SerializeField] public string inventoryName;
    [SerializeField] private Canvas canvas;
    [SerializeField] GameObject inventoryPanel;

    private Inventory inventory;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    private void Start()
    {
        inventory = GameManager.instance.player.inventoryManager.GetInventoryByName(inventoryName);
        inventoryPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        // check if the inventory is active
        if (inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(false);
            // Exit the store when closing the inventory
            GameManager.instance.ExitStore();
        }
        else
        {
            inventoryPanel.SetActive(true);
            // Enter the store when opening the inventory
            GameManager.instance.EnterStore();
        }
    }
}
