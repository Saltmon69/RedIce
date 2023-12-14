using System;
using UnityEngine;

public class BlueprintLinkMachinesState : BlueprintBaseState
{
    private GameObject _cableStock;

    private GameObject _thisCable;
    private CableLaserBehaviour _cableLaserBehaviour;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(7).gameObject.SetActive(true);

        _cableStock = GameObject.Find("CableStock");

        _thisCable = _cableStock.transform.GetChild(_cableStock.transform.childCount - 1).gameObject;

        _cableLaserBehaviour = _thisCable.GetComponent<CableLaserBehaviour>();
        _cableLaserBehaviour.enabled = true;
    }
    
    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            blueprint.SwitchState(blueprint.cableState);
            GameObject.Destroy(_thisCable);
        }
    }
    
    public override void RayState(BlueprintStateMachineManager blueprint, RaycastHit hitData, RaycastHit oldHitData)
    {
        if (hitData.transform.gameObject.layer == 6)
        {
            //si l'on sélectionne une entrée de machine, on assigne au cable qu il est attaché a cette entrée et a quel machine cette elle appartient
            //avance au mode de création de point de passage pour rediriger le chemin que le cable emprunte pour aller d'une machine à l'autre
            if(Input.GetKeyDown(KeyCode.Mouse0) && hitData.transform.CompareTag("Input") && hitData.transform.parent.gameObject != _cableLaserBehaviour.outputMachine)
            {
                _cableLaserBehaviour.inputMachine = hitData.transform.parent.gameObject;
                _cableLaserBehaviour.inputGameObject = hitData.transform.gameObject;
                blueprint.SwitchState(blueprint.checkpointState);
            }
            
            if (hitData.transform.CompareTag("Input") &&
                hitData.transform.parent.gameObject != _cableLaserBehaviour.outputMachine &&
                hitData.transform.gameObject != oldHitData.transform.gameObject)
            {
                hitData.transform.GetComponent<HighlightComponent>().Outline();
            }
        }
    }
        
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(7).gameObject.SetActive(false);
        
        _cableLaserBehaviour.enabled = true;
    }
}
