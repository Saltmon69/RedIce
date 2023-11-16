using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : IState, IObserver
{
    public GameObject subject;
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
        throw new System.NotImplementedException();
    }

    public void OnNotify(Data data)
    {
        throw new System.NotImplementedException();
    }
}
