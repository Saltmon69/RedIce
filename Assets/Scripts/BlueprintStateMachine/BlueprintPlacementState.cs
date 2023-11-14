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

        //viens prendre la machine que l'on a sélectionné pour la placer / construire
        machineToPlace = machineStock.transform.GetChild(machineStock.transform.childCount - 1).gameObject;

        _machineCollider = machineToPlace.transform.GetComponent<MachineCollider>();
        _machineCollider.isActive = true;
        
        _highlightComponent = machineToPlace.GetComponent<HighlightComponent>();
        _highlightComponent.Blueprint();
    }
    
    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        //calcule une rotation de notre machine en utilisant la molette de la souris
        _eulerRotation = machineToPlace.transform.eulerAngles;
        machineToPlace.transform.eulerAngles = new Vector3(_eulerRotation.x, _eulerRotation.y + Input.mouseScrollDelta.y * 36, _eulerRotation.z);

        //confirmation du placement de la machine si elle peut etre placé
        //retour au mode de sélection de la machine à construire
        if(Input.GetKeyDown(KeyCode.Mouse1) && _machineCollider.canBePlaced)
        {
            blueprint.SwitchState(blueprint.buildingState);
            _machineCollider.isActive = false;
            _highlightComponent.BaseMaterial();
        }

        //annuler la construction de la machine (elle retourne donc a son emplacement initial)
        //retour au mode de sélection de la machine à construire
        if(Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape))
        {
            blueprint.SwitchState(blueprint.buildingState);
            GameObject.Destroy(machineToPlace);
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
    }
}
