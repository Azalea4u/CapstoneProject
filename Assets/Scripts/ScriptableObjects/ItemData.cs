using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 50)]
public class ItemData : ScriptableObject
{
    [SerializeField] public string itemName = "Item Name";
    [SerializeField] public Sprite icon;
    [SerializeField] public int sellPrice;

}
