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
    public PlayerMenuing playerMenuing;
    public GameObject radialMenu;
    
    
    // Minages
    MineraiClass mineraiClass;
    bool hasAppliedDamage = false;
    public int damage;
    
    // Raycast
    [SerializeField] float interactionRange;
    RaycastHit itemHit;
    
    // Ping
    [HideInInspector]public bool pingIsPressed;
    

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
        pingIsPressed = true;
        playerMenuing.inMenu = true;
        radialMenu.SetActive(true);
        
    }
    
    public void OnPingReleased()
    {
        pingIsPressed = false;
        playerMenuing.inMenu = false;
        radialMenu.SetActive(false);
        
    }
    /// <summary>
    /// Permet de créer un raycast à partir de la position de la souris.
    /// </summary>
    /// <param name="range">taille du raycast</param>
    /// <returns>Retourne l'objet touché et sa position</returns>
    public RaycastHit RaycastMaker(float range)
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        ray.direction = playerCamera.transform.forward;
        Physics.Raycast(ray, out RaycastHit hit, range);
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red);
        itemHit = hit;
        return hit;
    }

    

    #endregion
}
