using System;
using UnityEngine;

public class BlueprintCableState : BlueprintBaseState
{
    private RaycastHit _hitData;
    private RaycastHit _oldHitData;
    public LayerMask layerMask;

    public GameObject cableStock;
    public UnityEngine.Object cable;

    private GameObject _thisCable;
    private CableLaserBehaviour _cableLaserBehaviour;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(6).gameObject.SetActive(true);

        layerMask = LayerMask.GetMask("Machine");
        cableStock = GameObject.Find("CableStock");

        cable = Resources.Load("Cables/Cable", typeof(GameObject));

        _thisCable = GameObject.Instantiate((GameObject)cable, cableStock.transform);

        _cableLaserBehaviour = _thisCable.GetComponent<CableLaserBehaviour>();
        _cableLaserBehaviour.enabled = true;
    }
    
    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        //bouton retour au mode de sélection des modes du blueprint
        if(Input.GetKeyDown(KeyCode.C))
        {
            blueprint.SwitchState(blueprint.startState);
            
            //si le cable n'a pas de sortie lié cela veut dire que le joueur a fait un retour arriere avant d utiliser le cable, le cable est donc supprimer
            if (_cableLaserBehaviour.outputMachine == null)
            {
                GameObject.Destroy(_thisCable);
            }
        }
    }
    
    public override void RayState(BlueprintStateMachineManager blueprint, Ray ray, float distance)
    {
        if (Physics.Raycast(ray, out _hitData, Mathf.Infinity , layerMask))
        {
            //si l'on sélectionne une sortie de machine, on assigne au cable qu il est attaché a cette sortie et a quel machine elle appartient
            //avance au mode de sélection de l'entrée de la seconde machine pour le cablage
            if(Input.GetKeyDown(KeyCode.Mouse0) && _hitData.transform.CompareTag("Output"))
            {
                _cableLaserBehaviour.outputMachine = _hitData.transform.parent.parent.gameObject;
                _cableLaserBehaviour.outputGameObject = _hitData.transform.gameObject;
                blueprint.SwitchState(blueprint.linkMachinesState);
            }

            try
            {
                //feedback pour voir quel sortie on vise / regarde
                if (_hitData.transform.gameObject != _oldHitData.transform.gameObject)
                {
                    _oldHitData.transform.GetComponent<HighlightComponent>().BaseMaterial();

                    if (_hitData.transform.CompareTag("Output"))
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
        GameObject.Find("UIStateCanvas").transform.GetChild(6).gameObject.SetActive(false);

        _cableLaserBehaviour.enabled = true;
    }
}
