using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

public class PingBehaviour : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;
    private EONStateMachine stateMachine;
    
    private void OnTriggerEnter(Collider other)
    {
        navMeshSurface = other.GetComponent<NavMeshSurface>();
        stateMachine = FindObjectOfType<EONStateMachine>();
        stateMachine.navMeshSurface = navMeshSurface;
        navMeshSurface.BuildNavMesh();
    }
}
