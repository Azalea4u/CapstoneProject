using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bool_SO", menuName = "ScriptableObjects/Bool_SO", order = 2)]
public class Bool_SO : ScriptableObject
{
    [SerializeField] public bool Value;

    public void SetValue(bool newValue)
    {
        Value = newValue;
    }

    public void ToggleValue()
    {
        Value = !Value;
    }
}
