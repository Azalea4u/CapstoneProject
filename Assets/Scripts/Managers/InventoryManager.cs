using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<string, Inventory> inventoryByName = new Dictionary<string, Inventory>();

    public Inventory_UI inventoryUI;

    //[Header("Backpack")]
    //public int backpackSlotCount = 21;
    //public Inventory backpack;

    [Header("SellSlot")]
    public int sellSlotCount = 1;
    public Inventory sellSlot;

    [Header("Hotbar")]
    public int hotbarSlotCount = 4;
    public Inventory hotbar;

    [Header("Inventory")]
    public int inventorySlotCount = 8;
    public Inventory inventory;

    private void Awake()
    {
        //backpack = new Inventory(backpackSlotCount);
        //inventoryByName.Add("Backpack", backpack);

        sellSlot = new Inventory(sellSlotCount);
        inventoryByName.Add("SellSlot", sellSlot);

        hotbar = new Inventory(hotbarSlotCount);
        inventoryByName.Add("Hotbar", hotbar);

        hotbar = new Inventory(hotbarSlotCount);
        inventoryByName.Add("Inventory", hotbar);
    }

    public void Add(string inventoryName, Item item)
    {
        if (inventoryByName.ContainsKey(inventoryName))
        {
            inventoryByName[inventoryName].Add(item);
            inventoryUI.Refresh();
        }
    }

    public void Remove(string inventoryName, int slotID, int quantity)
    {
        if (inventoryByName.ContainsKey(inventoryName))
        {
            inventoryByName[inventoryName].Remove(slotID, quantity);
            inventoryUI.Refresh();
        }
    }

    public Inventory GetInventoryByName(string inventoryName)
    {
        if (inventoryByName.ContainsKey(inventoryName))
        {
            return inventoryByName[inventoryName];
        }

        return null;
    }
}
