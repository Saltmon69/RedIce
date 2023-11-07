using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public class MineraiClass : MonoBehaviour
{
    [SerializeField] private ItemClass mineraiClass;

    public int mineraiLife;

    public int critMultiplicator = 1;
    
    [SerializeField] GameObject critGameObject;

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
        //mineraiClass.quantite += (int)(damage * critMultiplicator * mineraiClass.rendement);
        mineraiLife -= damage * critMultiplicator;
    }

    public void DestroyGameObject()
    {
        //Mettre gamefeel
        Destroy(gameObject);
    }
    
    public void CritPointCreation()
    {
        Debug.Log("CritPointCreation");
        
        int critPointNumber = Random.Range(1, 6);
        for (int i = 0; i < critPointNumber; i++)
        {
            float rdmNegPos = Random.Range(-0.60f, -0.65f);
            float rdmPosPos = Random.Range(0.60f, 0.65f);
            GameObject critPoint = Instantiate(critGameObject, transform);
            critPoint.transform.localPosition = new Vector3(Random.Range(rdmNegPos, rdmPosPos), Random.Range(rdmNegPos, rdmPosPos), Random.Range(rdmNegPos, rdmPosPos));
            
        }
    }
}
