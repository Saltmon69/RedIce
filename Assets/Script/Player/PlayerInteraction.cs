using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] RaycastHit itemHit;
    [SerializeField] float interactionRange;
    [SerializeField] GameObject itemInHand;

    public void OnInteractionPressed()
    {
        Physics.Raycast(playerCamera.transform.position, transform.forward, out RaycastHit hit, interactionRange);
        itemHit = hit;
        
        if (itemHit.collider.CompareTag("Machine"))
        {
            Debug.Log(itemHit.collider.name);
        }

        if (itemHit.collider.CompareTag("Minerai"))
        {
            switch (itemHit.collider.name)
            {
                case "crit":
                    Debug.Log("crit");
                    break;
                default:
                    Debug.Log("minerai de base");
                    break;
            }
        }
    }
    
}
