using UnityEngine;

public class MachinePlacement : MonoBehaviour
{
    public GameObject machineStock;
    public GameObject machineToPlace;

    private Camera _mainCamera;
    private Ray _ray;
    private RaycastHit _hitData;
    public float distance;
    public LayerMask layerMask;

    private MachineCollider _collider;
    private BlueprintMode _blueprintMode;
    private void Awake()
    {
        _mainCamera = Camera.main;
        _blueprintMode = this.gameObject.GetComponent<BlueprintMode>();
    }

    void OnEnable()
    {
        machineToPlace = machineStock.transform.GetChild(machineStock.transform.childCount - 1).gameObject;
        _collider = machineToPlace.GetComponent<MachineCollider>();
    }

    void Update()
    {
        _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out _hitData, distance, layerMask))
        {
            machineToPlace.transform.position = _hitData.point;
        }
        
        _blueprintMode.previousModesRequirements[0] = _collider.canBePlaced;
    }
}