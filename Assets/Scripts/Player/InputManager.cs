using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Description("Script qui gère les inputs du joueur en utilisant le New Input System. C'est un singleton.")]
public class InputManager : MonoBehaviour
{
    #region Variables

    public static InputManager instance;
    
    
    // Varaibles des scripts de mouvement et autres interactions
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerMouseLook playerMouseLook;
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] PlayerMenuing playerMenuing;
    
    // Variables du New Input System
    PlayerController playerController;
    PlayerController.PlayerMovementActions playerHorizontalMovement;
    PlayerController.InteractActions playerInteractionActions;
    PlayerController.MenuingActions playerMenuingActions;
    
    
    Vector2 horizontalInput;
    Vector2 mouseInput;
    
    #endregion
    
    #region Fonctions
    private void Awake()
    {
        instance = this;
        
        playerController = new PlayerController();
        playerHorizontalMovement = playerController.PlayerMovement;
        playerInteractionActions = playerController.Interact;
        playerMenuingActions = playerController.Menuing;
        
        // playerMovement.[action].performed += ctx => Action à effectuer;
        
        //Déplacement (Z Q S D Shift C )
        playerHorizontalMovement.Deplacement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
        
        //Mouvement caméra (Souris)
        playerHorizontalMovement.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        playerHorizontalMovement.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
    }

    private void OnEnable()
    {
        playerController.Enable();
    }

    private void OnDestroy()
    {
        playerController.Disable();
    }

    private void Update()
    {
        playerMovement.ReceiveInput(horizontalInput);
        playerMouseLook.ReceiveInput(mouseInput);
    }
    
    #endregion
}    
