using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<string, Inventory> inventoryByName = new Dictionary<string, Inventory>();

    public Inventory_UI inventoryUI;

    [Header("Hotbar")]
    public int hotbarSlotCount = 4;
    public Inventory hotbar;

    [Header("Store")]
    public int storeSlotCount = 6;
    public Inventory store;

    private void Awake()
    {
        hotbar = new Inventory(hotbarSlotCount);
        inventoryByName.Add("Hotbar", hotbar);

        store = new Inventory(storeSlotCount);
        inventoryByName.Add("Store", store);
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
