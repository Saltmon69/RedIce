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
    private bool _hasHit;
    private bool _isOldHitDataNull;

    //la machine entre dans le premier état
    public void Awake()
    {
        _currentState = startState;
        
        EnterState();

        mainCamera = Camera.main;
        _isOldHitDataNull = true;
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
        
        if(Physics.Raycast(ray, out _hitData, distance))
        {
            try
            {            
                if (_hitData.transform.gameObject != _oldHitData.transform.gameObject){}
                _isOldHitDataNull = false;
            }catch(NullReferenceException) { _isOldHitDataNull = true; }

            _currentState.RayState(this, _hitData, _oldHitData, _hasHit);
            
            if (!_isOldHitDataNull)
            {
                if(_hitData.transform.gameObject != _oldHitData.transform.gameObject)
                {
                    //le MoveState a sa propre logique pour remettre au matériaux de base dû a son fonctionnement particulier
                    try
                    {
                        if (_currentState != moveState) _oldHitData.transform.GetComponent<HighlightComponent>().BaseMaterial();
                    }catch(NullReferenceException){}
                    
                    _oldHitData = _hitData;
                }
            }
            else
            {
                _oldHitData = _hitData;
                _isOldHitDataNull = false;
            }

            _hasHit = true;
        }
        else if(_hasHit)
        {
            //permet de gérer le retour au materiaux de base si le raycast n'a rien touché
            try
            {
                _oldHitData.transform.GetComponent<HighlightComponent>().BaseMaterial();
            }catch(NullReferenceException){}
            
            _hasHit = false;
        }
    }

    public void SwitchState(BlueprintBaseState newState)
    {
        ExitState();
        _currentState = newState;
        newState.EnterState(this);
    }
}
