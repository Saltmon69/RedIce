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

    private Material _oldMaterial;
    public Material newMaterial;

    private bool _isInputSelected;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(7).gameObject.SetActive(true);

        layerMask = LayerMask.GetMask("Machine");
        cableStock = GameObject.Find("CableStock");

        _thisCable = cableStock.transform.GetChild(cableStock.transform.childCount - 1).gameObject;

        _cableLaserBehaviour = _thisCable.GetComponent<CableLaserBehaviour>();
        _cableLaserBehaviour.enabled = true;
        
        //_hitData.transform.GetComponent<MeshRenderer>().material = new Material(_oldMaterial);
    }
    
    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            blueprint.SwitchState(blueprint.cableState);
        }
        
        if(Input.GetKeyDown(KeyCode.Mouse0) && _isInputSelected)
        {
            blueprint.SwitchState(blueprint.checkpointState);
        }
    }
    
    public override void RayState(BlueprintStateMachineManager blueprint, Ray ray, float distance)
    {
        if (Physics.Raycast(ray, out _hitData, Mathf.Infinity , layerMask))
        {
            try
            {
                if (_hitData.transform.gameObject != _oldHitData.transform.gameObject)
                {
                    _oldHitData.transform.GetComponent<HighlightComponent>().BaseMaterial();
                    
                    _isInputSelected = false;

                    if (_hitData.transform.CompareTag("Input"))
                    {
                        _cableLaserBehaviour.inputMachine = _hitData.transform.parent.parent.gameObject;
                        _cableLaserBehaviour.inputGameObject = _hitData.transform.gameObject;
                        
                        _hitData.transform.GetComponent<HighlightComponent>().Outline();
                        
                        _isInputSelected = true;
                    }
                    _oldHitData = _hitData;
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
        GameObject.Find("UIStateCanvas").transform.GetChild(7).gameObject.SetActive(false);
        
        _cableLaserBehaviour.enabled = true;
        
        //_hitData.transform.GetComponent<MeshRenderer>().material = new Material(newMaterial);
    }
}
