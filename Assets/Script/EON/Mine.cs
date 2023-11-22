using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Description("Cette classe est l'état de minage du robot. Il est appelé par l'état GoOnPing.")]
public class Mine : IState, IObserver
{
    public GameObject subject;
    public GameObject ping;
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
        Collider[] mineraiInRange = Physics.OverlapBox(stateMachine.transform.position, new Vector3(2, 2, 2));
        
        if (mineraiInRange.Length > 0)
        {
            if (mineraiInRange[0].GetComponent<MineraiClass>().mineraiLife > 0)
            {
                mineraiInRange[0].GetComponent<MineraiClass>().takeDamage(5);
            }
            else if (mineraiInRange[0].GetComponent<MineraiClass>().mineraiLife <= 0)
            {
                stateMachine.ChangeState(new Idle());
            }
        }
    }

    public void OnNotify(Data data)
    {
        if (data.ping != null)
        {
            ping = data.ping;
        }
    }
}
