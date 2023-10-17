using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    RaycastHit itemHit;
    [SerializeField] float interactionRange;
    MineraiClass mineraiClass;
    bool hasAppliedDamage = false;
    

    private void Start()
    {

        
    }


    public void OnInteractionPressed()
    {
        
        RaycastMaker();
        

    }
    
    private void FixedUpdate()
    {
        
        if (itemHit.collider != null)
        {
            if (itemHit.collider.CompareTag("Machine"))
            {
                Debug.Log(itemHit.collider.name);
            }

            if (itemHit.collider.CompareTag("Minerai") || itemHit.collider.CompareTag("MineraiCrit"))
            {
                mineraiClass = itemHit.collider.GetComponentInParent<MineraiClass>();

                if (hasAppliedDamage == false)
                {
                    switch (itemHit.collider.tag)
                    {
                        case"MineraiCrit":
                            mineraiClass.takeDamage(9);
                            hasAppliedDamage = true;
                            Debug.Log(mineraiClass.mineraiLife);
                            break;
                        case"Minerai":
                            mineraiClass.takeDamage(3);
                            hasAppliedDamage = true;
                            Debug.Log(mineraiClass.mineraiLife);
                            break;
                    }
                }
            }
        }
    }
    
    public void OnUsePressed()
    {
        hasAppliedDamage = false;
        RaycastMaker();
        
    }

    private void RaycastMaker()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, interactionRange);
        Debug.DrawRay(ray.origin, ray.direction * interactionRange, Color.red);
        itemHit = hit;

    }
}
