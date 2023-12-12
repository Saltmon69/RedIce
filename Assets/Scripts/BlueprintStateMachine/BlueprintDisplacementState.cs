using System;
using UnityEngine;

public class BlueprintDisplacementState : BlueprintBaseState
{
    public GameObject machineStock;
    public GameObject machineToPlace;
    public GameObject fakeMachineHologram;
    private RaycastHit _hitData;
    public LayerMask layerMask;
    
    private HighlightComponent _highlightComponent;
    private MachineCollider _machineCollider;
    private Vector3 _eulerRotation;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(3).gameObject.SetActive(true);
        layerMask = LayerMask.GetMask("Ground");
        machineStock = GameObject.Find("MachineStock");

        //retrouve la machine que l on a selectionner grace a son changement dans sa hiérarchie grace au dernier etat
        machineToPlace = machineStock.transform.GetChild(machineStock.transform.childCount - 1).gameObject;

        _machineCollider = machineToPlace.transform.GetComponent<MachineCollider>();
        _machineCollider.isActive = true;

        _highlightComponent = machineToPlace.GetComponent<HighlightComponent>();
        _highlightComponent.Blueprint();

        //crée une fausse machine permettant de visulalizer la position initiale de l objet avant de le bouger
        fakeMachineHologram = GameObject.Instantiate(machineToPlace);
        fakeMachineHologram.GetComponent<Collider>().enabled = false;
    }

    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        //calcule une rotation de notre machine en utilisant la molette de la souris
        _eulerRotation = machineToPlace.transform.eulerAngles;
        machineToPlace.transform.eulerAngles = new Vector3(_eulerRotation.x, _eulerRotation.y + Input.mouseScrollDelta.y * 36, _eulerRotation.z);

        //confirmation du placement de la machine si elle peut etre placé
        //retour au mode de sélection de la machine à déplacer
        if(Input.GetKeyDown(KeyCode.Mouse1) && _machineCollider.canBePlaced)
        {
            if (_hitData.transform.CompareTag("BaseFloor"))
            {
                blueprint.SwitchState(blueprint.moveState);
            }
        }

        //supprimer / récuperer la machine
        //retour au mode de sélection de la machine à déplacer
        if(Input.GetKeyDown(KeyCode.X))
        {
            GameObject.Destroy(machineToPlace);
            blueprint.SwitchState(blueprint.moveState);
        }

        //annuler le déplacement de la machine (elle retourne donc a son emplacement initial)
        //retour au mode de sélection de la machine à déplacer
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            machineToPlace.transform.position = fakeMachineHologram.transform.position;
            machineToPlace.transform.rotation = fakeMachineHologram.transform.rotation;
            blueprint.SwitchState(blueprint.moveState);
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
        GameObject.Find("UIStateCanvas").transform.GetChild(3).gameObject.SetActive(false);

        _machineCollider.isActive = false;
        _highlightComponent.BaseMaterial();
        
        GameObject.Destroy(fakeMachineHologram);
    }
}
