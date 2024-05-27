using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DarkMatterObject : MonoBehaviour
{
    [SerializeField] Material darkMatterMaterial;
    [SerializeField] Material defaultMaterial;
    [SerializeField] DarkMatterType darkMatterType;
    public DarkMatterState darkMatterState;

    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private PlayerManager playerManager;
    private bool playerInTrigger = false;
    
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] public GameObject SFX;
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        playerInteraction = FindObjectOfType<PlayerInteraction>();
        SFX.SetActive(false);
    }

    private void Update()
    {
        if (playerInTrigger)
        {
            playerManager.playerHealth -= 1;
        }
        
        if (darkMatterType == DarkMatterType.Wall)
        {
            if (darkMatterState == DarkMatterState.DarkMatter)
            {
                meshRenderer.material = darkMatterMaterial;
                meshCollider.isTrigger = true;
                
                gameObject.layer = 0;
            }
            if (darkMatterState == DarkMatterState.Normal)
            {
                meshRenderer.material = defaultMaterial;
                meshCollider.isTrigger = false;

                gameObject.layer = 7;
            }
        }
        if (darkMatterType == DarkMatterType.Platform)
        {
            if (darkMatterState == DarkMatterState.DarkMatter)
            {
                meshRenderer.material = darkMatterMaterial;
                gameObject.layer = 0;

                meshCollider.isTrigger = true;
            }
            if (darkMatterState == DarkMatterState.Normal)
            {
                meshRenderer.material = defaultMaterial;
                meshCollider.isTrigger = false;
                gameObject.layer = 3;
            }
        }

        if (playerInteraction.avaIsPressed)
        {
            SFX.SetActive(true);
        }
        else
        {
            SFX.SetActive(false);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerManager = other.GetComponent<PlayerManager>();
            playerInTrigger = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerManager = null;
            playerInTrigger = false;
        }
    }

    
}



public enum DarkMatterType
{
    Wall, Platform
}

public enum DarkMatterState
{
    DarkMatter, Normal
}
