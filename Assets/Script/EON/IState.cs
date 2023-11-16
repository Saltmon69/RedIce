using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    
    public void OnEnter(EONStateMachine stateMachine);
    public void OnExit(EONStateMachine stateMachine);
    public void OnUpdate(EONStateMachine stateMachine);
    
}
