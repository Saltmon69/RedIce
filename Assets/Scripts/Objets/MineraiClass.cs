using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using VInspector;
using Random = UnityEngine.Random;


public class MineraiClass : MonoBehaviour
{
    #region Variables

    [Tab("Références")]
    [SerializeField] private List<ItemClass> ressources = new List<ItemClass>();
    [SerializeField] private ItemClass darkMatter;
    [SerializeField] GameObject critGameObject;
    [SerializeField] InventoryManager inventoryManager;
    [SerializeField] private MeshFilter mineraiMesh;
    private PlayerInteraction playerInteraction;
    private Vector3[] mineraiVertices;
    [SerializeField] private MineraiSpawner spawner;
    [SerializeField] private MeshRandomSlicer meshRandomSlicer;
    
    

    [Tab("Valeurs")]
    public float mineraiLife;
    public int critMultiplicator = 1;
    public float quantity = 0;
    public List<GameObject> critPoints = new List<GameObject>();
    
    [Tab("SFX")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip mineraiDestroyedSFX;

    [Tab("UI")] 
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject quantityFbPrefab;
    
    
    #endregion

    #region Fonctions
    private void Awake()
    {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        mineraiVertices = mineraiMesh.mesh.vertices;
        spawner = GetComponentInParent<MineraiSpawner>();

    }

    private void OnEnable()
    {
        CritPointCreation();
    }

    private void FixedUpdate()
    {
        try
        {
            GameObject player = Physics.OverlapSphere(transform.position, 10f).Where(x => x.CompareTag("Player")).FirstOrDefault().gameObject;
            if (player != null)
            {
                playerInteraction = player.GetComponent<PlayerInteraction>();
            }
        }catch(NullReferenceException){}
        
        
    }
    
    public void takeDamage(float damage)
    {
        mineraiLife -= damage * critMultiplicator;
        quantity += damage * critMultiplicator;
        if (mineraiLife <= 0)
        {
            audioSource.PlayOneShot(mineraiDestroyedSFX);
            DestroyGameObject();
        }
    }
    
    public void DestroyGameObject()
    {
        meshRandomSlicer.DestroyMesh();
        spawner.activeMinerai = null;
        Destroy(gameObject, 0.5f);
    }
    
    /// <summary>
    /// Le but de cette fonction est de calculer la quantité de ressources que le joueur va recevoir en fonction de la quantité de dégâts infligés au minerai.
    /// </summary>
    /// <param name="quantity"> La quantité de dégâts infligés</param>
    /// <returns>La fonction a fini de s'exécuter</returns>
    public bool QuantityCalculator(float quantity)
    {
        bool ended = false;

        foreach (var ressource in ressources)
        {
            for(float i = 0; i < quantity * ressource.rendement; i++)
            {
                inventoryManager.AddItem(ressource);
                InstantiateFeedback(ressource.sprite, (int)(quantity * ressource.rendement));
            }
        }
        
        for(float i = 0; i < quantity * darkMatter.rendement; i++)
        {
            inventoryManager.AddItem(darkMatter);
            InstantiateFeedback(darkMatter.sprite, (int)(quantity * darkMatter.rendement));
        }

        ended = true;

        return ended;
    }
    
    /// <summary>
    /// Permet la création de point critique de façon aléatoire sur le minerai. 
    /// </summary>
    [Button("CritPointCreation")]
    public void CritPointCreation()
    {
        int critPointNumber = Random.Range(1, 6);
        
        for (int i = 0; i < critPointNumber; i++)
        {
            Vector3 selectedVertices = mineraiVertices[Random.Range(0, mineraiVertices.Length)];
            critPoints.Add(Instantiate(critGameObject, transform.TransformPoint(selectedVertices), Quaternion.identity, transform));
        }
    }

    private void InstantiateFeedback(Sprite sprite, int quantity)
    {
        GameObject feedback = Instantiate(quantityFbPrefab, UI.transform);
        feedback.GetComponentInChildren<Image>().sprite = sprite;
        feedback.GetComponentInChildren<Text>().text = "x" + quantity;
    }


    
    #endregion
}
