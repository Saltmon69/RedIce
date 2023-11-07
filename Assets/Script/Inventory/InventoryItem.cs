using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public ItemClass item;
    [HideInInspector] public Transform parentAfterDrag;
    
    [Header("UI")] 
    public Image image;
    public TextMeshProUGUI countText;

    public int count = 1;
    public int stackSize;
    
    
    public void InitialiseItem(ItemClass newItem)
    {
        item = newItem;
        image.sprite = newItem.sprite;
        stackSize = newItem.stackSize;
        RefreshCount();
    }

    public void RefreshCount()
    {
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
        countText.text = count.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }
}
