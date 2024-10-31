using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu_UI : MonoBehaviour
{
    [SerializeField] public string inventoryName;
    [SerializeField] private Canvas canvas;
    [SerializeField] GameObject inventoryPanel;

    [Header("Hotbars")]
    [SerializeField] private GameObject inGame_Hotbar;
    [SerializeField] private GameObject inventory_Hotbar;

    private Inventory inventory;
    public bool isInventoryActive = false;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    private void Start()
    {
        inventory = GameManager.instance.player.inventoryManager.GetInventoryByName(inventoryName);
        inventoryPanel.SetActive(false);
        isInventoryActive = false;
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
        if (!inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(true);
            inGame_Hotbar.SetActive(false);

            // Enter the store when opening the inventory
            GameManager.instance.PauseGame();
            //isInventoryActive = true;
        }
        else if (inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(false);
            inGame_Hotbar.SetActive(true);

            // Exit the store when closing the inventory
            GameManager.instance.ResumeGame();
        }

        isInventoryActive = GameManager.instance.isGamePaused;
        SyncHotbars();

    }

    public void SyncHotbars()
    {
        List<Slot_UI> inGame_slots = inGame_Hotbar.GetComponent<Hotbar_UI>().hotbar_Slots;
        List<Slot_UI> inventory_slots = inventory_Hotbar.GetComponent<Hotbar_UI>().hotbar_Slots;

        // if opening inventory
        if (isInventoryActive)
        {
            // sync inventory hotbar with player hotbar

            for (int i = 0; i < inGame_slots.Count; i++)
            {
                inventory_slots[i].SetItem(inGame_slots[i].inventory.slots[i]);
            }

        }
        else // if closing inventory
        {
            // sync player hotbar with inventory hotbar
            for (int i = 0; i < inventory_Hotbar.transform.childCount; i++)
            {
                inGame_slots[i].SetItem(inventory_slots[i].inventory.slots[i]);
            }

        }
    }
}
