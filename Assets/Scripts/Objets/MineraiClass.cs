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
    public int critPointQuantity = 6;
    public List<GameObject> critPoints = new List<GameObject>();
    
    
    [Tab("SFX")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip mineraiDestroyedSFX;

    [Tab("UI")]
    [SerializeField] Canvas canvas;
    [SerializeField] public GameObject image;
    [SerializeField] private GameObject UI;
    [SerializeField] public GameObject grid;
    [SerializeField] private GameObject quantityFbPrefab;
    private List<GameObject> feedbacks = new List<GameObject>();
    
    
    [Tab("Feedback")]
    [SerializeField] private GameObject holographicMaterial;
    private float timerVsisible = 0f;
    private float activationCooldown = 1.5f;
    private float deactivationCooldown = 10f;
    [HideInInspector] public bool detected;
    private ItemClass feedbackItem;
    private int feedbackQuantity;
    
    #endregion

    #region Fonctions
    private void Awake()
    {
        canvas.worldCamera = GameObject.Find("UICamera").GetComponent<Camera>();
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        mineraiVertices = mineraiMesh.mesh.vertices;
        spawner = GetComponentInParent<MineraiSpawner>();
        playerInteraction = GameObject.Find("Player").GetComponent<PlayerInteraction>();
        grid = GameObject.Find("InventoryFBGrid");

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

        
        if (detected)
        {
            
            timerVsisible += Time.deltaTime;
            if (timerVsisible >= activationCooldown)
            {
                holographicMaterial.SetActive(true);
                timerVsisible = 0;
            }
            
            if (Vector3.Distance(transform.position, playerInteraction.transform.position) <= playerInteraction.interactionRange * 2f)
            {
                //image.transform.LookAt(playerInteraction.transform);
                image.SetActive(true);
                holographicMaterial.SetActive(false);
            }
            else
            {
                image.SetActive(false);
            }
        }
        else
        {
            timerVsisible += Time.deltaTime;
            if (timerVsisible >= deactivationCooldown)
            {
                image.SetActive(false);
                holographicMaterial.SetActive(false);
                timerVsisible = 0;
            }
            image.SetActive(false);
            holographicMaterial.SetActive(false);
        }

        if (feedbacks.Count > 0)
        {
            Destroy(feedbacks[0], 2f);
        }
    }
    
    public void takeDamage(float damage)
    {
        mineraiLife -= damage * critMultiplicator;
        quantity += damage * critMultiplicator;
        if (mineraiLife <= 0)
        {
            bool ended = QuantityCalculator(quantity);
            if (ended)
            {
                audioSource.PlayOneShot(mineraiDestroyedSFX);
                DestroyGameObject();
            }
            
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

        Debug.Log("Quantity : " + quantity);
        foreach (var ressource in ressources)
        {
            for(float i = 0; i < quantity * ressource.rendement; i++)
            {
                inventoryManager.AddItem(ressource);
            }
        }
        
        for(float i = 0; i < quantity * darkMatter.rendement; i++)
        {
            inventoryManager.AddItem(darkMatter);
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
        int critPointNumber = Random.Range(3, critPointQuantity);
        
        for (int i = 0; i < critPointNumber; i++)
        {
            Vector3 selectedVertices = mineraiVertices[Random.Range(0, mineraiVertices.Length)];
            critPoints.Add(Instantiate(critGameObject, transform.TransformPoint(selectedVertices), Quaternion.identity, transform));
        }
    }

    private void InstantiateFeedback(Sprite sprite, int quantity)
    {
        if (feedbacks.Count < 3)
        {
            GameObject feedback = Instantiate(quantityFbPrefab, grid.transform);
            feedback.GetComponentInChildren<Image>().sprite = sprite;
            feedback.GetComponentInChildren<Text>().text = "x" + quantity;
        }
        else
        {
            Destroy(feedbacks[0]);
            feedbacks.RemoveAt(0);
            GameObject feedback = Instantiate(quantityFbPrefab, grid.transform);
            feedback.GetComponentInChildren<Image>().sprite = sprite;
            feedback.GetComponentInChildren<Text>().text = "x" + quantity;
        }
    }

    private void OnBecameVisible()
    {
        detected = true;
    }

    private void OnBecameInvisible()
    {
        timerVsisible = 0;
        detected = false;
    }

    #endregion
}
