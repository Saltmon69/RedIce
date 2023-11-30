using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

[Description("Cette classe est l'état ordonnant au robot d'aller sur le ping. Il est appelé par l'état Idle et par l'état Follow.")]
public class GoOnPing : IState, IObserver
{
    public GameObject subject;
    public GameObject ping;
    public Order? activeOrder;
    
    private bool mineraiInRange = false;
    
    public void OnEnter(EONStateMachine stateMachine)
    {
        stateMachine.subject.GetComponent<PlayerManager>().AddObserver(this);
    }

    public void OnExit(EONStateMachine stateMachine)
    {
        activeOrder = null;
        stateMachine.subject.GetComponent<PlayerManager>().RemoveObserver(this);
        
    }

    public void OnUpdate(EONStateMachine stateMachine)
    {
        NavMeshAgent agent = stateMachine.GetComponent<NavMeshAgent>();
        agent.SetDestination(ping.transform.position);
        
        if (Vector3.Distance(stateMachine.transform.position, ping.transform.position) < 1)
        {
            if (mineraiInRange || activeOrder == Order.Mine)
            {
                stateMachine.ChangeState(new Mine());
            }

            if (activeOrder == Order.Follow)
            {
                stateMachine.ChangeState(new Follow());
            }
            
            stateMachine.ChangeState(new Idle());
            
        }
        
    }

    public void OnNotify(Data data)
    {
        if (data.ping != null)
        {
            ping = data.ping;
        }
        if (data.itemPinged != null)
        {
            ping = data.itemPinged;
        }
        
        activeOrder = data.order;
        
    }
}
