using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Item : MonoBehaviour
{
    [SerializeField] public ItemData data;
    [HideInInspector] public Rigidbody2D rb2D;

    public string ItemName => data.ItemName;
    public string Description => data.Description;
    public int SellPrice => data.SellPrice;
    public int BuyPrice => data.BuyPrice;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
}
