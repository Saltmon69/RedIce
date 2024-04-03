using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using VInspector;

[Description("Script qui gère les inputs du joueur en utilisant le New Input System. C'est un singleton.")]
public class InputManager : MonoBehaviour
{
    
    public static InputManager instance;
    
    [Tab("Movements")]
    [HideInInspector] public InputAction deplacement;
    [HideInInspector] public InputAction jump;
    [HideInInspector] public InputAction run;
    [HideInInspector] public InputAction crouch;
    [HideInInspector] public InputAction mousex;
    [HideInInspector] public InputAction mousey;
    
    [Tab("Interactions")]
    [HideInInspector] public InputAction interact;
    [HideInInspector] public InputAction leftClick;
    [HideInInspector] public InputAction ping;
    [HideInInspector] public InputAction ava;
    [HideInInspector] public InputAction shoot;
    [HideInInspector] public InputAction scrollWheel;
    
    [Tab("Menuing")]
    [HideInInspector] public InputAction mainMenu;
    [HideInInspector] public InputAction inventory;
    [HideInInspector] public InputAction map;
    
    
    
    // Varaibles des scripts de mouvement et autres interactions
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerMouseLook playerMouseLook;
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] PlayerMenuing playerMenuing;
    
    // Variables du New Input System
    PlayerController playerController;
    
    
    
    
    
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        
        playerController = new PlayerController();
        
        InitDeplacement();
        InitInteractions();
        InitMenuing();
        
        EnableDeplacement();
        EnableInteractions();
        EnableMenuing();
        
    }

    private void InitDeplacement()
    {
        deplacement = playerController.PlayerMovement.Deplacement;
        jump = playerController.PlayerMovement.Jump;
        run = playerController.PlayerMovement.Run;
        crouch = playerController.PlayerMovement.Crouch;
        mousex = playerController.PlayerMovement.MouseX;
        mousey = playerController.PlayerMovement.MouseY;
    }
    
    private void InitInteractions()
    {
        interact = playerController.Interact.Interaction;
        leftClick = playerController.Interact.LeftClick;
        ping = playerController.Interact.Ping;
        ava = playerController.Interact.AVA;
        shoot = playerController.Interact.Shoot;
        scrollWheel = playerController.Interact.ScrollWheel;
    }
    
    private void InitMenuing()
    {
        mainMenu = playerController.Menuing.MainMenu;
        inventory = playerController.Menuing.Inventory;
        map = playerController.Menuing.Map;
    }
    
    private void EnableDeplacement()
    {
        deplacement.Enable();
        jump.Enable();
        run.Enable();
        crouch.Enable();
        mousex.Enable();
        mousey.Enable();
    }
    
    private void EnableInteractions()
    {
        interact.Enable();
        leftClick.Enable();
        ping.Enable();
        ava.Enable();
        shoot.Enable();
        scrollWheel.Enable();
    }
    
    private void EnableMenuing()
    {
        mainMenu.Enable();
        inventory.Enable();
        map.Enable();
    }
    
    private void DisableDeplacement()
    {
        deplacement.Disable();
        jump.Disable();
        run.Disable();
        crouch.Disable();
        mousex.Disable();
        mousey.Disable();
    }
    
    private void DisableInteractions()
    {
        interact.Disable();
        leftClick.Disable();
        ping.Disable();
        ava.Disable();
        shoot.Disable();
        scrollWheel.Disable();
    }
    
    private void DisableMenuing()
    {
        mainMenu.Disable();
        inventory.Disable();
        map.Disable();
    }
    
    
    
}    
