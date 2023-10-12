using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class MineraiClass : MonoBehaviour
{
    [SerializeField] private RessourceClass mineraiClass;

    public int mineraiLife;
    
    public void takeDamage(int damage)
    {
        if (damage > mineraiLife)
        {
            damage = mineraiLife;
        }
        mineraiLife -= damage;
        mineraiClass.quantite += (int)(damage*mineraiClass.rendement);
        
        if (mineraiLife <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void DestroyGameObject()
    {
        //Mettre gamefeel
        Destroy(gameObject);
    }
}
