using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[Description("Cette classe est l'état Idle du robot. Il est appelé par l'état Follow et par l'état GoOnPing.")]
public class Idle : IState, IObserver
{
   
    private GameObject ping;
    public Order? activeOrder = Order.None;
    
    public void OnEnter(EONStateMachine stateMachine)
    {
        stateMachine.subject.GetComponent<PlayerManager>().AddObserver(this);
        
    }
    
    public void OnExit(EONStateMachine stateMachine)
    {
        stateMachine.subject.GetComponent<PlayerManager>().RemoveObserver(this);
        stateMachine.timeInIdle = 0;
        activeOrder = null;
        
    }
    
    public void OnUpdate(EONStateMachine stateMachine)
    {
        stateMachine.timeInIdle += Time.deltaTime;
        
        if (stateMachine.distanceToPlayer > 5 && activeOrder == Order.Follow || stateMachine.timeInIdle > 5 && activeOrder == Order.Follow)
        {
            stateMachine.ChangeState(new Follow());
        }
        if (activeOrder == Order.GoOnPing)
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
