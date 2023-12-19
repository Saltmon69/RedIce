using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Description("Cette classe est l'interface des états. Elle contient les fonctions OnEnter, OnExit et OnUpdate.")]
public interface IState
{
    // L'état est divisé en trois états: OnEnter, OnExit et OnUpdate. Cela sert à gérer les transitions simplement et efficacement dans la state machine.
    // OnUpdate est appelé dans le Update de la SM et contient le comportement réel de l'état.
    public void OnEnter(EONStateMachine stateMachine);
    public void OnExit(EONStateMachine stateMachine);
    public void OnUpdate(EONStateMachine stateMachine);
    
}
