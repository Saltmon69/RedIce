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
    InputManager inputManager;
    
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
    [SerializeField] public PlayerModeSelect playerModeSelect;
    

    // Évite un potentiel oubli d'activation lors des tests et builds.
    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        
        inMenu = false;
        mainMenu.SetActive(false);
        inventory.SetActive(false);
        map.SetActive(false);
        ATH.SetActive(true);

        inputManager.mainMenu.performed += OnEscape;
        inputManager.inventory.performed += OnI;
        inputManager.map.performed += OnM;
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
    public void OnEscape(InputAction.CallbackContext context)
    {
        if(playerModeSelect.canPlayerSwitchMode)
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
    public void OnI(InputAction.CallbackContext context)
    {
        inMenu = true;
        inventory.SetActive(true);
        mainMenu.SetActive(false);
        map.SetActive(false);
    }
    
    /// <summary>
    /// Touche M
    /// </summary>
    public void OnM(InputAction.CallbackContext context)
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
        //Time.timeScale = 0;
            
        inputManager.DisableDeplacement();
        
        if (!playerInteraction.pingIsPressed)
        {
            inputManager.DisableInteractions();
            ATH.SetActive(false);
        }
    }
    
    public void OutMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //Time.timeScale = 1;
        
        inputManager.EnableDeplacement();
        inputManager.EnableInteractions();
        ATH.SetActive(true);
        mainMenu.SetActive(false);
    }
}
