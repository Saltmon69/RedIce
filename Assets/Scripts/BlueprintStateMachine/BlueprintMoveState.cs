using System;
using UnityEngine;

public class BlueprintMoveState : BlueprintBaseState
{
    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
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
    
    public override void RayState(BlueprintStateMachineManager blueprint, RaycastHit hitData, RaycastHit oldHitData, bool hadHit)
    {
        if(hitData.transform.gameObject.layer == 6)
        {
            //mode du déplacement de la machine
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                //change la position de la machine dans la hiérarchie a la derniere position pour etre retrouver facilement dans le prochain etat 
                if(hitData.transform.CompareTag("Untagged") || hitData.transform.CompareTag("Chest"))
                {
                    hitData.transform.SetSiblingIndex(hitData.transform.parent.childCount - 1);
                    blueprint.SwitchState(blueprint.displacementState);
                }
            }
            
            //Actualise les matériaux pour le feedback de quel machine on va selectionner
            if(hitData.transform.gameObject != oldHitData.transform.gameObject || !hadHit)
            {
                try
                {
                    if(hitData.transform.CompareTag("Untagged") || hitData.transform.CompareTag("Chest"))
                    {
                        hitData.transform.GetComponent<HighlightComponent>().Outline();
                    }
                    else
                    {
                        hitData.transform.parent.GetComponent<HighlightComponent>().Outline();
                    }
                }catch(NullReferenceException){}
            }
        }

        try
        {
            //le but de cette partie de code est de faire en sorte que si on regarde une machine, puis une de ses entrée/sortie et vice-versa, elle ne désactive pas son feedback de sélection visuel
            if(hitData.transform.gameObject != oldHitData.transform.gameObject)
            {
                if (hitData.transform.gameObject.layer == 6 && (hitData.transform.parent.gameObject == oldHitData.transform.gameObject
                                                            || hitData.transform.gameObject == oldHitData.transform.parent.gameObject))
                {
                    Debug.Log("The two machine objects has a child/parent connection");
                }
                else
                {
                    //fait en sorte que si on avais le curseur sur la machine ou une de ses entrée/sortie, la machine entière se met au materiaux de base
                    if (oldHitData.transform.CompareTag("Input") || oldHitData.transform.CompareTag("Output"))
                    {
                        oldHitData.transform.parent.GetComponent<HighlightComponent>().BaseMaterial();
                    }
                    else
                    {
                        oldHitData.transform.GetComponent<HighlightComponent>().BaseMaterial();
                    }
                }
            }
        }catch(NullReferenceException){}
    }    
    
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(2).gameObject.SetActive(false);
    }
}
