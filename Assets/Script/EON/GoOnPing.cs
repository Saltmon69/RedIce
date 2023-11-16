using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoOnPing : IState, IObserver
{
    public GameObject subject;
    public GameObject ping;

    private bool mineraiInRange = false;
    public void OnEnter(EONStateMachine stateMachine)
    {
        subject = GameObject.Find("Player");
        subject.GetComponent<PlayerManager>().AddObserver(this);
    }

    public void OnExit(EONStateMachine stateMachine)
    {
        subject.GetComponent<PlayerManager>().RemoveObserver(this);
        throw new System.NotImplementedException();
    }

    public void OnUpdate(EONStateMachine stateMachine)
    {
        if (Vector3.Distance(stateMachine.transform.position, ping.transform.position) < 1)
        {
            if (mineraiInRange)
            {
                stateMachine.ChangeState(new Mine());
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
            if (data.itemPinged.CompareTag("Minerai"))
            {
                mineraiInRange = true;
            }
            else
            {
                mineraiInRange = false;
            }
        }
    }
}
