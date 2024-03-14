using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Ressource", menuName = "Objet/Ressource", order = 2)]
[Description("Classe de base pour les ressources et autres objets. C'est un SO (ScriptableObject) qui contient toutes les informations de base d'un objet.")]

public class ItemClass : ScriptableObject
{
    [Tooltip("Nom de la ressource")]
    public string nom;
    [Tooltip("Description de la ressource")]
    public string description;
    [Tooltip("Sprite de la ressource")]
    public Sprite sprite;
    [Tooltip("Taille de la pile")]
    public int stackSize;
    [Tooltip("Prefab de la ressource")]
    public GameObject prefab;
    [Tooltip("Rendement de la ressource")]
    public float rendement;
    [Tooltip("L'objet est utilisable")]
    public bool isUsable; 
    [Tooltip("La masse atomique de l'objet")]  
    public int atomicMass;
}

