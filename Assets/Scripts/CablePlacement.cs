using UnityEngine;

public class CablePlacement : MonoBehaviour
{
    public GameObject cableStock;
    public GameObject thisCable;
    public GameObject checkpoint;

    public float checkpointOffset;
    
    private GameObject _thisCheckpoint;
    
    private Camera _mainCamera;
    private Ray _ray;
    private RaycastHit _hitData;
    public float distance;
    public LayerMask layerMask;

    private MachineCollider _checkpointCollider;
    private BlueprintMode _blueprintMode;
    private CableLaserBehaviour _cableLaser;
    private void OnEnable()
    {
        _mainCamera = Camera.main;
        _blueprintMode = this.gameObject.GetComponent<BlueprintMode>();
        
        thisCable = cableStock.transform.GetChild(cableStock.transform.childCount - 1).gameObject;
        
        _cableLaser = thisCable.transform.GetComponent<CableLaserBehaviour>();
        _thisCheckpoint = Instantiate(checkpoint, thisCable.transform);
        _checkpointCollider = _thisCheckpoint.GetComponent<MachineCollider>();
        _checkpointCollider.isActive = true;

    }
    
    void Update()
    {
        _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out _hitData, distance, layerMask))
        {
            _thisCheckpoint.transform.position = _hitData.point + Vector3.up * checkpointOffset;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && _checkpointCollider.canBePlaced)
        {
            _checkpointCollider.isActive = false;
            _thisCheckpoint = Instantiate(checkpoint, thisCable.transform);
            _checkpointCollider = _thisCheckpoint.GetComponent<MachineCollider>();
            _checkpointCollider.isActive = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(thisCable.transform.GetChild(thisCable.transform.childCount - 2).gameObject);
        }
        
        _blueprintMode.previousModesRequirements[0] = _cableLaser.isLinked;
    }
}
