using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using VInspector;

[Description("Gère les déplacements du joueur")]
public class PlayerMovement : MonoBehaviour
{
    #pragma warning disable 0649
    
    InputManager inputManager;
   
    [Tab("States")]
    [SerializeField] bool sprint = false;
    [SerializeField] bool walk = false;
    [SerializeField] bool isGrounded;
    [SerializeField] bool jump;
    [SerializeField] bool crouch;
    
    
    [Tab("Mouvements")]
    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 12f;
    Vector2 horizontalInput;
    
    [Tab("Saut")]
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask obstacleMask;
    Vector3 verticalVelocity = Vector3.zero;
    float halfHeight;
    
    [Tab("Crouch")]
    [SerializeField] Transform playerCamera;
    [SerializeField] private float standingHeight;
    [SerializeField] private float crouchHeight;
    
    
    [Tab("SFX")]
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip landSFX;
    [SerializeField] private AudioClip walkSFX;
    [SerializeField] private AudioClip runSFX;
    [SerializeField] private AudioClip crouchSFX;
    [SerializeField] private AudioSource audioSource;
    
    

    #region Fonctions

    
    private void Start()
    {
        inputManager = InputManager.instance;
        
        standingHeight = controller.height;
        crouchHeight = standingHeight / 2;
        
        inputManager.deplacement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
        inputManager.deplacement.performed += Walk;
        inputManager.deplacement.canceled += Walk;
        inputManager.jump.performed += OnJumpPressed;
        inputManager.run.performed += OnSprint;
        inputManager.run.canceled += OnSprint;
        inputManager.crouch.performed += OnCrouchPressed;
    }

    private void Update()
    {
        //Jump
        
        halfHeight = controller.height / 2;
        var bottomPoint = transform.TransformPoint(controller.center - Vector3.up * halfHeight);
        isGrounded = Physics.CheckBox(bottomPoint, new Vector3(0.4f, 0.1f, 0.4f), Quaternion.identity, groundMask|obstacleMask);
        
        if (isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f;
        }
        
        if (jump)
        {
            if (isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            jump = false;
        }
        
        //Caméra
        
        Vector3 camForward = playerCamera.forward;
        Vector3 camRight = playerCamera.right;
        
        camForward.y = 0;
        camRight.y = 0;
        
        Vector3 forwardRelative = camForward.normalized * horizontalInput.y;
        Vector3 rightRelative = camRight.normalized * horizontalInput.x;
        
        //Mouvements
        
        Vector3 moveDirection = forwardRelative + rightRelative;
        
        Vector3 horizontalVelocity = (moveDirection) * speed;
        controller.Move(horizontalVelocity * Time.deltaTime);
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
        
        
        
        if (crouch)
        {
            controller.height = crouchHeight;
            speed = 6f;
            playerCamera.localPosition = new Vector3(0, 0.5f, 0);
            
            
        }
        if (!crouch)
        {
            controller.height = standingHeight;
            speed = 12f;
            playerCamera.localPosition = new Vector3(0, 0.8f, 0);
            
        }

        if (!crouch)
        {
            if (sprint)
            {
                speed = 24f;
            }
            if (!sprint)
            {
                speed = 12f;
            }
        }
    }
    
    public void Walk(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            audioSource.clip = walkSFX;
            audioSource.loop = true;
            audioSource.Play();
        }
        else if(context.canceled)
        {
           audioSource.Stop();
           audioSource.loop = false;
        }
    }
    public void OnJumpPressed(InputAction.CallbackContext context)
    {
        if(context.performed)
         jump = true;
    }
    
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            sprint = true;
        }
        else if(context.canceled)
        {
            sprint = false;
        }
    }

    
    
    public void OnCrouchPressed(InputAction.CallbackContext context)
    {
        if(crouch == false)
        {
            crouch = true;
        }
        else
        {
            crouch = false;
        }
    }
    
    
   
    #endregion
}
