using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoldData", menuName = "ScriptableObjects/GoldData", order = 1)]
public class GoldData : ScriptableObject
{
    [SerializeField] public int currentGold;
    [SerializeField] public bool IsDoubleGoldActive;

    public int CurrentGold
    {
        get { return currentGold; }
        set { this.currentGold = value; }
    }
}
