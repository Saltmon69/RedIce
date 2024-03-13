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

    #region Variables
    
    // Movements
    [Tab("Mouvements")]
    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 12f;
    Vector2 horizontalInput;
    bool sprint = false;
    bool walk = false;

    // Jump
    [Tab("Saut")]
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] LayerMask groundMask;
    Vector3 verticalVelocity = Vector3.zero;
    bool isGrounded;
    bool jump;
    float halfHeight;
    
    // Crouch
    [Tab("Crouch")]
    [SerializeField] Transform playerCamera;
    private bool crouch;
    
    [Tab("SFX")]
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip landSFX;
    [SerializeField] private AudioClip walkSFX;
    [SerializeField] private AudioClip runSFX;
    [SerializeField] private AudioClip crouchSFX;
    [SerializeField] private GameObject sfxObject = null;
    #endregion

    #region Fonctions

  
    private void Update()
    {
        //Jump
        
        halfHeight = controller.height / 2;
        var bottomPoint = transform.TransformPoint(controller.center - Vector3.up * halfHeight);
        isGrounded = Physics.CheckSphere(bottomPoint, 1f, groundMask); // Sert à vérifir si le joueur touche le sol.
        
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

        if (walk)
        {
            if(sfxObject == null)
            {
                SFXManager.instance.PlaySFX(walkSFX, transform, 0.5f, false);
                sfxObject = SFXManager.instance.InstantiatedSFXObject.gameObject;
            }
        }
        else
        {
            Destroy(sfxObject);
        }
        
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
        
        
        
        if (crouch)
        {
            controller.height = 1f;
            speed = 6f;
            transform.localScale = new Vector3(1, 0.5f, 1);
            
        }
        if (!crouch)
        {
            controller.height = 2f;
            speed = 12f;
            transform.localScale = new Vector3(1, 1, 1);
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

    public void ReceiveInput (Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
        
    }
    
    public void OnJumpPressed()
    {
        jump = true;
    }
    
    public void OnSprintPressed()
    {
        sprint = true;
    }

    public void OnSprintReleased()
    {
        sprint = false;
    }
    
    public void OnCrouchPressed()
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

    public void OnMovePressed()
    {
        if (walk == false)
            walk = true;
        else
            walk = false;
    }
    
   
    #endregion
}
