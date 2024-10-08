using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public class MineraiClass : MonoBehaviour
{
    #region Variables

    [SerializeField] private ItemClass mineraiClass;

    public int mineraiLife;

    public int critMultiplicator = 1;
    
    [SerializeField] GameObject critGameObject;
    
    #endregion

    #region Fonctions
    private void Start()
    {
        CritPointCreation();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            CritPointCreation();
        }
    }

    
    public void takeDamage(int damage)
    {
        if (damage > mineraiLife)
        {
            damage = mineraiLife;
        }
        //mineraiClass.quantite += (int)(damage * critMultiplicator * mineraiClass.rendement);  Ligne à adapter au nouveau fonctionnement
        mineraiLife -= damage * critMultiplicator;
    }

    public void DestroyGameObject()
    {
        //Mettre gamefeel
        Destroy(gameObject);
    }
    
    /// <summary>
    /// Permet la création de point critique de façon aléatoire sur le minerai. 
    /// </summary>
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
