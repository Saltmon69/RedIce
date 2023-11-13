using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintStateMachineManager : MonoBehaviour
{
    private BlueprintBaseState _currentState;
    public BlueprintStartState startState = new BlueprintStartState();
    public BlueprintMoveState moveState = new BlueprintMoveState();
    public BlueprintBuildingState buildingState = new BlueprintBuildingState();
    public BlueprintCableState cableState = new BlueprintCableState();
    public BlueprintDisplacementState displacementState = new BlueprintDisplacementState();
    public BlueprintPlacementState placementState = new BlueprintPlacementState();
    public BlueprintLinkMachinesState linkMachinesState = new BlueprintLinkMachinesState();
    public BlueprintCheckpointState checkpointState = new BlueprintCheckpointState();

        public Camera mainCamera;
        public Ray ray;
        public float distance;

    public void Awake()
    {
        _currentState = startState;
        
        EnterState();

        mainCamera = Camera.main;
    }

    public void EnterState()
    {
        _currentState.EnterState(this);
    }
    
    public void Update()
    {
        _currentState.UpdateState(this);
        RayState();
    }

    public void ExitState()
    {
        _currentState.ExitState(this);
    }
    
    public void RayState()
    {
        //physic ray, faire passer le hit data et un layer mask vide
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        _currentState.RayState(this, ray, distance); 
    }

    public void SwitchState(BlueprintBaseState newState)
    {
        ExitState();
        _currentState = newState;
        newState.EnterState(this);
    }
}
