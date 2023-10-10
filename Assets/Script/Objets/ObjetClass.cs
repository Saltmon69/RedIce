using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Objet", menuName = "Objet", order = 1)]

public class ObjetClass : ScriptableObject
{
    [Tooltip("Nom de l'objet")]
    public string nom;
    [Tooltip("Description de l'objet")]
    public string description;
    [Tooltip("Sprite de l'objet")]
    public Sprite sprite;
    [Tooltip("Quantité de l'objet")]
    public int quantite;
    [Tooltip("Quantité maximale de l'objet")]
    public int quantiteMax;
    [Tooltip("Prefab de l'objet")]
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "Ressource", menuName = "Objet", order = 2)]
public class RessourceClass : ObjetClass
{
    [HideInInspector] public float rendement;
    
}
