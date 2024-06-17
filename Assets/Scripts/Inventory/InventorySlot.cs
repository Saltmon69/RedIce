using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;

[Description("Ce script permet de drop l'item dans un slot de l'inventaire")]
public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        try
        {
            if (transform.childCount == 0)
            {
                InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
                item.parentAfterDrag = transform;
            }
        }catch(NullReferenceException){}
    }
}
