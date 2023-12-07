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
    [SerializeField] GameObject ATH;
    
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerMouseLook playerMouseLook;
    [SerializeField] PlayerInteraction playerInteraction;

    public bool inMenu;
    
    
    // Évite un potentiel oubli d'activation lors des tests et builds.
    private void Start()
    {
        inMenu = false;
        mainMenu.SetActive(false);
        inventory.SetActive(false);
        map.SetActive(false);
        ATH.SetActive(true);
    }

    private void Update()
    {
        if (inMenu) 
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            
            playerMovement.enabled = false;
            playerMouseLook.enabled = false;
            if (!playerInteraction.pingIsPressed)
            {
                playerInteraction.enabled = false;
                ATH.SetActive(false);
            }
        }
        if (!inMenu)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
            
            playerMovement.enabled = true;
            playerMouseLook.enabled = true;
            playerInteraction.enabled = true;
            ATH.SetActive(true);
        }
    }

    /// <summary>
    /// Touche Esc
    /// </summary>
    public void OnMainMenuPressed()
    {
        if (!inMenu)
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
            mainMenu.SetActive(true);
        }
    }
    
    /// <summary>
    /// Touche I
    /// </summary>
    public void OnInventoryPressed()
    {
        inMenu = true;
        map.SetActive(false);
        mainMenu.SetActive(false);
        inventory.SetActive(true);
    }
    
    /// <summary>
    /// Touche M
    /// </summary>
    public void OnMapPressed()
    {
        inMenu = true;
        mainMenu.SetActive(false);
        inventory.SetActive(false);
        map.SetActive(true);

    }
    
    
    public void OnQuitPressed()
    {
        mainMenu.SetActive(false);
        inMenu = false;
    }
}
