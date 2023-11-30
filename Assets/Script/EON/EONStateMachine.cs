using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Description("Ce script est la state machine du robot assistant EON. Il va s'occuper d'éxecuter les états mais pas de les définir ou décider quand en changer.")]
public class EONStateMachine : MonoBehaviour
{
    #region Variables

    IState currentState;
    
    public float distanceToPlayer;
    public float distanceToPing;
    public float timeInIdle;
    
    public GameObject subject;
    
    #endregion

    #region Fonctions

    private void Start()
    {
        currentState = new Idle();
    }

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, GameObject.Find("Player").transform.position);
        
        currentState.OnUpdate(this);
    }
    
    /// <summary>
    /// Sert à changer l'état actuel du robot. Est appelé par les états eux-mêmes.
    /// </summary>
    /// <param name="newState">Etat à activer</param>
    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        currentState.OnEnter(this);
    }

    #endregion 
}
