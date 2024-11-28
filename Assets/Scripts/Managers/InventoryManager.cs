using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<string, Inventory> inventoryByName = new Dictionary<string, Inventory>();

    public Inventory_UI inventoryUI;

    [Header("InGame Hotbar")]
    public int hotbar_SlotCount = 4;
    public Inventory hotbar;
    public HotBar_Data hotbarData;

    private void Awake()
    {
        hotbar = new Inventory(hotbar_SlotCount);
        inventoryByName.Add("Hotbar", hotbar);

        LoadHotBarData();
    }

    public void LoadHotBarData()
    {
        // Sync hotbar inventory slots with loaded data
        for (int i = 0; i < hotbarData.slots.Count; i++)
        {
            if (i < hotbar.slots.Count)
            {
                hotbar.slots[i].itemName = hotbarData.slots[i].itemName;
                hotbar.slots[i].count = hotbarData.slots[i].count;
                hotbar.slots[i].icon = hotbarData.slots[i].icon;
            }
        }
    }

    public void SaveHotBarData()
    {
        hotbarData.UpdateData(hotbar.slots.Select(slot => new HotBar_Data.SlotData
        {
            itemName = slot.itemName,
            count = slot.count,
            icon = slot.icon
        }).ToList());
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

        EditorUtility.SetDirty(hotbarData);
        AssetDatabase.SaveAssets();
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
