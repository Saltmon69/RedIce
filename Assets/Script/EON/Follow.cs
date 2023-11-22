using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

[Description("Cette classe est l'état de suivi du joueur par le robot. Il est appelé par l'état Idle et est appelé par l'état GoOnPing.")]


public class Follow : IState, IObserver
{
    public GameObject subject;
    public GameObject ping;
    public Order activeOrder;
    
    public void OnEnter(EONStateMachine stateMachine)
    {
        subject = GameObject.Find("Player");
        subject.GetComponent<PlayerManager>().AddObserver(this);
    }

    public void OnExit(EONStateMachine stateMachine)
    {
        subject.GetComponent<PlayerManager>().RemoveObserver(this);
        
    }

    public void OnUpdate(EONStateMachine stateMachine)
    {
        NavMeshAgent agent = stateMachine.GetComponent<NavMeshAgent>();
        agent.SetDestination(subject.transform.position);
        
        if (stateMachine.distanceToPlayer < 5)
        {
            stateMachine.ChangeState(new Idle());
        }
        if(activeOrder == Order.GoOnPing)
        {
            stateMachine.ChangeState(new GoOnPing());
        }
    }

    public void OnNotify(Data data)
    {
        if (data.ping != null)
        {
            ping = data.ping;
        }
        if(activeOrder == Order.GoOnPing)
        {
            activeOrder = Order.GoOnPing;
        }
        
    }
}
