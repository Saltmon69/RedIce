using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.VFX;
using VInspector;

[Description("Gère les interactions du joueur")]
public class PlayerInteraction : MonoBehaviour
{
    #region Variables

    [SerializeField] InputManager inputManager;
    
    [Tab("Références")]
    [SerializeField] Camera playerCamera;
    [SerializeField] GameObject ava;
    [SerializeField] private Camera mainCamera;
    public PlayerManager playerManager;
    public PlayerMenuing playerMenuing;
    public GameObject radialMenu;
    
    [Tab("Minage")]
    [SerializeField] MineraiClass mineraiClass;
    public bool isApplyingDamage = false;
    public float damage;
    public bool isMiningModeActive;
    
    [Tab("Raycast")]
    [SerializeField] float interactionRange;
    RaycastHit itemHit;
    
    [Tab("Ping")]
    public bool pingIsPressed;
    
    [Tab("Lunette AVA")]
    [SerializeField] GameObject darkMatterBullet;
    [SerializeField] PlayerModeSelect playerModeSelect;
    public bool avaIsPressed;
    
    [Tab("SFX")]
    [SerializeField] private AudioSource laserAudioSource;
    
    [Tab("VFX")]
    [SerializeField] private GameObject instantiatedLaserVFX;
    [SerializeField] private GameObject laserVFX;
    [SerializeField] private GameObject laserImpactVFX;

    #endregion

    #region Fonctions

    private void Awake()
    {
        

        inputManager.interact.performed += OnInteraction;
        
        inputManager.leftClick.performed += OnLeftClick;
        inputManager.leftClick.canceled += OnLeftClick;
        
        inputManager.ping.performed += OnPing;
        inputManager.ping.canceled += OnPing;
        
        inputManager.ava.performed += OnAva;
        inputManager.ava.canceled += OnAva;
        
        inputManager.shoot.performed += OnShoot;
    }


    private void FixedUpdate()
    {
        if (isApplyingDamage)
        {
            VisualEffect effect = laserImpactVFX.GetComponent<VisualEffect>();
            RaycastMaker(interactionRange);
            effect.Play();
            if(instantiatedLaserVFX == null)
                instantiatedLaserVFX = Instantiate(laserImpactVFX, itemHit.point, Quaternion.identity);
            else
                instantiatedLaserVFX.transform.position = itemHit.point;
            
            if (itemHit.collider != null)
            {
                switch (itemHit.collider.tag)
                {
                    case "MineraiCrit":
                        if (mineraiClass != itemHit.collider.GetComponentInParent<MineraiClass>())
                            mineraiClass = itemHit.collider.GetComponentInParent<MineraiClass>();
                        mineraiClass.critMultiplicator = 2;
                        mineraiClass.takeDamage(damage);
                        mineraiClass.critPoints.Remove(itemHit.collider.gameObject);
                        Destroy(itemHit.collider.gameObject);
                        break;
                    case "Minerai":
                        if(mineraiClass != itemHit.collider.GetComponent<MineraiClass>())
                            mineraiClass = itemHit.collider.GetComponent<MineraiClass>();
                        mineraiClass.critMultiplicator = 1;
                        mineraiClass.takeDamage(damage);
                        break;
                }
            }
        }
        else
        {
            if(instantiatedLaserVFX != null)
                Destroy(instantiatedLaserVFX);
            if (mineraiClass != null)
            {
                bool ended = mineraiClass.QuantityCalculator(mineraiClass.quantity);
                if (ended)
                {
                    mineraiClass.quantity = 0;
                }
            }
                
            
        }
        
        if (mineraiClass != null)
        {
            if (mineraiClass.mineraiLife <= 0)
            {
                
                isApplyingDamage = false;
                mineraiClass = null;
            }
        }
        
    }
    
    public void OnInteraction(InputAction.CallbackContext context)
    {
        RaycastMaker(interactionRange);
    }
    
    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(isMiningModeActive)
            {
                laserVFX.SetActive(true);
                isApplyingDamage = true;
                laserAudioSource.Play();
            }
        }
        if(context.canceled)
        {
            isApplyingDamage = false;
            laserAudioSource.Stop();
            laserVFX.SetActive(false);
        }
    }
    public void OnPing(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            pingIsPressed = true;
            playerMenuing.inMenu = true;
            radialMenu.SetActive(true);
        }
        else if(context.canceled)
        {
            pingIsPressed = false;
            playerMenuing.inMenu = false;
            radialMenu.SetActive(false);
        }
    }
    
    public void OnAva(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (playerModeSelect.modeSelected == 1)
            {
                avaIsPressed = true;
                ava.SetActive(true);
            }
        }
        else if(context.canceled)
        {
            avaIsPressed = false;
            ava.SetActive(false);
        }
    }
    public void OnShoot(InputAction.CallbackContext context)
    {
        if(avaIsPressed)
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
