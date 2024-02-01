using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VInspector;

public class PlayerMenuing : MonoBehaviour
{
    [Tab("Menus")]
    [SerializeField] GameObject mainMenu;
    public GameObject inventory;
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
    public void OnEscapePressed()
    {
        if (inMenu || !mainMenu.activeSelf)
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
            inventory.SetActive(false);
            map.SetActive(false);
        }
    }
    
    /// <summary>
    /// Touche I
    /// </summary>
    public void OnIPressed()
    {
        inMenu = true;
        inventory.SetActive(true);
        mainMenu.SetActive(false);
        map.SetActive(false);
    }
    
    /// <summary>
    /// Touche M
    /// </summary>
    public void OnMPressed()
    {
        inMenu = true;
        map.SetActive(true);
        mainMenu.SetActive(false);
        inventory.SetActive(false);
    }
    
    
    public void OnQuitPressed()
    {
        inMenu = false;
    }
    
    public void InMenu()
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
    
    public void OutMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        
        playerMovement.enabled = true;
        playerMouseLook.enabled = true;
        playerInteraction.enabled = true;
        ATH.SetActive(true);
        mainMenu.SetActive(false);
    }
}
