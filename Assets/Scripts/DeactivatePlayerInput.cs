using System;
using System.Collections.Generic;
using UnityEngine;

public class DeactivatePlayerInput : MonoBehaviour
{
    public List<MonoBehaviour> playerComponents;
    public bool isDeactivated;
    public GameObject menu;
    public bool isSwitchActive;

    public void OnEnable()
    {
        if (isSwitchActive)
        {
            Deactivate();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void OnDisable()
    {
        if (isSwitchActive)
        {
            Activate();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void Deactivate()
    {
        for (var i = 0; i < playerComponents.Count; i++)
        {
            playerComponents[i].enabled = false;
        }

        isDeactivated = true;
    }
    
    public void SoftDeactivate()
    {
        for (var i = 0; i < playerComponents.Count; i++)
        {
            if(i <= 2) continue;
            playerComponents[i].enabled = false;
        }

        isDeactivated = true;
    }

    public void Update()
    {
        if (isDeactivated && Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void Activate()
    {
        for (var i = 0; i < playerComponents.Count; i++)
        {
            playerComponents[i].enabled = true;
        }
        playerComponents[0].gameObject.GetComponent<PlayerMenuing>().OnQuitPressed();
        isDeactivated = false;
    }
}
