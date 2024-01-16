using UnityEngine;

public class BlueprintDisplacementState : BlueprintBaseState
{
    private GameObject _machineStock;
    private GameObject _machineToPlace;
    private GameObject _fakeMachineHologram;

    private HighlightComponent _highlightComponent;
    private MachineCollider _machineCollider;
    private Vector3 _eulerRotation;

    private MachineUIDisplay _machineUIDisplay;
    private ComputerUIDisplay _computerUIDisplay;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(3).gameObject.SetActive(true);

        _machineStock = GameObject.Find("MachineStock");

        //retrouve la machine que l on a selectionner grace a son changement dans sa hiérarchie grace au dernier etat
        _machineToPlace = _machineStock.transform.GetChild(_machineStock.transform.childCount - 1).gameObject;
        _machineToPlace.layer = 2;
        _machineUIDisplay = _machineToPlace.GetComponent<MachineUIDisplay>();
        
        _machineCollider = _machineToPlace.transform.GetComponent<MachineCollider>();
        _machineCollider.enabled = true;

        _highlightComponent = _machineToPlace.GetComponent<HighlightComponent>();
        _highlightComponent.Blueprint();

        _computerUIDisplay = GameObject.Find("ComputerAndBase").GetComponent<ComputerUIDisplay>();
        
        //crée une fausse machine permettant de visulalizer la position initiale de l objet avant de le bouger
        _fakeMachineHologram = GameObject.Instantiate(_machineToPlace);
        _fakeMachineHologram.GetComponent<MachineCollider>().IsTrigger(true);
        _fakeMachineHologram.GetComponent<MachineCollider>().enabled = false;
    }

    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        //calcule une rotation de notre machine en utilisant la molette de la souris
        _eulerRotation = _machineToPlace.transform.eulerAngles;
        _machineToPlace.transform.eulerAngles = new Vector3(_eulerRotation.x, _eulerRotation.y + Input.mouseScrollDelta.y * 36, _eulerRotation.z);

        //supprimer / récuperer la machine
        //retour au mode de sélection de la machine à déplacer
        if(Input.GetKeyDown(KeyCode.X))
        {
            _computerUIDisplay.currentPowerUsage -= _machineUIDisplay.machinePowerCost;
            GameObject.Destroy(_machineToPlace);
            blueprint.SwitchState(blueprint.moveState);
        }

        //annuler le déplacement de la machine (elle retourne donc a son emplacement initial)
        //retour au mode de sélection de la machine à déplacer
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _machineToPlace.transform.position = _fakeMachineHologram.transform.position;
            _machineToPlace.transform.rotation = _fakeMachineHologram.transform.rotation;
            blueprint.SwitchState(blueprint.moveState);
        }
    }

    public override void RayState(BlueprintStateMachineManager blueprint, RaycastHit hitData, RaycastHit oldHitData, bool hadHit)
    {
        if (hitData.transform.gameObject.layer == 3)
        {
            _machineToPlace.transform.position = hitData.point;
        }
        
        //confirmation du placement de la machine si elle peut etre placé
        //retour au mode de sélection de la machine à déplacer
        if(Input.GetKeyDown(KeyCode.Mouse1) && _machineCollider.canBePlaced)
        {
            blueprint.SwitchState(blueprint.moveState);
            _machineCollider.enabled = false;
            _highlightComponent.BaseMaterial();
        }
    }
        
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(3).gameObject.SetActive(false);

        _machineCollider.enabled = false;
        _highlightComponent.BaseMaterial();
        _machineToPlace.layer = 6;
        
        GameObject.Destroy(_fakeMachineHologram);
    }
}
