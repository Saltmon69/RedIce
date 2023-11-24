using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMenuing : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject inventory;
    [SerializeField] GameObject map;
    
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerMouseLook playerMouseLook;
    [SerializeField] PlayerInteraction playerInteraction;

    public bool inMenu;
    
    
    private void Start()
    {
        inMenu = false;
        mainMenu.SetActive(false);
        inventory.SetActive(false);
        map.SetActive(false);
    }

    private void Update()
    {
        if (inMenu)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            playerMovement.enabled = false;
            playerMouseLook.enabled = false;
            playerInteraction.enabled = false;
        }
        if (!inMenu)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            playerMovement.enabled = true;
            playerMouseLook.enabled = true;
            playerInteraction.enabled = true;
        }
    }


    public void OnMainMenuPressed()
    {
        if (inMenu)
        {
            inMenu = false;
            mainMenu.SetActive(false);
            inventory.SetActive(false);
            map.SetActive(false);
            
        }
        else
        {
            inMenu = true;
            mainMenu.SetActive(true);
        }
        
        
        if (inventory.activeSelf)
        {
            inventory.SetActive(false);
            
        }
        else if (map.activeSelf)
        {
            map.SetActive(false);
            
        }
        else
        {
            Debug.Log("Main Menu");
            mainMenu.SetActive(true);
            
        }
    }
    
    public void OnInventoryPressed()
    {
        inMenu = true;
        Debug.Log("Inventory");
        inventory.SetActive(true);
    }
    
    public void OnMapPressed()
    {
        inMenu = true;
        Debug.Log("Map");
        map.SetActive(true);

    }
}
