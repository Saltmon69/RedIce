using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : IState, IObserver
{
    public void OnEnter()
    {
        Debug.Log("Je suis dans l'état Idle");
    }
    
    public void OnExit()
    {
        Debug.Log("Je quitte l'état Idle");
    }
    
    public void OnUpdate()
    {
        Debug.Log("Je suis dans l'état Idle");
    }
    
    public void OnNotify(Data data)
    {
        
    }
}
