using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VInspector;

[Description("Contient toutes les constantes du joueur (vie, oxygène, radiation, pression, température).")]
public class PlayerManager : MonoBehaviour
{
    
    [Tooltip("Instance de PlayerManager")]
    public static PlayerManager instance;
    [Tooltip("L'objet Player")]
    public GameObject player;

    
    //Variables de valeurs
    [Tab("Constantes")]
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
    
    [Tab("State Machine")]
    //Variables pour le syst Observateur
    [Tooltip("Liste des observateurs")]
    public List<IObserver> observers = new List<IObserver>();
    [HideInInspector] public GameObject activePing;
    public Order activeOrder;
    
    [Tab("Barres Constantes")]
    //Variables pour l'UI
    [Tooltip("Barre de vie")]
    [SerializeField] private Image healthBar;
    [Tooltip("Barre d'oxygène")]
    [SerializeField] private Image oxygenBar;
    [Tooltip("Barre de radiation")]
    [SerializeField] private Image radiationBar;
    
    
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        
        playerMaxHealth = playerHealth;
        maxOxygen = oxygen;
        maxRadiation = radiation;
        maxPressure = pressure;
        maxTemperature = temperature;
    }
    private void Update()
    {   
        UIUpdater();
        //Debug.Log("Observeurs : " + observers.Count + observers[0]);
    }

    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }
    
    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }
    
    
    public void NotifyObservers(Data dataInFct)
    {
        activePing = dataInFct.ping;
        activeOrder = dataInFct.order;
        
        foreach (IObserver observer in observers)
        {
            observer.OnNotify(dataInFct);
        }
    }
    
    /// <summary>
    /// Évite une surcharge de code dans le Update. Actualise les barres de vie, oxygène, radiation, pression et température.
    /// </summary>
    private void UIUpdater()
    {
        healthBar.fillAmount = (float) playerHealth / playerMaxHealth;
        oxygenBar.fillAmount = (float) oxygen / maxOxygen;
        radiationBar.fillAmount = (float) radiation / maxRadiation;
    }
    
    public void TakeDamage(float damage, ZoneType zone)
    {
        switch (zone)
        {
            case ZoneType.Hot:
                playerHealth -= (int)damage;
                break;
            case ZoneType.Cold:
                playerHealth -= (int)damage;
                break;
            case ZoneType.Pressure:
                pressure += (int)damage;
                break;
            case ZoneType.Toxic:
                playerHealth -= (int)damage;
                break;
            case ZoneType.Radiation:
                radiation -= (int)damage;
                break;
            case ZoneType.LowOxygen:
                oxygen -= (int)damage;
                break;
        }
    }
    
    
    
}

/// <summary>
/// Cette classe sert à transmettre le ping créé ainsi que l'objet pingé et l'ordre à effectuer.
/// </summary>
public class Data : MonoBehaviour
{
    public GameObject itemPinged;
    public GameObject ping;
    public Order order;
}

/// <summary>
/// Cette classe contient les différents ordres que peut recevoir le robot. Il seront utilisés dans les états comme condition pour changer d'états ou non.
/// </summary>
public enum Order
{
    Follow, GoOnPing, Idle, None
}


