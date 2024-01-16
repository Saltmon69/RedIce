using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
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
    [HideInInspector] private PlayerInteraction playerInteraction;

    [Tab("Valeurs")]
    public int mineraiLife;
    public int critMultiplicator = 1;
    
    
    #endregion

    #region Fonctions
    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
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
        mineraiLife -= (int)damage * critMultiplicator;
        float quantity = damage * critMultiplicator;

        if (mineraiLife <= 0)
        {
            bool ended = QuantityCalculator(quantity);
            if (ended)
                DestroyGameObject();
        }
        else if(playerInteraction.isApplyingDamage == false)
        {
            bool ended;
            ended = QuantityCalculator(quantity);
        }
    }

    
    
    public void DestroyGameObject()
    {
        //Mettre gamefeel
        Destroy(gameObject);
    }
    

    /// <summary>
    /// Le but de cette fonction est de calculer la quantité de ressources que le joueur va recevoir en fonction de la quantité de dégâts infligés au minerai.
    /// </summary>
    /// <param name="quantity"> La quantité de dégâts infligés</param>
    /// <returns>La fonction a fini de s'exécuter</returns>
    private bool QuantityCalculator(float quantity)
    {
        bool ended = false;

        foreach (var ressource in ressources)
        {
            for(int i = 0; i < (int)quantity * ressource.rendement; i++)
            {
                inventoryManager.AddItem(ressource);
            }
        }
        
        for(int i = 0; i < (int)quantity * darkMatter.rendement; i++)
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
        Debug.Log("CritPointCreation");
        
        int critPointNumber = Random.Range(1, 6);
        
        for (int i = 0; i < critPointNumber; i++)
        {
            float rdmNegPos = Random.Range(-0.60f, -0.65f); //Valeur négative
            float rdmPosPos = Random.Range(0.60f, 0.65f); //Valeur positive
            GameObject critPoint = Instantiate(critGameObject, transform);
            critPoint.transform.localPosition = new Vector3(Random.Range(rdmNegPos, rdmPosPos), Random.Range(rdmNegPos, rdmPosPos), Random.Range(rdmNegPos, rdmPosPos));
            
        }
    }
    
    #endregion
}
