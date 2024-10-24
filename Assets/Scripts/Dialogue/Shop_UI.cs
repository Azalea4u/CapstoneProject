using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop_UI : MonoBehaviour
{
    [Header("Buy Panel")]
    [SerializeField] private Button[] buyButtons;
    [SerializeField] private Button closeBuyButton;

    [Header("Sell Panel")]
    [SerializeField] private Button[] sellButtons;
    [SerializeField] private Button closeSellButton;

    private void Start()
    {
        // Setup close button listeners
        if (closeBuyButton != null)
            closeBuyButton.onClick.AddListener(CloseBuyPanel);

        if (closeSellButton != null)
            closeSellButton.onClick.AddListener(CloseSellPanel);
    }

    private void CloseBuyPanel()
    {
        // Call the DialogueManager to handle closing the buy panel
        DialogueManager.GetInstance().CloseBuyMenu();
    }

    private void CloseSellPanel()
    {
        // Call the DialogueManager to handle closing the sell panel
        DialogueManager.GetInstance().CloseSellMenu();
    }

    // Add methods to handle buying and selling items
    public void BuyItem(int itemIndex)
    {
        // Add your buying logic here
        Debug.Log($"Buying item {itemIndex}");
    }

    public void SellItem(int itemIndex)
    {
        // Add your selling logic here
        Debug.Log($"Selling item {itemIndex}");
    }
}