using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] public int[,] shopItems = new int[4, 3]; // [ID, ItemID, Price
    [SerializeField] public float coins;
    [SerializeField] public TextMeshProUGUI coinsText;

    // Buttons
    [Header("Buttons")]
    [SerializeField] public Button[] buttons;

    [Header("Sell")]
    [SerializeField] public Button SellButton;
    [SerializeField] public Slot_UI sellSlot;

    // Items To Buy
    [Header("Items")]
    [SerializeField] public Item WheatSeeds;
    [SerializeField] public Item TomateSeeds;

    public InventoryManager inventoryManager;

    private void Start()
    {
        SelectSlot(sellSlot);

        coinsText.text = coins.ToString();

        // ID's
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;

        // Prices
        shopItems[2, 1] = 15;
        shopItems[2, 2] = 20;

        for (int i = 0; i < buttons.Length; i++)
        {
            int itemID = i + 1;
            buttons[i].onClick.AddListener(() => BuyItem(itemID));
        }
    }

    public void BuyItem(int itemID)
    {
        if (coins >= shopItems[2, itemID])
        {
            coins -= shopItems[2, itemID];
            shopItems[3, itemID]++;
            coinsText.text = coins.ToString();

            switch (itemID)
            {
                case 1:
                    inventoryManager.Add("Hotbar", WheatSeeds);
                    break;
                case 2:
                    inventoryManager.Add("Hotbar", TomateSeeds);
                    break;
            }
        }
        else
        {
            Debug.Log("Not enough coins");
        }
    }

    public void SellItem()
    {
        if (GameManager.instance.player.inventoryManager.sellSlot.slots[0] != null)
        {
            // get the item from the slot[0]
            Inventory.Slot slot = GameManager.instance.player.inventoryManager.sellSlot.slots[0];
            if (slot.itemName.Contains("Tomato") || slot.itemName.Contains("Wheat"))
            {
                Item itemToSell = GameManager.instance.itemManager.GetItemByName(slot.itemName);
                if (itemToSell != null)
                {
                    int sellPrice = itemToSell.SellPrice;
                    coins += sellPrice * slot.count;
                    slot.count = 0;
                    if (slot.count == 0 )
                    {
                        slot.itemName = "";
                        slot.icon = null;
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
        GameManager.instance.player.inventoryManager.sellSlot.SelectSlot(slot.slotID);
    }

}
