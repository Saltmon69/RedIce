using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VInspector;

public class PlayerMenuing : MonoBehaviour
{
    InputManager inputManager;
    
    [Tab("Menus")]
    [SerializeField] GameObject mainMenu;
    public GameObject inventory;
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
            Time.timeScale = 0;
            mainMenu.SetActive(true);
            inventory.SetActive(false);
            
        }
    }
    
    /// <summary>
    /// Touche I
    /// </summary>
    public void OnI(InputAction.CallbackContext context)
    {
        inMenu = true;
        Time.timeScale = 0;
        inventory.SetActive(true);
        mainMenu.SetActive(false);
        
    }
    
    /// <summary>
    /// Touche M
    /// </summary>
    public void OnM(InputAction.CallbackContext context)
    {
        inMenu = true;
        Time.timeScale = 0;
        mainMenu.SetActive(false);
        inventory.SetActive(false);
    }
    
    
    public void OnQuitPressed()
    {
        inMenu = false;
        Time.timeScale = 1;
    }
    
    public void InMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //Time.timeScale = 0;
            
        inputManager.DisableDeplacement();
        playerMouseLook.enabled = false;
        
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
        Time.timeScale = 1;
        
        inputManager.EnableDeplacement();
        inputManager.EnableInteractions();
        playerMouseLook.enabled = true;
        
        ATH.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Scene_Menu");
    }
    public void Inventory()
    {
        inMenu = true;
        Time.timeScale = 0;
        inventory.SetActive(true);
        mainMenu.SetActive(false);
        
    }
}
