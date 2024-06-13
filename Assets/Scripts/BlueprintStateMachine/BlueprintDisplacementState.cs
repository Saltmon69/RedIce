using System.Collections.Generic;
using UnityEngine;
public class BlueprintDisplacementState : BlueprintBaseState
{
    private GameObject _machineStock;
    private GameObject _machineToPlace;
    private GameObject _fakeMachineHologram;
    
    private MachineCollider _machineCollider;
    private Vector3 _eulerRotation;

    private MachineUIDisplay _machineUIDisplay;
    private ComputerUIDisplay _computerUIDisplay;
    private MachineCost _machineCost;

    private GameObject _playerInventory;
    private InventoryItem _thisPlayerInventoryItem;
    private List<InventoryItem> _playerInventoryItemList;
    private int _materialAmountLeft;

    private GameObject _inventoryItemPrefab;
    private InventoryItem _thisInventoryItem;

    private GeneratorUIDisplay _generatorUIDisplay;
    
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
        
        _computerUIDisplay = GameObject.FindWithTag("Computer").GetComponent<ComputerUIDisplay>();
        _generatorUIDisplay = GameObject.FindWithTag("Generator").GetComponent<GeneratorUIDisplay>();

        _machineCost = _machineToPlace.GetComponent<MachineCost>();
        
        _inventoryItemPrefab = Resources.Load<GameObject>("MachineUI/InventoryItem");

        _playerInventory = GameObject.FindWithTag("Player").GetComponent<PlayerMenuing>().inventory.transform.GetChild(0).GetChild(1).gameObject;
        _playerInventoryItemList = new List<InventoryItem>();
        
        for(var i = 0; i < _playerInventory.transform.childCount; i++)
        {
            if(_playerInventory.transform.GetChild(i).childCount == 0) continue;
            _thisPlayerInventoryItem = _playerInventory.transform.GetChild(i).GetChild(0).GetComponent<InventoryItem>();
            _playerInventoryItemList.Add(_thisPlayerInventoryItem);
        }
        
        //crée une fausse machine permettant de visulalizer la position initiale de l objet avant de le bouger
        _fakeMachineHologram = Object.Instantiate(_machineToPlace, _machineStock.transform);
        _fakeMachineHologram.GetComponent<MachineCollider>().IsTrigger(true);
        _fakeMachineHologram.GetComponent<MachineCollider>().enabled = false;
        _fakeMachineHologram.GetComponent<HighlightComponent>().Blueprint();
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
            if(_machineToPlace.CompareTag("Untagged")) if(_machineToPlace.GetComponent<MachineUIDisplay>().machineItemList.Count > 0) return;
            if(_machineToPlace.CompareTag("Chest")) return;
            
            _computerUIDisplay.currentPowerUsage -= _machineCost.machinePowerCost;
            RecoverMachineMaterials();
            if(_generatorUIDisplay.machineList.Contains(_machineToPlace)) _generatorUIDisplay.machineList.Remove(_machineToPlace);
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
        }
    }
        
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(3).gameObject.SetActive(false);

        _machineCollider.enabled = false;
        _machineToPlace.layer = 6;
        
        GameObject.Destroy(_fakeMachineHologram);
    }

    private void RecoverMachineMaterials()
    {
        for(var i = 0; i < _machineCost.buildingMaterialList.Count; i++)
        {
            _materialAmountLeft = _machineCost.buildingMaterialAmountList[i];
                        
            for(var j = 0; j < _playerInventoryItemList.Count; j++)
            {
                if(_playerInventoryItemList[j].item == _machineCost.buildingMaterialList[i])
                {
                    _playerInventoryItemList[j].count += _materialAmountLeft;
                    _materialAmountLeft = 0;
                    if(_playerInventoryItemList[j].count > _playerInventoryItemList[j].stackSize)
                    {
                        _materialAmountLeft = _playerInventoryItemList[j].stackSize - _playerInventoryItemList[j].count;
                        _playerInventoryItemList[j].count = _playerInventoryItemList[j].stackSize;
                    }
                    _playerInventoryItemList[j].RefreshCount();

                    if(_materialAmountLeft <= 0) break;
                }
            }

            if (_materialAmountLeft > 0)
            {
                for (var j = 0; j < _playerInventory.transform.childCount; j++)
                {
                    if(_playerInventory.transform.GetChild(j).childCount == 0)
                    {
                        _thisInventoryItem = Object.Instantiate(_inventoryItemPrefab, _playerInventory.transform.GetChild(j)).GetComponent<InventoryItem>();
                        _thisInventoryItem.InitialiseItem(_machineCost.buildingMaterialList[i]);
                        _thisInventoryItem.count = _materialAmountLeft;
                        _thisInventoryItem.RefreshCount();
                        _materialAmountLeft = 0;
                        break;
                    }
                }
            }
        }
    }
}
