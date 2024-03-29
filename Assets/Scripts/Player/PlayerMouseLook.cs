using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[Description("Gère les mouvements de caméra")]   
public class PlayerMouseLook : MonoBehaviour
{
    InputManager inputManager;
    
    [SerializeField] float mouseSensitivityX = 8f;
    [SerializeField] float mouseSensitivityY = 0.5f;
    private float mouseX, mouseY;
    
    [SerializeField] Transform playerCamera;
    [SerializeField] float xClamp = 85f;
    float xRotation = 0f;

    
    
    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        inputManager.mousex.performed += ctx => mouseX = ctx.ReadValue<float>() * mouseSensitivityX;
        inputManager.mousey.performed += ctx => mouseY = ctx.ReadValue<float>() * mouseSensitivityY;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * mouseX * Time.deltaTime);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;
        playerCamera.eulerAngles = targetRotation;
    }

    
}
