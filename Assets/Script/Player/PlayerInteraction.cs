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
    #region Variables

    // Références
    [SerializeField] Camera playerCamera;
    public PlayerManager playerManager;
    public GameObject radialMenu;
    
    
    // Minages
    MineraiClass mineraiClass;
    bool hasAppliedDamage = false;
    public int damage;
    
    // Raycast
    [SerializeField] float interactionRange;
    RaycastHit itemHit;
    

    #endregion

    #region Fonctions
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
                            mineraiClass.critMultiplicator = 2;
                            mineraiClass.takeDamage(damage);
                            hasAppliedDamage = true;
                            Debug.Log(mineraiClass.mineraiLife);
                            break;
                        case"Minerai":
                            mineraiClass.critMultiplicator = 1;
                            mineraiClass.takeDamage(damage);
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
        Cursor.lockState = CursorLockMode.None;
        radialMenu.SetActive(true);
        
    }

    public void RaycastMaker(float range)
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, range);
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red);
        itemHit = hit;

    }

    

    #endregion
}
