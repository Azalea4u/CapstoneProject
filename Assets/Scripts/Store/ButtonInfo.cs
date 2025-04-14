using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonInfo : MonoBehaviour
{
    [SerializeField] public int ItemID;  // ID of the item
    [SerializeField] public TextMeshProUGUI NameText;
    [SerializeField] public TextMeshProUGUI DescriptionText;
    [SerializeField] public TextMeshProUGUI PriceText;
    [SerializeField] public GameObject ShopManager;

    private void Update()
    {
        // Get the ShopManager component
        ShopManager shopManager = ShopManager.GetComponent<ShopManager>();

        // Access the item from the ItemsToBuy array using the ItemID
        Item item = shopManager.ItemsToBuy[ItemID];

        // Update UI elements with the item info
        NameText.text = item.ItemName;
        DescriptionText.text = item.Description;
        PriceText.text = item.BuyPrice.ToString();
    }
}
