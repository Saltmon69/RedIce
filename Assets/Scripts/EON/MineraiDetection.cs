using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineraiDetection : MonoBehaviour
{
    [SerializeField] EONStateMachine eonStateMachine;
    
    private void OnTriggerEnter(Collider other)
    {
        eonStateMachine.mineraiDetected.Clear();
        if (other.CompareTag("Minerai"))
        {
            MineraiClass minerai = other.GetComponent<MineraiClass>();
            eonStateMachine.mineraiDetected.Add(minerai);
        }
    }
}
