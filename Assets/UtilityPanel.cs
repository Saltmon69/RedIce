using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityPanel : MonoBehaviour
{
    public InventoryItem item = null;
    public InventoryManager inventoryManager;
    public RectTransform rectTransform;

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void UseItem()
    {
        if (item.item.isUsable == true)
        {
            item.count--;
            item.RefreshCount();
            if (item.count == 0)
            {
                Destroy(item.gameObject);
            }
        }
        ClosePanel();
    }
    
    public void DropItem()
    {
        Destroy(item.gameObject);
        ClosePanel();
    }
    
    public void SplitItem()
    {
        if (item.count > 1)
        {
            int half = item.count / 2;
            item.count -= half;
            item.RefreshCount();
            InventoryItem newItem = inventoryManager.AddItemExclusive(item.item).GetComponentInChildren<InventoryItem>();
            newItem.count = half;
            newItem.RefreshCount();
        }
        ClosePanel();
    }
    
    public void ClosePanel()
    {
        item.panelActive = false;
        Destroy(gameObject);
    }
}
