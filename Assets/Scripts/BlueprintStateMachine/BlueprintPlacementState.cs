using UnityEngine;

public class BlueprintPlacementState : BlueprintBaseState
{
    public GameObject machineStock;
    public GameObject machineToPlace;
    private RaycastHit _hitData;
    public LayerMask layerMask;
    
    private HighlightComponent _highlightComponent;
    private MachineCollider _machineCollider;
    private Vector3 _eulerRotation;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(5).gameObject.SetActive(true);

        layerMask = LayerMask.GetMask("Ground");
        machineStock = GameObject.Find("MachineStock");


        machineToPlace = machineStock.transform.GetChild(machineStock.transform.childCount - 1).gameObject;

        machineToPlace.GetComponent<MachineCollider>().isActive = true;
        
        _highlightComponent = machineToPlace.GetComponent<HighlightComponent>();
        _highlightComponent.Blueprint();

        _machineCollider = machineToPlace.transform.GetComponent<MachineCollider>();
    }
    
    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        _eulerRotation = machineToPlace.transform.eulerAngles;
        machineToPlace.transform.eulerAngles = new Vector3(_eulerRotation.x, _eulerRotation.y + Input.mouseScrollDelta.y * 36, _eulerRotation.z);

        if(Input.GetKeyDown(KeyCode.Mouse1) && _machineCollider.canBePlaced)
        {
            blueprint.SwitchState(blueprint.buildingState);
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            blueprint.SwitchState(blueprint.buildingState);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            blueprint.SwitchState(blueprint.buildingState);
        }
    }
    
    public override void RayState(BlueprintStateMachineManager blueprint, Ray ray, float distance)
    {
        if (Physics.Raycast(ray, out _hitData, distance, layerMask))
        {
            machineToPlace.transform.position = _hitData.point;
        }
    }
        
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(5).gameObject.SetActive(false);

        _highlightComponent.BaseMaterial();
        
        machineToPlace.GetComponent<MachineCollider>().isActive = false;
    }
}
