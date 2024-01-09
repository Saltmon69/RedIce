using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VInspector;

public class PlayerMenuing : MonoBehaviour
{
    [Tab("Menus")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject inventory;
    [SerializeField] GameObject map;
    [SerializeField] GameObject ATH;
    
    [Tab("Références")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerMouseLook playerMouseLook;
    [SerializeField] PlayerInteraction playerInteraction;

    
    [HideInInspector] public bool inMenu;
    
    
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
            InMenu();
        }
        if (!inMenu)
        {
            OutMenu();
        }
    }

    /// <summary>
    /// Touche Esc
    /// </summary>
    public void OnMainMenuPressed()
    {
        
        if (inventory.activeSelf)
        {
            inventory.SetActive(false);
            mainMenu.SetActive(true);
        }
        if (map.activeSelf)
        {
            map.SetActive(false);
            mainMenu.SetActive(true);
        }
        else if(!inventory.activeSelf && !map.activeSelf && !mainMenu.activeSelf)
        {
            mainMenu.SetActive(true);
        }
        else if(mainMenu.activeSelf)
        {
            inMenu = false;
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
        inMenu = false;
    }
    
    private void InMenu()
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
    
    private void OutMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        
        mainMenu.SetActive(false);
        playerMovement.enabled = true;
        playerMouseLook.enabled = true;
        playerInteraction.enabled = true;
        ATH.SetActive(true);
    }
}
