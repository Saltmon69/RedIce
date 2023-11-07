using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] slots;
    public GameObject inventoryItemPrefab;
    public GameObject inventorySlot;
    public GameObject inventoryPanel;
    public Canvas canvas;
    public int inventorySize;

    

    public bool AddItem(ItemClass item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            InventoryItem itemInSlot = null;
            InventorySlot slot = slots[i];
            if (slot.transform.childCount != 0)
            {
                itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            }
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < item.stackSize)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        
        for (int i = 0; i < slots.Length; i++)
        {
            InventoryItem itemInSlot = null;
            InventorySlot slot = slots[i];
            if (slot.transform.childCount != 0)
            {
                 itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            }
            if (itemInSlot == null)
            {
                SpawnItem(item, slot);
                return true;
            }
        }

        return false;
    }
    
    void SpawnItem(ItemClass item, InventorySlot slot)
    {
        GameObject newItemGameObject = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGameObject.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    public void InitUI(int size)
    {
        slots = new InventorySlot[size];
        var panel = Instantiate(inventoryPanel, canvas.transform);
        for (int i = 0; i < size; i++)
        {
            var slot = Instantiate(inventorySlot, panel.transform);
            slots[i] = slot.GetComponent<InventorySlot>();
        }
    }
}
