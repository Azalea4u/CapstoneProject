using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HungerData", menuName = "ScriptableObjects/HungerData")]
public class HungerData : ScriptableObject
{
    [SerializeField, Range(0, 100)] public float Hunger;
    [SerializeField] public float HungerSpeed;
    [SerializeField] public bool JustAte;
}
