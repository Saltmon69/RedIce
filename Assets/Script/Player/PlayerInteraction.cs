using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    RaycastHit itemHit;
    
    private void Update()
    {
        Physics.Raycast(playerCamera.transform.position, transform.forward, out RaycastHit hit, 2f);
        itemHit = hit;
        
    }

    public void OnInteractionPressed()
    {
        if (itemHit.collider != null)
        {
            Debug.Log(itemHit.collider.name);
        }
        else
        {
            Debug.Log("Rien");
        }
        
    }
}
