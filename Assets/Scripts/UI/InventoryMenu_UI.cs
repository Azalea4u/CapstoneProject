using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryMenu_UI : MonoBehaviour
{
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

        // Ensure both hotbar slot lists are the same size
        int minSlots = Mathf.Min(inGame_slots.Count, store_slots.Count);

        if (isStoreActive)
        {
            // Sync from in-game hotbar to store hotbar
            for (int i = 0; i < minSlots; i++)
            {
                if (inGame_slots[i].inventory != null && inGame_slots[i].inventory.slots.Count > i)
                {
                    store_slots[i].SetItem(inGame_slots[i].inventory.slots[i]);
                }
                else
                {
                    store_slots[i].SetEmpty();
                }
            }
        }
        else
        {
            // Sync from store hotbar to in-game hotbar
            for (int i = 0; i < minSlots; i++)
            {
                if (store_slots[i].inventory != null && store_slots[i].inventory.slots.Count > i)
                {
                    inGame_slots[i].SetItem(store_slots[i].inventory.slots[i]);
                }
                else
                {
                    inGame_slots[i].SetEmpty();
                }
            }
        }
    }

}
