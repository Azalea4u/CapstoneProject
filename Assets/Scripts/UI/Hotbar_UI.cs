using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar_UI : MonoBehaviour
{
    [SerializeField] public List<Slot_UI> hotbar_Slots = new List<Slot_UI>();

    private Slot_UI selectedSlot;

    private void Start()
    {
        SelectSlot(0);
    }

    private void Update()
    {
        CheckAlphaNumericKeys();
    }

    public void SelectSlot(Slot_UI slot)
    {
        SelectSlot(slot.slotID);
    }

    public void SelectSlot(int index)
    {
        if (hotbar_Slots.Count == 5)
        {
            if (selectedSlot != null)
            {
                selectedSlot.SetHighlight(false);
            }
            selectedSlot = hotbar_Slots[index];
            selectedSlot.SetHighlight(true);
            
            GameManager.instance.player.inventoryManager.hotbar.SelectSlot(index);
        }
    }

    private void CheckAlphaNumericKeys()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1)) 
        {
            SelectSlot(0);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            SelectSlot(1);            
        }

        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            SelectSlot(2);
        }

        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            SelectSlot(3);
        }
    }
}
