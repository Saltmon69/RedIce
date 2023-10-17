using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ressource", menuName = "Objet/Ressource", order = 2)]
public class RessourceClass : ScriptableObject
{
    [Tooltip("Nom de la ressource")]
    public string nom;
    [Tooltip("Description de la ressource")]
    public string description;
    [Tooltip("Sprite de la ressource")]
    public Sprite sprite;
    [Tooltip("Quantité de la ressource")]
    public int quantite;
    [Tooltip("Quantité maximale de la ressource")]
    public int quantiteMax;
    [Tooltip("Prefab de la ressource")]
    public GameObject prefab;
    [Tooltip("Rendement de la ressource")]
    public float rendement;
}

