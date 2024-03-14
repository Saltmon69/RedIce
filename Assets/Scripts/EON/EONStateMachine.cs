using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using VInspector;

[Description("Ce script est la state machine du robot assistant EON. Il va s'occuper d'éxecuter les états mais pas de les définir ou décider quand en changer.")]
public class EONStateMachine : MonoBehaviour
{
    #region Variables

    [SerializeField] IState currentState;
    
    [Tab("Valeurs")]
    public float distanceToPlayer;
    public float distanceToPing;
    public float timeInIdle;
    
    [Tab("NavMesh")]
    public NavMeshSurface navMeshSurface;
    public NavMeshAgent agent;
    public GameObject subject;
    
    [Tab("Anims")]
    public Animator anim;
    
    #endregion

    #region Fonctions

    private void Start()
    {
        currentState = new Idle();
        currentState.OnEnter(this);
    }

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, GameObject.Find("Player").transform.position);
        currentState.OnUpdate(this);
        
        if (currentState is GoOnPing || currentState is Follow)
        {
            anim.SetBool("isWalking", true);
        }
        else if (currentState is Idle)
        {
            anim.SetBool("isWalking", false);
        }
        
        
        
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
