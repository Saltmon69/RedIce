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

    private Material _oldMaterial;
    public Material newMaterial;

    private bool _isOutputSelected;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(6).gameObject.SetActive(true);

        layerMask = LayerMask.GetMask("Machine");
        cableStock = GameObject.Find("CableStock");

        cable = Resources.Load("Cables/Cable", typeof(GameObject));

        _thisCable = GameObject.Instantiate((GameObject)cable, cableStock.transform);

        _cableLaserBehaviour = _thisCable.GetComponent<CableLaserBehaviour>();
        _cableLaserBehaviour.enabled = true;
        
        //_hitData.transform.GetComponent<MeshRenderer>().material = new Material(_oldMaterial);
    }
    
    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            blueprint.SwitchState(blueprint.startState);
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && _isOutputSelected)
        {
            blueprint.SwitchState(blueprint.linkMachinesState);
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
                    
                    _isOutputSelected = false;

                    if (_hitData.transform.CompareTag("Output"))
                    {
                        _cableLaserBehaviour.outputMachine = _hitData.transform.parent.parent.gameObject;
                        _cableLaserBehaviour.outputGameObject = _hitData.transform.gameObject;
                        
                        _hitData.transform.GetComponent<HighlightComponent>().Outline();

                        _isOutputSelected = true;
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
        GameObject.Find("UIStateCanvas").transform.GetChild(6).gameObject.SetActive(false);

        if (_cableLaserBehaviour.outputMachine == null)
        {
            GameObject.Destroy(_thisCable);
        }
        
        _cableLaserBehaviour.enabled = true;
        //_hitData.transform.GetComponent<MeshRenderer>().material = new Material(newMaterial);
    }
}
