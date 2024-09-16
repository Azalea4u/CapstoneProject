using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    public bool canReceiveInput;
    public bool inputReceived;

    private void Awake()
    {
        instance = this;
    }

    public void Atack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (canReceiveInput)
            {
                inputReceived = true;
                canReceiveInput = false;
            }
            else
            {
                return;
            }
        }
    }

    public void InputManager()
    {
        if (!canReceiveInput)
        {
            canReceiveInput = true;
        }
        else
        {
            canReceiveInput = false;
        }
    }
}

