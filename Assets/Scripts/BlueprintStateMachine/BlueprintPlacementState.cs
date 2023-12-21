using UnityEngine;

public class BlueprintPlacementState : BlueprintBaseState
{
    private GameObject _machineStock;
    private GameObject _machineToPlace;

    private HighlightComponent _highlightComponent;
    private MachineCollider _machineCollider;
    private Vector3 _eulerRotation;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(5).gameObject.SetActive(true);
        
        _machineStock = GameObject.Find("MachineStock");

        //viens prendre la machine que l'on a sélectionné pour la placer / construire
        _machineToPlace = _machineStock.transform.GetChild(_machineStock.transform.childCount - 1).gameObject;
        _machineToPlace.layer = 2;
        
        _machineCollider = _machineToPlace.transform.GetComponent<MachineCollider>();
        _machineCollider.enabled = true;
        
        
        _highlightComponent = _machineToPlace.GetComponent<HighlightComponent>();
        _highlightComponent.Blueprint();
    }
    
    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        //calcule une rotation de notre machine en utilisant la molette de la souris
        _eulerRotation = _machineToPlace.transform.eulerAngles;
        _machineToPlace.transform.eulerAngles = new Vector3(_eulerRotation.x, _eulerRotation.y + Input.mouseScrollDelta.y * 36, _eulerRotation.z);

        //annuler la construction de la machine (elle retourne donc a son emplacement initial)
        //retour au mode de sélection de la machine à construire
        if(Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape))
        {
            blueprint.SwitchState(blueprint.buildingState);
            GameObject.Destroy(_machineToPlace);
        }
    }
    
    public override void RayState(BlueprintStateMachineManager blueprint, RaycastHit hitData, RaycastHit oldHitData, bool hadHit)
    {
        if (hitData.transform.gameObject.layer == 3)
        {
            _machineToPlace.transform.position = hitData.point;
        }
        
        //confirmation du placement de la machine si elle peut etre placé
        //retour au mode de sélection de la machine à construire
        if(Input.GetKeyDown(KeyCode.Mouse1) && _machineCollider.canBePlaced)
        {
            if (hitData.transform.CompareTag("BaseFloor"))
            {
                blueprint.SwitchState(blueprint.buildingState);
                _machineCollider.enabled = false;
                _highlightComponent.BaseMaterial();
            }
        }
    }
        
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(5).gameObject.SetActive(false);
        
        _machineToPlace.layer = 6;
    }
}
