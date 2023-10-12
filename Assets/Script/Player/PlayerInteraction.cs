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
    RaycastHit itemHit;
    [SerializeField] float interactionRange;
    
    

    public void OnInteractionPressed()
    {
        RaycastMaker();
    }
    
    private void Update()
    {
        RaycastMaker();
        Vector3 aimpoint = playerCamera.transform.position + playerCamera.transform.forward * 3;
        Debug.DrawRay(playerCamera.transform.position, playerCamera.ScreenToViewportPoint(aimpoint), Color.black);
        if (itemHit.collider != null)
        {
            if (itemHit.collider.CompareTag("Machine"))
            {
                Debug.Log(itemHit.collider.name);
            }

            if (itemHit.collider.CompareTag("Minerai") || itemHit.collider.CompareTag("MineraiCrit"))
            {
                switch (itemHit.collider.tag)
                {
                    case"MineraiCrit":
                        Debug.Log("crit");
                        break;
                    default:
                        Debug.Log("minerai de base");
                        break;
                }
            }
            else
            {
                Debug.Log("Rien");
            }
        }
    }
    
    public void OnUsePressed()
    {
        RaycastMaker();
        Debug.Log("Click gauche");
    }

    private void RaycastMaker()
    {
        Physics.Raycast(playerCamera.transform.position, transform.forward + transform.up, out RaycastHit hit, interactionRange);
        itemHit = hit;
        
    }
}
