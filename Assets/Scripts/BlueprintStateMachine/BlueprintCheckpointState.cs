using UnityEngine;

public class BlueprintCheckpointState : BlueprintBaseState
{
    public GameObject cableStock;
    public GameObject thisCable;
    public Object checkpoint;
    
    private GameObject _thisCheckpoint;
    private RaycastHit _hitData;
    public LayerMask layerMask;

    private MachineCollider _checkpointCollider;
    private CableLaserBehaviour _cableLaser;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(8).gameObject.SetActive(true);
        
        layerMask = LayerMask.GetMask("Ground");
        cableStock = GameObject.Find("CableStock");

        thisCable = cableStock.transform.GetChild(cableStock.transform.childCount - 1).gameObject;
        
        checkpoint = Resources.Load("Cables/Checkpoint", typeof(GameObject));

        _cableLaser = thisCable.transform.GetComponent<CableLaserBehaviour>();
        _thisCheckpoint = GameObject.Instantiate((GameObject)checkpoint, thisCable.transform);
        _checkpointCollider = _thisCheckpoint.GetComponent<MachineCollider>();
        _checkpointCollider.isActive = true;
        
        GameObject.Destroy(thisCable.transform.GetChild(thisCable.transform.childCount - 2).gameObject);
    }
    
    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        if(Input.GetKeyDown(KeyCode.Mouse1) && _cableLaser.isLinked)
        {
            blueprint.SwitchState(blueprint.cableState);
        }
    }
    
    public override void RayState(BlueprintStateMachineManager blueprint, Ray ray, float distance)
    {
        if (Physics.Raycast(ray, out _hitData, distance, layerMask))
        {
            _thisCheckpoint.transform.position = _hitData.point + Vector3.up * 1;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && _checkpointCollider.canBePlaced)
        {
            GameObject.Instantiate((GameObject)checkpoint,  _thisCheckpoint.transform.position, Quaternion.identity, thisCable.transform);
            _thisCheckpoint.transform.SetSiblingIndex(_thisCheckpoint.transform.parent.childCount - 1);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject.Destroy(thisCable.transform.GetChild(thisCable.transform.childCount - 2).gameObject);
        }
    }
    
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(8).gameObject.SetActive(false);
    }
}
