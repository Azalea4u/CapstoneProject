using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public Dictionary<string, Inventory_UI> inventoryUIByName = new Dictionary<string, Inventory_UI>();

    [SerializeField] public List<Inventory_UI> inventoryUIs;
    [SerializeField] GameObject inventoryPanel;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        inventoryPanel.SetActive(false);
    }

    public Inventory_UI GetInventory_UI(string inventoryName)
    {
        if (inventoryUIByName.ContainsKey(inventoryName))
        {
            return inventoryUIByName[inventoryName];
        }

        Debug.LogWarning("Inventory UI with name " + inventoryName + " not found.");
        return null;
    }

    private void Initialize()
    {
        foreach(Inventory_UI ui in inventoryUIs)
        {
            if (inventoryUIByName.ContainsKey(ui.inventoryName))
            {
                inventoryUIByName.Add(ui.inventoryName, ui);
            }
        }
    }
}
