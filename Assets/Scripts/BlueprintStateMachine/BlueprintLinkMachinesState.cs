using System;
using UnityEngine;

public class BlueprintLinkMachinesState : BlueprintBaseState
{
    private RaycastHit _hitData;
    private RaycastHit _oldHitData;
    public LayerMask layerMask;

    public GameObject cableStock;

    private GameObject _thisCable;
    private CableLaserBehaviour _cableLaserBehaviour;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(7).gameObject.SetActive(true);

        layerMask = LayerMask.GetMask("Machine");
        cableStock = GameObject.Find("CableStock");

        _thisCable = cableStock.transform.GetChild(cableStock.transform.childCount - 1).gameObject;

        _cableLaserBehaviour = _thisCable.GetComponent<CableLaserBehaviour>();
        _cableLaserBehaviour.enabled = true;
    }
    
    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            blueprint.SwitchState(blueprint.cableState);
        }
    }
    
    public override void RayState(BlueprintStateMachineManager blueprint, Ray ray, float distance)
    {
        if (Physics.Raycast(ray, out _hitData, Mathf.Infinity , layerMask))
        {
            //si l'on sélectionne une entrée de machine, on assigne au cable qu il est attaché a cette entrée et a quel machine cette elle appartient
            //avance au mode de création de point de passage pour rediriger le chemin que le cable emprunte pour aller d'une machine à l'autre
            if(Input.GetKeyDown(KeyCode.Mouse0) && _hitData.transform.CompareTag("Input") && _hitData.transform.parent.gameObject != _cableLaserBehaviour.outputMachine)
            {
                _cableLaserBehaviour.inputMachine = _hitData.transform.parent.gameObject;
                _cableLaserBehaviour.inputGameObject = _hitData.transform.gameObject;
                blueprint.SwitchState(blueprint.checkpointState);
            }
            
            try
            {
                //feedback pour voir quel entrée on vise / regarde
                if (_hitData.transform.gameObject != _oldHitData.transform.gameObject)
                {
                    _oldHitData.transform.GetComponent<HighlightComponent>().BaseMaterial();

                    if (_hitData.transform.CompareTag("Input") && _hitData.transform.parent.gameObject != _cableLaserBehaviour.outputMachine)
                    {
                        _hitData.transform.GetComponent<HighlightComponent>().Outline();
                    }
                }
            }catch (NullReferenceException){}
            _oldHitData = _hitData;
        }
    }
        
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(7).gameObject.SetActive(false);
        
        _cableLaserBehaviour.enabled = true;
    }
}
