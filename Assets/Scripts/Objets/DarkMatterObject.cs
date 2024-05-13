using System;
using System.Collections;
using System.Collections.Generic;
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
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        playerInteraction = FindObjectOfType<PlayerInteraction>();
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
<<<<<<< HEAD
<<<<<<< HEAD
                boxCollider.isTrigger = true;
=======
                meshCollider.isTrigger = true;
>>>>>>> origin/Artiste
=======
                meshCollider.isTrigger = true;
>>>>>>> Louis
                gameObject.layer = 0;
            }
            if (darkMatterState == DarkMatterState.Normal)
            {
                meshRenderer.material = defaultMaterial;
<<<<<<< HEAD
<<<<<<< HEAD
                boxCollider.isTrigger = false;
=======
                meshCollider.isTrigger = false;
>>>>>>> origin/Artiste
=======
                meshCollider.isTrigger = false;
>>>>>>> Louis
                gameObject.layer = 7;
            }
        }
        if (darkMatterType == DarkMatterType.Platform)
        {
            if (darkMatterState == DarkMatterState.DarkMatter)
            {
                meshRenderer.material = darkMatterMaterial;
                gameObject.layer = 0;
<<<<<<< HEAD
<<<<<<< HEAD
                boxCollider.isTrigger = true;
=======
                meshCollider.isTrigger = true;
>>>>>>> origin/Artiste
=======
                meshCollider.isTrigger = true;
>>>>>>> Louis
            }
            if (darkMatterState == DarkMatterState.Normal)
            {
                meshRenderer.material = defaultMaterial;
<<<<<<< HEAD
<<<<<<< HEAD
                boxCollider.isTrigger = false;
=======
                meshCollider.isTrigger = false;
>>>>>>> origin/Artiste
=======
                meshCollider.isTrigger = false;
>>>>>>> Louis
                gameObject.layer = 3;
            }
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
