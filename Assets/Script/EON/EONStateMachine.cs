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
    

    #endregion

    #region Fonctions

    private void Update()
    {
        currentState.OnUpdate();
    }
    
    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = newState;
        currentState.OnEnter();
    }

    #endregion
}
