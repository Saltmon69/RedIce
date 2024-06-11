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
    
    [SerializeField] InputManager inputManager;
   
    [Tab("States")]
    [SerializeField] bool sprint = false;
    [SerializeField] bool walk = false;
    [SerializeField] bool isGrounded;
    [SerializeField] bool jump;
    [SerializeField] bool crouch;
    [SerializeField] private bool falling;
    
    
    [Tab("Mouvements")]
    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 12f;
    Vector2 horizontalInput;
    Vector3 horizontalVelocity, moveDirection;
    private Transform fallStart;
    private Transform fallEnd;
    
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
    
    [Tab("References")]
    [SerializeField] private PlayerManager playerManager;
    
    [Tab("Camera effects")]
    public float baseCameraFOV = 60f;
    public float baseCameraHeight = 1.8f;
    
    public float walkBobbingRate = .75f;
    public float runBobbingRate = 1f;
    public float maxWalkBobbingOffset = .2f;
    public float maxRunBobbingOffset = .3f;
    
    [Tab("SFX")]
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip landSFX;
    [SerializeField] private AudioClip walkSFX;
    [SerializeField] private AudioClip runSFX;
    [SerializeField] private AudioClip crouchSFX;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Animator animator;
    
    
    

    #region Fonctions

    
    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        
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
        // Jump
    
        halfHeight = controller.height / 2;
        var bottomPoint = transform.TransformPoint(controller.center - Vector3.up * halfHeight);
        isGrounded = Physics.CheckBox(bottomPoint, new Vector3(0.4f, 0.1f, 0.4f), Quaternion.identity, groundMask | obstacleMask);
        
    
        if (isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f;
        }

        if (!isGrounded)
        {
            fallStart = transform;
            falling = true;
            //Debug.Log("Falling");
            if (!falling)
            {
                Debug.Log("FallEnd");
                fallEnd = transform;
                if (fallStart.position.y - fallEnd.position.y > 5f)
                {
                    playerManager.playerHealth -= 10 * ((int)((fallStart.position.y - fallEnd.position.y) - 5))/10;
                }
            }
        }

        if (isGrounded)
        {
            falling = false;
        }
    
        // Caméra
    
        Vector3 camForward = playerCamera.forward;
        Vector3 camRight = playerCamera.right;
    
        camForward.y = 0;
        camRight.y = 0;
    
        Vector3 forwardRelative = camForward.normalized * horizontalInput.y;
        Vector3 rightRelative = camRight.normalized * horizontalInput.x;
    
        // Mouvements
        Vector3 moveDirection = forwardRelative + rightRelative;

        if (jump)
        {
            if (isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            jump = false;
        }
    
        if (walk)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    
        if (crouch)
        {
            controller.height = crouchHeight;
            speed = 6f;
            playerCamera.localPosition = new Vector3(0, 0.5f, 0);
        }
        else
        {
            controller.height = standingHeight;
            playerCamera.localPosition = new Vector3(0, 0.8f, 0);
        
            if (sprint)
            {
                speed = 24f;
            }
            else
            {
                speed = 12f;
            }
        }
        // Conserver l'inertie en l'air
        if (isGrounded)
        {
            horizontalVelocity = moveDirection * speed; // Met à jour la vitesse horizontale seulement au sol
        }
        // FOV
        
        float fovOffset = (controller.velocity.y < 0f) ? Mathf.Sqrt(Mathf.Abs(controller.velocity.y)) : 0f;
        // Head Bobbing
        if (isGrounded)
        {
            float bobbingRate = sprint ? runBobbingRate : walkBobbingRate;
            float maxBobbingOffset = sprint ? maxRunBobbingOffset : maxWalkBobbingOffset;
            Vector3 targetHeadPosition = Vector3.up * baseCameraHeight + Vector3.up *
                (Mathf.PingPong(Time.time * bobbingRate, maxBobbingOffset) - maxBobbingOffset * 0.5f);
            
            if (walk)
            {
                playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, targetHeadPosition, .1f);
            }
        }

        // Effectuer le mouvement
        controller.Move(horizontalVelocity * Time.deltaTime);
    
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    
    }

    
    public void Walk(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
            walk = true;
            
        }
        else if(context.canceled)
        { 
            walk = false;
            
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
            animator.SetBool("Idle", true);
            animator.SetBool("Running", true);
            sprint = true;
        }
        else if(context.canceled)
        {
            animator.SetBool("Running", false);
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
