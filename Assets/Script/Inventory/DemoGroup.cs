using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Description("Script pour les boutons de demo de l'inventaire. Sert seulement au debug.")]
public class DemoGroup : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public ItemClass[] item;
    
    public void PickupItem(int id)
    {
        bool result = inventoryManager.AddItem(item[id]);
        if (result == true)
        {
            Debug.Log("Item added");
        }
        else
        {
            Debug.Log("Inventory full");
        }
    }
}
