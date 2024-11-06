using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu_UI : MonoBehaviour
{
    [SerializeField] public string inventoryName;
    [SerializeField] private Canvas canvas;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] public GameObject storePanel;

    [Header("Hotbars")]
    [SerializeField] private GameObject inGame_Hotbar;
    [SerializeField] private GameObject inventory_Hotbar;
    [SerializeField] private GameObject store_Hotbar;

    private Inventory inventory;
    public bool isInventoryActive = false;
    public bool isStoreActive = false;

    private static InventoryMenu_UI instance;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        instance = this;
    }

    public static InventoryMenu_UI GetInstance()
    {
        return instance;
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

        isStoreActive = storePanel.activeSelf;
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
        SyncHotbars_Inventory();
    }

    public void SyncHotbars_Inventory()
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
        else
        {
            // sync player hotbar with inventory hotbar
            for (int i = 0; i < inventory_Hotbar.transform.childCount; i++)
            {
                inGame_slots[i].SetItem(inventory_slots[i].inventory.slots[i]);
            }

        }
    }

    public void ToggleStore()
    {
        // check if the inventory is active
        if (!storePanel.activeSelf)
        {
            //storePanel.SetActive(true);
            inGame_Hotbar.SetActive(false);

            // Enter the store when opening the inventory
            GameManager.instance.PauseGame();
        }
        else if (storePanel.activeSelf)
        {
            //storePanel.SetActive(false);
            inGame_Hotbar.SetActive(true);

            // Exit the store when closing the inventory
            GameManager.instance.ResumeGame();
        }

        isStoreActive = GameManager.instance.isGamePaused;
        SyncHotbars_Store();
    }

    public void SyncHotbars_Store()
    {
        List<Slot_UI> inGame_slots = inGame_Hotbar.GetComponent<Hotbar_UI>().hotbar_Slots;
        List<Slot_UI> store_slots = store_Hotbar.GetComponent<Hotbar_UI>().hotbar_Slots;

        if (isStoreActive)
        {
            // sync store hotbar with store hotbar
            for (int i = 0; i < store_Hotbar.transform.childCount; i++)
            {
                store_slots[i].SetItem(inGame_slots[i].inventory.slots[i]);
            }
        }
        else
        {
            // sync player hotbar with store hotbar
            for (int i = 0; i < inGame_Hotbar.transform.childCount; i++)
            {
                inGame_slots[i].SetItem(store_slots[i].inventory.slots[i]);
            }
        }
    }
}
