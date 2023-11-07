using UnityEngine;

public class MachinePlacement : MonoBehaviour
{
    public GameObject machineStock;
    public GameObject machineToPlace;
    public GameObject fakeMachineHologram;

    private Camera _mainCamera;
    private Ray _ray;
    private RaycastHit _hitData;
    public float distance;
    public LayerMask layerMask;

    public bool isMoving;
    
    private HighlightComponent _highlightComponent;
    private MachineCollider _machineCollider;
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

        machineToPlace.GetComponent<MachineCollider>().isActive = true;
        
        _highlightComponent = machineToPlace.GetComponent<HighlightComponent>();
        _highlightComponent.Blueprint();

        _machineCollider = machineToPlace.transform.GetComponent<MachineCollider>();
        
        if (isMoving)
        {
            fakeMachineHologram = Instantiate(machineToPlace);

            fakeMachineHologram.GetComponent<Collider>().enabled = false;
            
            for (var i = 0; i < fakeMachineHologram.GetComponents<MonoBehaviour>().Length; i++)
            {
                fakeMachineHologram.GetComponents<MonoBehaviour>()[i].enabled = false;
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

        _blueprintMode.previousModesRequirements[0] = _machineCollider.canBePlaced;

        if (isMoving)
        {
            if (Input.GetKeyDown(_blueprintMode.keyCodeToPreviousModes[1]))
            {
                Destroy(machineToPlace);
            }
            
            if (Input.GetKeyDown(_blueprintMode.keyCodeToPreviousModes[2]))
            {
                machineToPlace.transform.position = fakeMachineHologram.transform.position;
                machineToPlace.transform.rotation = fakeMachineHologram.transform.rotation;
            }
        }
    }

    private void OnDisable()
    {
        _highlightComponent.BaseMaterial();

        if (isMoving)
        {
            Destroy(fakeMachineHologram);
        }
        
        machineToPlace.GetComponent<MachineCollider>().isActive = false;
    }
}