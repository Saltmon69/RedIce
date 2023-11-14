using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[Description("Gère les mouvements de caméra")]   
public class PlayerMouseLook : MonoBehaviour
{
    
    [SerializeField] float mouseSensitivityX = 8f;
    [SerializeField] float mouseSensitivityY = 0.5f;
    private float mouseX, mouseY;
    
    [SerializeField] Transform playerCamera;
    [SerializeField] float xClamp = 85f;
    float xRotation = 0f;


    private void Update()
    {
        transform.Rotate(Vector3.up * mouseX * Time.deltaTime);
        
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;
        playerCamera.eulerAngles = targetRotation;
    }

    public void ReceiveInput(Vector2 mouseInput)
    {
        mouseX = mouseInput.x * mouseSensitivityX;
        mouseY = mouseInput.y * mouseSensitivityY;
    }
}
