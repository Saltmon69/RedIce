using System;
using UnityEngine;

public class SelectMachineCableLink : MonoBehaviour
{
    public bool isInput;
    private Camera _mainCamera;
    private Ray _ray;
    private RaycastHit _hitData;
    private RaycastHit _oldHitData;
    public LayerMask layerMask;
        
    private BlueprintMode _blueprintMode;

    public GameObject cableStock;
    public GameObject cable;

    private GameObject _thisCable;
    private CableLaserBehaviour _cableLaserBehaviour;

    private Material _oldMaterial;
    public Material newMaterial;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
        _blueprintMode = this.gameObject.GetComponent<BlueprintMode>();
    }
    
    void OnEnable()
    {
        if (!isInput)
        {
            _thisCable = Instantiate(cable, cableStock.transform);
        }

        if (isInput)
        {
            _thisCable = cableStock.transform.GetChild(cableStock.transform.childCount - 1).gameObject;
        }

        _cableLaserBehaviour = _thisCable.GetComponent<CableLaserBehaviour>();
        _cableLaserBehaviour.enabled = true;
        
        _hitData.transform.GetComponent<MeshRenderer>().material = new Material(_oldMaterial);
    }

    private void OnDisable()
    {
        if (_cableLaserBehaviour.outputMachine == null)
        {
            Destroy(_thisCable);
        }
        
        _cableLaserBehaviour.enabled = true;
        _hitData.transform.GetComponent<MeshRenderer>().material = new Material(newMaterial);
    }

    private void Update()
    {
        _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out _hitData, Mathf.Infinity , layerMask))
        {
            try
            {
                if (_hitData.transform.gameObject != _oldHitData.transform.gameObject)
                {
                    _oldHitData.transform.GetComponent<HighlightComponent>().BaseMaterial();
                    
                    _blueprintMode.nextModesRequirements[0] = false;
                    
                    if (!isInput && _hitData.transform.CompareTag("Output"))
                    {
                        _cableLaserBehaviour.outputMachine = _hitData.transform.parent.parent.gameObject;
                        _cableLaserBehaviour.outputGameObject = _hitData.transform.gameObject;
                        
                        _hitData.transform.GetComponent<HighlightComponent>().Outline();

                        _blueprintMode.nextModesRequirements[0] = true;
                    }

                    if (isInput && _hitData.transform.CompareTag("Input"))
                    {
                        _cableLaserBehaviour.inputMachine = _hitData.transform.parent.parent.gameObject;
                        _cableLaserBehaviour.inputGameObject = _hitData.transform.gameObject;
                        
                        _hitData.transform.GetComponent<HighlightComponent>().Outline();

                        _blueprintMode.nextModesRequirements[0] = true;
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
}
