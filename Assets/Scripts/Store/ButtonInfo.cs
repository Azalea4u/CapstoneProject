using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonInfo : MonoBehaviour
{
    [SerializeField] public int ItemID;
    [SerializeField] public TextMeshProUGUI PriceText;
    [SerializeField] public GameObject ShopManager;

    private void Update()
    {
        PriceText.text = ShopManager.GetComponent<ShopManager>().shopItems[2, ItemID].ToString();
    }
}
