using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Description("Gère les déplacements du joueur")]
public class PlayerMovement : MonoBehaviour
{
    #pragma warning disable 0649

    #region Variables

    // Movements
    
    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 12f;
    Vector2 horizontalInput;
    bool sprint;

    // Jump
    
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] LayerMask groundMask;
    Vector3 verticalVelocity = Vector3.zero;
    bool isGrounded;
    bool jump;
    float halfHeight;
    
    // Crouch
    
    [SerializeField] Transform playerCamera;
    private bool crouch;
    
    #endregion

    #region Fonctions
    private void Update()
    {
        //Jump
        
        halfHeight = controller.height / 2;
        var bottomPoint = transform.TransformPoint(controller.center - Vector3.up * halfHeight);
        isGrounded = Physics.CheckSphere(bottomPoint, 0.1f, groundMask); // Sert à vérifir si le joueur touche le sol.
        
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
        
        Vector3 moveDirection = forwardRelative + rightRelative;
        
        //Mouvements
        
        Vector3 horizontalVelocity = (moveDirection) * speed;
        controller.Move(horizontalVelocity * Time.deltaTime);
        
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
        
        if (crouch)
        {
            speed = 6f;
            playerCamera.localPosition = new Vector3(0, halfHeight, 0);
            crouch = false;
        }
        if (sprint)
        {
            speed = 24f;
            sprint = false;
        }
        if (!crouch)
        {
            speed = 12f;
            playerCamera.localPosition = new Vector3(0, 0.9f, 0);
        }
        if (!sprint)
        {
            speed = 12f;
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
    
    public void OnCrouchPressed()
    {
        crouch = true;
    }
    
    #endregion
}
