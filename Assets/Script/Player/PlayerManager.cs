using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    
    
    //Variables pour le syst Observateur
    [Tooltip("Liste des observateurs")]
    [SerializeField] private List<IObserver> observers = new List<IObserver>();
    [Tooltip("Data à notifier")]
    [HideInInspector] public Data data; //L'ordre contenu sera changer par le menu radial d'ordres.
    
    //Variables pour l'UI
    [Tooltip("Barre de vie")]
    [SerializeField] private Image healthBar;
    [Tooltip("Barre d'oxygène")]
    [SerializeField] private Image oxygenBar;
    [Tooltip("Barre de radiation")]
    [SerializeField] private Image radiationBar;
    [Tooltip("Barre de pression")]
    [SerializeField] private Image pressureBar;
    [Tooltip("Barre de température")]
    [SerializeField] private Image temperatureBar;
    

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        data = new Data();
    }

    private void Update()
    {
        UIUpdater();
    }

    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }
    
    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }
    
    
    public void NotifyObservers()
    {
        foreach (IObserver observer in observers)
        {
            observer.OnNotify(data);
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
        pressureBar.fillAmount = (float) pressure / maxPressure;
        temperatureBar.fillAmount = temperature / maxTemperature;
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
    Follow, Mine, GoOnPing, Idle
}


