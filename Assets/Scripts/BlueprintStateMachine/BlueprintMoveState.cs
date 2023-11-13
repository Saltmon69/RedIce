using System;
using UnityEngine;

public class BlueprintMoveState : BlueprintBaseState
{
    private RaycastHit _hitData;
    private RaycastHit _oldHitData;
    private LayerMask layerMask;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        layerMask = LayerMask.GetMask("Machine");
        GameObject.Find("UIStateCanvas").transform.GetChild(2).gameObject.SetActive(true);
    }
    
    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            blueprint.SwitchState(blueprint.startState);
        }
    }
    
    public override void RayState(BlueprintStateMachineManager blueprint, Ray ray, float distance)
    {
        if (Physics.Raycast(ray, out _hitData, distance, layerMask))
        {
            try
            {
                if (_hitData.transform.gameObject != _oldHitData.transform.gameObject)
                {
                    _hitData.transform.SetSiblingIndex(_hitData.transform.parent.childCount - 1);
                    _hitData.transform.GetComponent<HighlightComponent>().Outline();
                    _oldHitData.transform.GetComponent<HighlightComponent>().BaseMaterial();
                    _oldHitData = _hitData;
                }
                                
                if(Input.GetKeyDown(KeyCode.Mouse0))
                {
                    blueprint.SwitchState(blueprint.displacementState);
                }
            }
            catch (NullReferenceException)
            {
                _oldHitData = _hitData;
            }
        }
    }    
    
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(2).gameObject.SetActive(false);
    }
}
