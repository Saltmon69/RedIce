using System;
using UnityEngine;

public class BlueprintMoveState : BlueprintBaseState
{
    private LayerMask _layerMask;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        _layerMask = LayerMask.GetMask("Machine");
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
    
    public override void RayState(BlueprintStateMachineManager blueprint, RaycastHit hitData, RaycastHit oldHitData)
    {
        if (hitData.transform.gameObject.layer == _layerMask)
        {
            //mode du déplacement de la machine
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //change la position de la machine dans la hiérarchie a la derniere position pour etre retrouver facilement dans le prochain etat 
                hitData.transform.SetSiblingIndex(hitData.transform.parent.childCount - 1);
                blueprint.SwitchState(blueprint.displacementState);
            }
            
            //Actualise les matériaux pour le feedback de quel machine on va selectionner
            if (hitData.transform.gameObject != oldHitData.transform.gameObject)
            {
                hitData.transform.GetComponent<HighlightComponent>().Outline();
            }
        }
    }    
    
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(2).gameObject.SetActive(false);
    }
}
