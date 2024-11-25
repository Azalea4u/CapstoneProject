using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 50)]
public class ItemData : ScriptableObject
{
    [SerializeField] public string ItemName = "Item Name";
    [SerializeField] public Sprite Icon;
    [SerializeField] public int BuyPrice;
    [SerializeField] public int SellPrice;
    [SerializeField] public string Description = "Description of Item";
}
