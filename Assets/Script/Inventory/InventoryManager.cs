using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{
    [Tooltip("Array des slots de cet inventaire")]
    public InventorySlot[] slots;
    
    [Space(10)]
    
    [Header("Prefabs")]
    public GameObject inventoryItemPrefab;
    public GameObject inventorySlot;
    public GameObject inventoryPanel;
    
    [Space(10)]
    
    [Header("UI")]
    public Canvas canvas;
    
    [Tooltip("Taille de l'inventaire qui sera généré")]
    public int inventorySize;

    

    /// <summary>
    /// Permet d'ajouter un objet dans l'inventaire
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddItem(ItemClass item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            InventoryItem itemInSlot = null;
            InventorySlot slot = slots[i];
            if (slot.transform.childCount != 0)
            {
                itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot.item == item && itemInSlot.count < item.stackSize)
                {
                    itemInSlot.count++;
                    itemInSlot.RefreshCount();
                    return true;
                }
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
    
    /// <summary>
    /// Permet d'ajouter un objet dans l'inventaire sans vérifier qu'il y en a déjà un identique
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>

    public InventorySlot AddItemExclusive(ItemClass item)
    {
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
                return slot;
            }
        }

        return null;
    }

    /// <summary>
    /// Permet de créer un objet dans l'inventaire
    /// </summary>
    /// <param name="item"></param>
    /// <param name="slot"></param>
    public void SpawnItem(ItemClass item, InventorySlot slot)
    {
        GameObject newItemGameObject = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGameObject.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    /// <summary>
    /// Permet de créer un inventaire
    /// </summary>
    /// <param name="size"></param>
    public void InitInventory(int size)
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
