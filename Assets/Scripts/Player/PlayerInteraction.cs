using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
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

    InputManager inputManager;
    
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
    public VisualEffect vfxLaser;
    
    [Tab("Raycast")]
    [SerializeField] float interactionRange;
    RaycastHit itemHit;
    
    [Tab("Ping")]
    public bool pingIsPressed;
    
    [Tab("Lunette AVA")]
    [SerializeField] GameObject darkMatterBullet;
    public bool avaIsPressed;
    
    [Tab("SFX")]
    [SerializeField] private GameObject sfxObject = null;
    [SerializeField] private AudioClip laserSFX;
    

    #endregion

    #region Fonctions

    private void Awake()
    {
        inputManager = InputManager.instance;

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
        if (isMiningModeActive)
        {
            if (itemHit.collider != null)
            {
                if (itemHit.collider.CompareTag("Minerai") || itemHit.collider.CompareTag("MineraiCrit"))
                {
                    if(mineraiClass == null)
                        if (itemHit.collider.CompareTag("MineraiCrit"))
                            mineraiClass = itemHit.collider.GetComponentInParent<MineraiClass>();
                        else
                            mineraiClass = itemHit.collider.GetComponent<MineraiClass>();
                    else
                    {
                        if (isApplyingDamage)
                        {
                            switch (itemHit.collider.tag)
                            {
                                case"MineraiCrit":
                                    mineraiClass.critMultiplicator = 2;
                                    mineraiClass.takeDamage(damage);
                                    Destroy(itemHit.collider.gameObject);
                                    break;
                                case"Minerai":
                                    mineraiClass.critMultiplicator = 1;
                                    mineraiClass.takeDamage(damage);
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    isApplyingDamage = false;
                }
            }
        }
        else
        {
            if (itemHit.collider != null)
            {
                if (itemHit.collider.CompareTag("EON"))
                {
                    itemHit.collider.GetComponent<ChestUIDisplay>().ActivateUIDisplay();
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        itemHit.collider.GetComponent<ChestUIDisplay>().DeactivateUIDisplay();
                    }
                }
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
                if (sfxObject == null)
                { 
                    SFXManager.instance.PlaySFX(laserSFX, transform, 0.5f, true);
                    sfxObject = SFXManager.instance.InstantiatedSFXObject.gameObject;
                }
                
                
                isApplyingDamage = true;
                RaycastMaker(interactionRange);
            }
        }
        if(context.canceled)
        {
            Destroy(sfxObject);
            isApplyingDamage = false;
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
            avaIsPressed = true;
            ava.SetActive(true);
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
