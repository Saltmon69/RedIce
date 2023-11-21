using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Description("Gère les interactions du joueur")]
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    RaycastHit itemHit;
    [SerializeField] float interactionRange;
    MineraiClass mineraiClass;
    bool hasAppliedDamage = false;
    [SerializeField] GameObject pingPrefab;
    public PlayerManager playerManager;
    
    public void OnInteractionPressed()
    {
        RaycastMaker(interactionRange);
        
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
        RaycastMaker(interactionRange);
        
    }
    
    public void OnPingPressed()
    {
        RaycastMaker(100f);
        if(itemHit.collider == null || itemHit.collider.CompareTag("Obstacle") || itemHit.collider.CompareTag("Ground") || itemHit.collider.CompareTag("Player") || itemHit.collider.CompareTag("Minerai"))
        {
            playerManager.data.ping = Instantiate(pingPrefab, itemHit.point, Quaternion.identity);
            
            if (itemHit.collider.CompareTag("Minerai"))
            {
                playerManager.data.itemPinged = itemHit.collider.gameObject;
            }
            else
            {
                playerManager.data.itemPinged = null;
            }
        }
        
    }

    private void RaycastMaker(float range)
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, range);
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red);
        itemHit = hit;

    }
}
