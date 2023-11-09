using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerMouseLook playerMouseLook;
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] PlayerMenuing playerMenuing;
    
    PlayerController playerController;
    PlayerController.PlayerMovementActions playerHorizontalMovement;
    PlayerController.InteractActions playerInteractionActions;
    PlayerController.MenuingActions playerMenuingActions;
    
    
    Vector2 horizontalInput;
    Vector2 mouseInput;

    private void Awake()
    {
        playerController = new PlayerController();
        playerHorizontalMovement = playerController.PlayerMovement;
        playerInteractionActions = playerController.Interact;
        playerMenuingActions = playerController.Menuing;
        
        // playerMovement.[action].performed += ctx => Action à effectuer;
        
        playerHorizontalMovement.Deplacement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
        
        playerHorizontalMovement.Jump.performed += _ => playerMovement.OnJumpPressed();
        
        playerHorizontalMovement.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        playerHorizontalMovement.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
        
        playerInteractionActions.Interaction.performed += _ => playerInteraction.OnInteractionPressed();
        playerInteractionActions.Use.performed += _ => playerInteraction.OnUsePressed();
        
        playerMenuingActions.MainMenu.performed += ctx => playerMenuing.OnMainMenuPressed();
        playerMenuingActions.Inventory.performed += ctx => playerMenuing.OnInventoryPressed();
        playerMenuingActions.Map.performed += ctx => playerMenuing.OnMapPressed();
        
        
        
        
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
        if (Input.GetKeyDown(KeyCode.C))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        playerMovement.ReceiveInput(horizontalInput);
        playerMouseLook.ReceiveInput(mouseInput);
        
        
    }
}    
