using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

public class PingBehaviour : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;
    private void Start()
    {
       Physics.Raycast(transform.position, -transform.up, out var hit, 100f);
         navMeshSurface = hit.collider.GetComponent<NavMeshSurface>();
         navMeshSurface.BuildNavMesh();
        Debug.Log(hit.collider.name);
        
    }

}
