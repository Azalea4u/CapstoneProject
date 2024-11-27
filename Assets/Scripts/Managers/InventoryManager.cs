using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<string, Inventory> inventoryByName = new Dictionary<string, Inventory>();

    public Inventory_UI inventoryUI;

    [Header("InGame Hotbar")]
    public int hotbar_SlotCount = 4;
    public Inventory hotbar;
    public HotBar_Data hotbarData;

    [Header("Store Hotbar")]
    public int hotbarStore_SlotCount = 4;
    public Inventory hotbar_Store;

    private void Awake()
    {
        hotbar = new Inventory(hotbar_SlotCount);
        inventoryByName.Add("Hotbar", hotbar);

        hotbar_Store = new Inventory(hotbarStore_SlotCount);
        inventoryByName.Add("Hotbar_Store", hotbar_Store);
    }

    public void RefreshHotBarData()
    {
        hotbarData.slots.Clear();

        foreach (var slot in hotbar.slots)
        {
            var slotData = new HotBar_Data.SlotData
            {
                itemName = slot.itemName,
                count = slot.count,
                icon = slot.icon
            };
            hotbarData.slots.Add(slotData);
        }
    }

    // Call this in your Add and Remove methods
    public void Add(string inventoryName, Item item)
    {
        if (inventoryByName.ContainsKey(inventoryName))
        {
            inventoryByName[inventoryName].Add(item);
            inventoryUI.Refresh();
            RefreshHotBarData();
        }
    }

    public void Remove(string inventoryName, int slotID, int quantity)
    {
        if (inventoryByName.ContainsKey(inventoryName))
        {
            inventoryByName[inventoryName].Remove(slotID, quantity);
            inventoryUI.Refresh();
            RefreshHotBarData();
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
