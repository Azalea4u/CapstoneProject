using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Int_SO", menuName = "ScriptableObjects/Int_SO", order = 1)]
public class Int_SO : ScriptableObject
{
	[SerializeField] public int value;

	public int Value
	{
		get { return value; }
		set { this.value = value; }
	}

}
