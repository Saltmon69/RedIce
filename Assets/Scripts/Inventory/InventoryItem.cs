using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Description("Cette classe est l'image de l'item dans l'inventaire. Elle contient les fonctions gérant son affichage ainsi que le drag and drop.")]
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    #region Variables
    
    [Header("System")]
    [HideInInspector] public ItemClass item;
    [HideInInspector] public Transform parentAfterDrag;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private InventoryManager inventoryManager;
    
    
    [Space(10)]
    
    [Header("UI")] 
    public Image image;
    public TextMeshProUGUI countText;
    [SerializeField] private GameObject utilityPanel;
    [HideInInspector] public bool panelActive;
  
    [Space(10)]
    
    [Header("Data")]
    public int count = 1;
    public int stackSize;
    
    #endregion


    #region Fonctions
    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }
    
    /// <summary>
    /// Permet de créer l'ojet dans l'inventaire avec les paramètres du SO
    /// </summary>
    /// <param name="newItem"> De type ItemClass qui contient le sprite ainsi que la quantité max. </param>
    public void InitialiseItem(ItemClass newItem)
    {
        item = newItem;
        image.sprite = newItem.sprite;
        stackSize = newItem.stackSize;
        RefreshCount();
    }

    /// <summary>
    /// Permet de rafraîchir l'affichage du nombre de l'objet
    /// </summary>
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
        
        InventoryItem itemInSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventoryItem>();
       
        if (itemInSlot != null)  // On vérifie que l'objet est bien lâché sur un slot et par conséquent si le slot contient un objet. Cela permet d'éviter un placement random sur l'UI.
        {
            if (itemInSlot.item == item && itemInSlot.count < item.stackSize)
            {
                if(itemInSlot.count <= item.stackSize - count)
                {
                    itemInSlot.count += count;
                    itemInSlot.RefreshCount();
                    Destroy(gameObject);
                }
                else
                {
                    int difference = item.stackSize - itemInSlot.count;
                    itemInSlot.count += difference;
                    itemInSlot.RefreshCount();
                    count -= difference;
                    RefreshCount();
                }
            }
        }
        else
        {
            transform.SetParent(parentAfterDrag);
            transform.position = parentAfterDrag.position;
        }
        
        // Permet de remettre l'objet là où il était avant le drag
        transform.SetParent(parentAfterDrag);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        
        if (eventData.button == PointerEventData.InputButton.Right && panelActive == false)
        {
            var panelPos = Instantiate(utilityPanel, transform.parent.parent.parent).GetComponent<UtilityPanel>();
            panelPos.item = this;
            panelPos.rectTransform.position = rectTransform.position;
            panelActive = true;
            
        }
    }
    
    #endregion
}
