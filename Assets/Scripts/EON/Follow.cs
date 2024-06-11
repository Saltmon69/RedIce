using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

[Description("Cette classe est l'état de suivi du joueur par le robot. Il est appelé par l'état Idle et est appelé par l'état GoOnPing.")]


public class Follow : IState, IObserver
{
    
    public GameObject ping;
    public Order? activeOrder;

    private float timer;
    private float scanningCd = 5f;
    
    public void OnEnter(EONStateMachine stateMachine)
    {
        stateMachine.subject.GetComponent<PlayerManager>().AddObserver(this);
        stateMachine.agent.SetDestination(stateMachine.subject.transform.position);
    }

    public void OnExit(EONStateMachine stateMachine)
    {
        stateMachine.subject.GetComponent<PlayerManager>().RemoveObserver(this);
        activeOrder = null;
        
    }

    public void OnUpdate(EONStateMachine stateMachine)
    {
        timer += Time.deltaTime;
        if (timer >= scanningCd)
        {
            stateMachine.Scanning();
            timer = 0;
        }
        
        NavMeshAgent agent = stateMachine.GetComponent<NavMeshAgent>();
        
        agent.SetDestination(stateMachine.subject.transform.position);
        
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
        
        ping = data.ping;
        activeOrder = data.order;
        
        
    }
}
