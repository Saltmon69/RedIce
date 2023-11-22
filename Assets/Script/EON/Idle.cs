using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[Description("Cette classe est l'état Idle du robot. Il est appelé par l'état Follow et par l'état GoOnPing.")]
public class Idle : IState, IObserver
{
    public GameObject subject;
    private GameObject ping;
    public Order activeOrder;
    public void OnEnter(EONStateMachine stateMachine)
    {
        subject = GameObject.Find("Player");
        subject.GetComponent<PlayerManager>().AddObserver(this);
        Debug.Log("Je suis dans l'état Idle");
    }
    
    public void OnExit(EONStateMachine stateMachine)
    {
        subject.GetComponent<PlayerManager>().RemoveObserver(this);
        stateMachine.timeInIdle = 0;
        Debug.Log("Je quitte l'état Idle");
    }
    
    public void OnUpdate(EONStateMachine stateMachine)
    {
        stateMachine.timeInIdle += Time.deltaTime;
        
        if (stateMachine.distanceToPlayer > 5)
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
        if (data.ping != null)
        {
            ping = data.ping;
        }
        if (data.order == Order.GoOnPing)
        {
            activeOrder = Order.GoOnPing;
        }
    }
}
