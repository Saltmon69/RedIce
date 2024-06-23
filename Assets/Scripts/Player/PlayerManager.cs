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
    [Tooltip("Liste des observateurs")]
    public List<IObserver> observers = new List<IObserver>();
    public GameObject activePing;
    public Order activeOrder;
    
    //Variables de valeurs
    [Tab("Constantes")]
    [Tooltip("Vie du joueur")]
    public int playerHealth;
    [HideInInspector] public int playerMaxHealth;
    [Tooltip("Oxygène du joueur")]
    public int oxygen;
    [HideInInspector] public int maxOxygen;
    [Tooltip("Radiation du joueur")]
    public int radiation;
    [HideInInspector] public int maxRadiation;
    [Tooltip("Pression du joueur")]
    public int pressure;
    [HideInInspector] public int maxPressure;
    [Tooltip("Température du joueur")]
    public float temperature;
    [HideInInspector] public float maxTemperature;
    
    [Tab("Barres Constantes")]
    //Variables pour l'UI
    [Tooltip("Barre de vie")]
    [SerializeField] private Image healthBar;
    [Tooltip("Barre d'oxygène")]
    [SerializeField] private Image oxygenBar;
    [Tooltip("Barre de radiation")]
    [SerializeField] private Image radiationBar;

    private bool _isAppearing;
    public float flashingSpeed;
    public InventoryUpgrade inventoryUpgrade;
    
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
        
        if(healthBar.fillAmount < 0.3f) FlashingBar(healthBar);
        if(oxygenBar.fillAmount < 0.3f) FlashingBar(oxygenBar);
        if(radiationBar.fillAmount < 0.3f) FlashingBar(radiationBar);
    }

    private void FlashingBar(Image bar)
    {
        if(_isAppearing)
        {
            if(bar.color.a > 0.3f)
            {
                bar.color -= new Color(0,0,0,Time.deltaTime * flashingSpeed);
            }
            else
            {
                _isAppearing = false;
            }
        }
        else
        {
            if(bar.color.a < 1f)
            {
                bar.color += new Color(0,0,0,Time.deltaTime * flashingSpeed);
            }
            else
            {
                _isAppearing = true;
            }
        }
    }
    
    public void TakeDamage(float damage, ZoneType zone)
    {
        switch (zone)
        {
            case ZoneType.Hot:
                if(!inventoryUpgrade.upgradeItemInInventory.Contains(Resources.Load<ItemClass>("SO/Crafts/AM_Temperature")))
                    playerHealth -= (int)damage;
                break;
            case ZoneType.Cold:
                if(!inventoryUpgrade.upgradeItemInInventory.Contains(Resources.Load<ItemClass>("SO/Crafts/AM_Temperature")))
                    playerHealth -= (int)damage;
                break;
            case ZoneType.Pressure:
                if(!inventoryUpgrade.upgradeItemInInventory.Contains(Resources.Load<ItemClass>("SO/Crafts/AM_Pressure")))
                    pressure -= (int)damage;
                break;
            case ZoneType.Toxic:
                playerHealth -= (int)damage;
                break;
            case ZoneType.Radiation:
                if(!inventoryUpgrade.upgradeItemInInventory.Contains(Resources.Load<ItemClass>("SO/Crafts/AM_Radioactive")))
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


