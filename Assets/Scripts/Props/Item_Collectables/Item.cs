using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Item : MonoBehaviour
{
    [SerializeField] public ItemData data;
    [HideInInspector] public Rigidbody2D rb2D;

    public int SellPrice => data.sellPrice;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
}
