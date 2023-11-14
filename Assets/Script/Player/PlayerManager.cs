using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

[Description("Contient toutes les constantes du joueur (vie, oxygène, radiation, pression, température).")]
public class PlayerManager : MonoBehaviour
{
    [Tooltip("Instance de PlayerManager")]
    public static PlayerManager instance;
    [Tooltip("L'objet Player")]
    public GameObject player;
    
    //Variables de valeurs
    
    [Tooltip("Vie du joueur")]
    public int playerHealth;
    int playerMaxHealth;
    [Tooltip("Oxygène du joueur")]
    public int oxygen;
    int maxOxygen;
    [Tooltip("Radiation du joueur")]
    public int radiation;
    int maxRadiation;
    [Tooltip("Pression du joueur")]
    public int pressure;
    int maxPressure;
    [Tooltip("Température du joueur")]
    public float temperature;
    float maxTemperature;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
