using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerMouseLook playerMouseLook;
    
    PlayerController playerController;
    PlayerController.PlayerMovementActions playerHorizontalMovement;
    
    
    Vector2 horizontalInput;
    Vector2 mouseInput;

    private void Awake()
    {
        playerController = new PlayerController();
        playerHorizontalMovement = playerController.PlayerMovement;
        
        // playerMovement.[action].performed += ctx => Action à effectuer;
        
        playerHorizontalMovement.Deplacement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
        
        playerHorizontalMovement.Jump.performed += _ => playerMovement.OnJumpPressed();
        
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
}    
