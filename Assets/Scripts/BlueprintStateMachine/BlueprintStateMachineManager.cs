using System;
using UnityEngine;

public class BlueprintStateMachineManager : MonoBehaviour
{

    //initialise tous les états de la state machine
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
    private RaycastHit _hitData;
    private RaycastHit _oldHitData;
    
    //la machine entre dans le premier état
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
    
    //fonction qui permet de faire des raycasts
    //utile au fait que cela permet de passer le meme rayon et la meme distance pour chaque état
    public void RayState()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out _hitData, distance))
        {
            _currentState.RayState(this, _hitData, _oldHitData);

            try
            {
                if (_hitData.transform.gameObject != _oldHitData.transform.gameObject || _hitData.transform.gameObject == null)
                {
                    _oldHitData.transform.GetComponent<HighlightComponent>().BaseMaterial();
                }
            }catch(NullReferenceException){}
            
            _oldHitData = _hitData;
        }
    }

    public void SwitchState(BlueprintBaseState newState)
    {
        ExitState();
        _currentState = newState;
        newState.EnterState(this);
    }
}
