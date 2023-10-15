using System;
using UnityEngine;

public class MachinePlacement : MonoBehaviour
{
    public GameObject machineStock;
    public GameObject machineToPlace;
    public GameObject machineOrigin;

    private Camera _mainCamera;
    private Ray _ray;
    private RaycastHit _hitData;
    public float distance;
    public LayerMask layerMask;

    public Boolean isMoving;
    
    private MachineCollider _collider;
    private BlueprintMode _blueprintMode;
    private Vector3 _eulerRotation;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
        _blueprintMode = this.gameObject.GetComponent<BlueprintMode>();
    }

    void OnEnable()
    {
        machineToPlace = machineStock.transform.GetChild(machineStock.transform.childCount - 1).gameObject;
        _collider = machineToPlace.GetComponent<MachineCollider>();
        _collider.ActivateHologram();
        
        if (isMoving)
        {
            machineOrigin = Instantiate(machineToPlace);

            machineOrigin.GetComponent<CapsuleCollider>().enabled = false;
            
            for (var i = 0; i < machineOrigin.GetComponents<MonoBehaviour>().Length; i++)
            {
                machineOrigin.GetComponents<MonoBehaviour>()[i].enabled = false;
            }
        }
    }

    void Update()
    {
        _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out _hitData, distance, layerMask))
        {
            machineToPlace.transform.position = _hitData.point;
        }

        _eulerRotation = machineToPlace.transform.eulerAngles;
        machineToPlace.transform.eulerAngles = new Vector3(_eulerRotation.x, _eulerRotation.y + Input.mouseScrollDelta.y * 36, _eulerRotation.z);

        _blueprintMode.previousModesRequirements[0] = _collider.canBePlaced;

        if (isMoving)
        {
            if (Input.GetKeyDown(_blueprintMode.keyCodeToPreviousModes[1]))
            {
                Destroy(machineToPlace);
            }
            
            if (Input.GetKeyDown(_blueprintMode.keyCodeToPreviousModes[2]))
            {
                machineToPlace.transform.position = machineOrigin.transform.position;
                machineToPlace.transform.rotation = machineOrigin.transform.rotation;
            }
        }
    }

    private void OnDisable()
    {
        _collider.DeactivateHologram();

        if (isMoving)
        {
            Destroy(machineOrigin);
        }
    }
}