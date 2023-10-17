using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class MineraiClass : MonoBehaviour
{
    [SerializeField] private RessourceClass mineraiClass;

    public int mineraiLife;

    public int critMultiplicator = 3;
    
    
    
    public void takeDamage(int damage)
    {
        if (damage > mineraiLife)
        {
            damage = mineraiLife;
        }
        mineraiClass.quantite += (int)(damage*mineraiClass.rendement);
        mineraiLife -= damage;
    }

    public void DestroyGameObject()
    {
        //Mettre gamefeel
        Destroy(gameObject);
    }
}
