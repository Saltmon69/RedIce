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
        //bouton retour au mode de sélection des modes du blueprint
        if(Input.GetKeyDown(KeyCode.B))
        {
            blueprint.SwitchState(blueprint.startState);
        }
    }
    
    public override void RayState(BlueprintStateMachineManager blueprint, Ray ray, float distance)
    {
        if (Physics.Raycast(ray, out _hitData, distance, layerMask))
        {
            //mode du déplacement de la machine
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                //change la position de la machine dans la hiérarchie a la derniere position pour etre retrouver facilement dans le prochain etat 
                _hitData.transform.SetSiblingIndex(_hitData.transform.parent.childCount - 1);
                blueprint.SwitchState(blueprint.displacementState);
            }

            //Actualise les matériaux pour le feedback de quel machine on va selectionner
            try
            {
                if (_hitData.transform.gameObject != _oldHitData.transform.gameObject)
                {
                    _hitData.transform.GetComponent<HighlightComponent>().Outline();
                    _oldHitData.transform.GetComponent<HighlightComponent>().BaseMaterial();
                }
            }catch (NullReferenceException){}

            _oldHitData = _hitData;
        }
    }    
    
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(2).gameObject.SetActive(false);
    }
}
