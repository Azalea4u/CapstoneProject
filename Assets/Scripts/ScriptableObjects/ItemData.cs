using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 50)]
public class ItemData : ScriptableObject
{
    [SerializeField] public string ItemName = "Item Name";
    [SerializeField] public Sprite Icon;
    [SerializeField] public int BuyPrice;
    [SerializeField] public int SellPrice;
    [SerializeField] public bool IsFood;

    [SerializeField, ConditionalField("IsFood", true)] // Custom attribute to show based on IsFood
    public float HungerHealed;

    [SerializeField] public string Description = "Description of Item";
}
