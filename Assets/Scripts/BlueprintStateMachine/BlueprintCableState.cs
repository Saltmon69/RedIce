using System;
using UnityEngine;

public class BlueprintCableState : BlueprintBaseState
{
    private LayerMask _layerMask;

    private GameObject _cableStock;
    private UnityEngine.Object _cable;

    private GameObject _thisCable;
    private CableLaserBehaviour _cableLaserBehaviour;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(6).gameObject.SetActive(true);

        _layerMask = LayerMask.GetMask("Machine");
        _cableStock = GameObject.Find("CableStock");

        _cable = Resources.Load("Cables/Cable", typeof(GameObject));

        _thisCable = GameObject.Instantiate((GameObject)_cable, _cableStock.transform);

        _cableLaserBehaviour = _thisCable.GetComponent<CableLaserBehaviour>();
        _cableLaserBehaviour.enabled = true;
    }
    
    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        //bouton retour au mode de sélection des modes du blueprint
        if(Input.GetKeyDown(KeyCode.C))
        {
            blueprint.SwitchState(blueprint.startState);
            GameObject.Destroy(_thisCable);
        }
    }
    
    public override void RayState(BlueprintStateMachineManager blueprint, RaycastHit hitData, RaycastHit oldHitData)
    {
        if (hitData.transform.gameObject.layer == _layerMask)
        {
            //si l'on sélectionne une sortie de machine, on assigne au cable qu il est attaché a cette sortie et a quel machine elle appartient
            //avance au mode de sélection de l'entrée de la seconde machine pour le cablage
            if(Input.GetKeyDown(KeyCode.Mouse0) && hitData.transform.CompareTag("Output"))
            {
                _cableLaserBehaviour.outputMachine = hitData.transform.parent.gameObject;
                _cableLaserBehaviour.outputGameObject = hitData.transform.gameObject;
                blueprint.SwitchState(blueprint.linkMachinesState);
            }
            
            //feedback pour voir quel sortie on vise / regarde
            if (hitData.transform.CompareTag("Output") && hitData.transform.gameObject != oldHitData.transform.gameObject)
            {
                hitData.transform.GetComponent<HighlightComponent>().Outline();
            }
        }
    }
        
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(6).gameObject.SetActive(false);

        _cableLaserBehaviour.enabled = true;
    }
}
