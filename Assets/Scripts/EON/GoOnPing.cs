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
        if (ping == null)
        {
            ping = stateMachine.subject.GetComponent<PlayerManager>().activePing;
        }
        stateMachine.agent.SetDestination(ping.transform.position);
        
    }

    public void OnExit(EONStateMachine stateMachine)
    {
        activeOrder = null;
        stateMachine.subject.GetComponent<PlayerManager>().RemoveObserver(this);
        
    }

    public void OnUpdate(EONStateMachine stateMachine)
    {
        if (activeOrder == Order.Follow)
        {
            stateMachine.ChangeState(new Follow());
        }
        
        stateMachine.agent.SetDestination(ping.transform.position);
        
        if (Vector3.Distance(stateMachine.transform.position, ping.transform.position) < 2)
        {
            stateMachine.ChangeState(new Idle());
        }
        
        if (mineraiInRange)
        {
            stateMachine.ChangeState(new Mine());
        }

        
                
    }

    public void OnNotify(Data data)
    {
        if (data.itemPinged != null)
        {
            ping = data.itemPinged;
        }
        
        ping = data.ping;
        activeOrder = data.order;
        
    }
}
