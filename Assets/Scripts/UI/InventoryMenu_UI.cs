using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryMenu_UI : MonoBehaviour
{
    [SerializeField] public string inventoryName;
    [SerializeField] private Canvas canvas;
    [SerializeField] public GameObject storePanel;


    [Header("Hotbars")]
    [SerializeField] private GameObject inGame_Hotbar;
    [SerializeField] private GameObject store_Hotbar;

    private Inventory inventory;
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
        if (SceneManager.GetActiveScene().name == "Rest_Level")
        {
            inventory = GameManager.instance.player.inventoryManager.GetInventoryByName(inventoryName);
            storePanel.SetActive(false);
        }
    }

    private void Update()
    {
        //isStoreActive = storePanel.activeSelf;
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
