using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VInspector;

[Description("Gère les interactions du joueur")]
public class PlayerInteraction : MonoBehaviour
{
    #region Variables

    
    [Tab("Références")]
    [SerializeField] Camera playerCamera;
    public PlayerManager playerManager;
    public PlayerMenuing playerMenuing;
    public GameObject radialMenu;
    [SerializeField] GameObject ava;
    
    
    
    [Tab("Minage")]
    [SerializeField] MineraiClass mineraiClass;
    public bool isApplyingDamage = false;
    public int damage;
    
    // Raycast
    [Tab("Raycast")]
    [SerializeField] float interactionRange;
    RaycastHit itemHit;
    
    // Ping
    [Tab("Ping")]
    public bool pingIsPressed;
    
    // Lunette AVA
    [Tab("Lunette AVA")]
    public bool avaIsPressed;
    [SerializeField] GameObject darkMatterBullet;
    
    

    #endregion

    #region Fonctions
    
    public void OnInteractionPressed()
    {
        RaycastMaker(interactionRange);
        
    }
    
    private void FixedUpdate()
    {
        if (avaIsPressed)
        {
            ava.SetActive(true);
        }
        else
        {
            ava.SetActive(false);
        }
        
        if (itemHit.collider != null)
        {
            if (itemHit.collider.CompareTag("Machine"))
            {
                Debug.Log(itemHit.collider.name);
            }

            if (itemHit.collider.CompareTag("Minerai") || itemHit.collider.CompareTag("MineraiCrit"))
            {
                mineraiClass = itemHit.collider.GetComponentInParent<MineraiClass>();

                if (isApplyingDamage == true)
                {
                    switch (itemHit.collider.tag)
                    {
                        case"MineraiCrit":
                            mineraiClass.critMultiplicator = 2;
                            mineraiClass.takeDamage(damage);
                            Debug.Log(mineraiClass.mineraiLife);
                            break;
                        case"Minerai":
                            mineraiClass.critMultiplicator = 1;
                            mineraiClass.takeDamage(damage);
                            Debug.Log(mineraiClass.mineraiLife);
                            break;
                    }
                }
            }
            
        }
        
    }
    
    public void OnUsePressed()
    {
        isApplyingDamage = true;
        RaycastMaker(interactionRange);
    }
    
    public void OnUseReleased()
    {
        isApplyingDamage = false;
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
    
    public void OnAvaPressed()
    {
        if (avaIsPressed)
        {
            avaIsPressed = false;
        }
        else
        {
            avaIsPressed = true;
        }
        
    }
    
    public void OnShootPressed()
    {
        Instantiate(darkMatterBullet, playerCamera.transform.position, playerCamera.transform.rotation);
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
