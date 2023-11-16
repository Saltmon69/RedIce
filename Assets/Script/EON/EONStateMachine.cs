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
    
    #endregion

    #region Fonctions

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, GameObject.Find("Player").transform.position);
        
        currentState.OnUpdate(this);
    }
    
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
