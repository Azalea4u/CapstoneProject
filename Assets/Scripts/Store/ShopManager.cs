using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public ShopManager shopManager;

    [SerializeField] public List<Item> shopItems = new List<Item>();

    [SerializeField] private Player_UI playerUI;

    // Buttons
    [Header("Buy")]
    [SerializeField] public Button[] buttons;

    [Header("Sell")]
    [SerializeField] public Button SellButton;
    [SerializeField] public Slot_UI sellSlot;

    // Items To Buy
    [Header("Items")]
    [SerializeField] public Item[] ItemsToBuy;

    public InventoryManager inventoryManager;
    public bool DoubleGold;

    private void Start()
    {
        SelectSlot(sellSlot);

        for (int i = 0; i < buttons.Length; i++)
        {
            int itemID = i;
            buttons[i].onClick.AddListener(() => BuyItem(itemID));
            buttons[i].enabled = true;
        }
    }

    public void BuyItem(int itemID)
    {
        if (itemID < 0 || itemID >= ItemsToBuy.Length) return;

        Item selectedItem = ItemsToBuy[itemID];
        if (playerUI.Gold >= selectedItem.BuyPrice)
        {
            Debug.Log($"Bought {selectedItem.ItemName}");
            playerUI.Gold -= selectedItem.BuyPrice;
            inventoryManager.Add("Hotbar", selectedItem);

            // Check if DoubleGold_Item was bought
            if (selectedItem.ItemName == "DoubleGold_Item")
            {
                DoubleGold = true;
                buttons[itemID].enabled = false;
                Debug.Log("Double gold activated for the next level!");
            }
        }
        else
        {
            Debug.Log("Not enough coins");
        }
    }


    /*
    public void SellItem()
    {
        if (GameManager.instance.player.inventoryManager.sellSlot.slots[0] != null)
        {
            // get the item from the slot[0]
            Inventory.Slot slot = GameManager.instance.player.inventoryManager.sellSlot.slots[0];
            if (slot.ItemName.Contains("Tomato") || slot.ItemName.Contains("Wheat"))
            {
                Item itemToSell = GameManager.instance.itemManager.GetItemByName(slot.ItemName);
                if (itemToSell != null)
                {
                    int SellPrice = itemToSell.SellPrice;
                    coins += SellPrice * slot.count;
                    slot.count = 0;
                    if (slot.count == 0 )
                    {
                        slot.ItemName = "";
                        slot.Icon = null;
                    }
                    coinsText.text = coins.ToString();

                    // Clear the sell slot
                   // sellSlot.SetEmpty();
                    RefreshSellSlot();
                }
            }
            else
            {
                Debug.Log("You can't sell this item");
            }
        }
    }
    */

    public void RefreshSellSlot()
    {
        if (sellSlot != null)
        {
            if (sellSlot.inventory.selectedSlot.itemName != "")
            {
                sellSlot.SetItem(sellSlot.inventory.selectedSlot);
            }
            else
            {
                sellSlot.SetEmpty();
            }
        }
    }

    public void SelectSlot(Slot_UI slot)
    {
        //GameManager.instance.player.inventoryManager.sellSlot.SelectSlot(slot.slotID);
    }

}
