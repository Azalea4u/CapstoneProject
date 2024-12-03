using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [HideInInspector] public InventoryManager inventoryManager;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerHealth playerHealth;

    [SerializeField] private HotBar_Data hotBar_Data;

    private void Awake()
    {
        inventoryManager = GetComponent<InventoryManager>();
    }

    private void Update()
    {
        if (playerMovement.isCrouching && Input.GetMouseButtonDown(0))
        {
            if (HasBombInHotbar() && inventoryManager.hotbar.selectedSlot.itemName == "Bomb")
            {
                UseBomb();
                playerMovement.PlaceBomb();


                // Save changes and refresh UI after using bomb
                inventoryManager.LoadHotBarData();
                inventoryManager.inventoryUI.Refresh();
                //inventoryManager.RefreshHotBarData(); // Ensure UI updates
            }
            else
            {
                Debug.Log("No bombs available in the hotbar!");
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && 
            !GameManager.instance.isGamePaused && !DialogueManager.instance.dialogueIsPlaying)
        { 
            ConsumeFood();

            // Refresh hotbar UI
            inventoryManager.LoadHotBarData();
            inventoryManager.inventoryUI.Refresh();
        }
    }

    private bool HasBombInHotbar()
    {
        foreach (var slot in hotBar_Data.slots)
        {
            if (slot.itemName == "Bomb" && slot.count > 0)
            {
                return true;
            }
        }
        return false;
    }

    private void UseBomb()
    {
        for (int i = 0; i < hotBar_Data.slots.Count; i++)
        {
            if (hotBar_Data.slots[i].itemName == "Bomb" && hotBar_Data.slots[i].count > 0)
            {
                hotBar_Data.slots[i].count--;

                // Remove the slot if count drops to 0
                if (hotBar_Data.slots[i].count <= 0)
                {
                    hotBar_Data.slots[i].itemName = null;
                    hotBar_Data.slots[i].icon = null;
                }
                break;
            }
        }
    }

    private void ConsumeFood()
    {
        var selectedSlot = inventoryManager.hotbar.selectedSlot;

        if (selectedSlot != null && !string.IsNullOrEmpty(selectedSlot.itemName) && GameManager.instance.playerUI.Hunger < 100)
        {
            var itemData = GameManager.instance.itemManager.GetItemByName(selectedSlot.itemName);

            //Debug.Log("Item food: "+ itemData.IsFood);

            if (itemData != null && itemData.IsFood)
            {
                // Regenerate hunger
                HungerData hungerData = GameManager.instance.playerUI.hungerData;
                hungerData.Hunger = Mathf.Min(100, hungerData.Hunger + itemData.HealHunger);

                // Remove the food item
                if (itemData.IsFood && hotBar_Data.slots[selectedSlot.count].count > 0)
                {
                    hotBar_Data.slots[selectedSlot.count].count--;

                    // Remove the slot if count drops to 0
                    if (hotBar_Data.slots[selectedSlot.count].count <= 0)
                    {
                        hotBar_Data.slots[selectedSlot.count].itemName = null;
                        hotBar_Data.slots[selectedSlot.count].icon = null;
                    }
                }

                Debug.Log($"Consumed {itemData.ItemName}, Hunger: {hungerData.Hunger}");
            }
            else
            {
                Debug.Log("Selected item is not food.");
            }
        }
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