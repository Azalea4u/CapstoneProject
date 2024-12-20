using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] public int[,] shopItems = new int[4, 3]; // [ID, ItemID, Price

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
    [SerializeField] public Item Bomb;
    [SerializeField] public Item Apple;
    [SerializeField] public Item Bread;
    [SerializeField] public Item Meat;

    public InventoryManager inventoryManager;

    private int gold;

    private void Start()
    {
        SelectSlot(sellSlot);

        // ID's
       // shopItems[1, 1] = 1;
        //shopItems[1, 2] = 2;

        // Prices
        //shopItems[2, 1] = 15;
        //shopItems[2, 2] = 20;

        for (int i = 0; i < buttons.Length; i++)
        {
            int itemID = i + 1;
            //buttons[i].onClick.AddListener(() => BuyItem(itemID));
        }
    }

    private void Update()
    {
        //gold = GameManager.instance.Gold;
    }

    /*
    public void BuyItem(int itemID)
    {
        if (Player_UI.instance.Gold >= shopItems[6, itemID])
        {
            Player_UI.instance.Gold -= shopItems[6, itemID];
            shopItems[6, itemID]++;

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
    */

    public void BuyBomb()
    {
        if (playerUI.Gold >= Bomb.BuyPrice)
        {
            Debug.Log("Bought Bomb");
            playerUI.Gold -= Bomb.BuyPrice;
            inventoryManager.Add("Hotbar", Bomb);
        }
        else
        {
            Debug.Log("Not enough coins");
        }
    }

    public void BuyApple()
    {
        if (playerUI.Gold >= Apple.BuyPrice)
        {
            Debug.Log("Bought Apple");
            playerUI.Gold -= Apple.BuyPrice;
            inventoryManager.Add("Hotbar", Apple);
        }
        else
        {
            Debug.Log("Not enough coins");
        }
    }

    public void BuyBread()
    {
        if (playerUI.Gold >= Bread.BuyPrice)
        {
            Debug.Log("Bought Bread");
            playerUI.Gold -= Bread.BuyPrice;
            inventoryManager.Add("Hotbar", Bread);
        }
        else
        {
            Debug.Log("Not enough coins");
        }
    }

    public void BuyMeat()
    {
        if (playerUI.Gold >= Meat.BuyPrice)
        {
            Debug.Log("Bought Meat");
            playerUI.Gold -= Meat.BuyPrice;
            inventoryManager.Add("Hotbar", Meat);
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
