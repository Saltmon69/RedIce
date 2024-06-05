using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public class BlueprintPlacementState : BlueprintBaseState
{
    private GameObject _machineStock;
    private GameObject _machineToPlace;

    private HighlightComponent _highlightComponent;
    private MachineCollider _machineCollider;
    private Vector3 _eulerRotation;
    
    private BasePower _basePower;
    private ComputerUIDisplay _computerUIDisplay;
    private MachineCost _machineCost;
    private GameObject _playerInventory;
    private InventoryItem _thisPlayerInventoryItem;
    private List<InventoryItem> _playerInventoryItemList;
    private int _materialAmountLeft;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(5).gameObject.SetActive(true);
        
        _machineStock = GameObject.Find("MachineStock");

        //viens prendre la machine que l'on a sélectionné pour la placer / construire
        _machineToPlace = _machineStock.transform.GetChild(_machineStock.transform.childCount - 1).gameObject;
        _machineToPlace.layer = 2;

        LayerChanger(2);

        _machineCollider = _machineToPlace.transform.GetComponent<MachineCollider>();
        _machineCollider.enabled = true;

        _computerUIDisplay = GameObject.FindWithTag("Computer").GetComponent<ComputerUIDisplay>();

        _machineCost = _machineToPlace.GetComponent<MachineCost>();
        
        _playerInventory = GameObject.FindWithTag("Player").GetComponent<PlayerMenuing>().inventory.transform.GetChild(0).GetChild(1).gameObject;
        _playerInventoryItemList = new List<InventoryItem>();
        
        for(var i = 0; i < _playerInventory.transform.childCount; i++)
        {
            if(_playerInventory.transform.GetChild(i).childCount == 0) continue;
            _thisPlayerInventoryItem = _playerInventory.transform.GetChild(i).GetChild(0).GetComponent<InventoryItem>();
            _playerInventoryItemList.Add(_thisPlayerInventoryItem);
        }
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
            GameObject.DestroyImmediate(_machineToPlace);
            blueprint.SwitchState(blueprint.buildingState);
        }
    }
    
    public override void RayState(BlueprintStateMachineManager blueprint, RaycastHit hitData, RaycastHit oldHitData, bool hadHit)
    {
        if(hitData.transform.gameObject.layer == 3)
        {
            _machineToPlace.transform.position = hitData.point;
        }

        //confirmation du placement de la machine si elle peut etre placé
        //retour au mode de sélection de la machine à construire
        if(Input.GetKeyDown(KeyCode.Mouse1) && _machineCollider.canBePlaced)
        {
            if(hitData.transform.CompareTag("BaseFloor") && !_machineToPlace.CompareTag("Tirolienne") && !_machineToPlace.CompareTag("Computer"))
            {
                if (_computerUIDisplay.currentPowerUsage + _machineCost.machinePowerCost <=
                    _computerUIDisplay.maxPower)
                {
                    _computerUIDisplay.currentPowerUsage += _machineCost.machinePowerCost;

                    InventoryItemCostRemoval();
                    LayerChanger(6);
                    blueprint.SwitchState(blueprint.buildingState);
                }
                else
                {
                    _basePower = hitData.transform.GetComponent<BasePower>();
                    _basePower.Flash();
                }
            }

            if (hitData.transform.CompareTag("Ground") && _machineToPlace.CompareTag("Tirolienne"))
            {
                _machineToPlace.transform.GetComponent<TirolienneMachine>().isPlaced = true;

                InventoryItemCostRemoval();
                blueprint.SwitchState(blueprint.buildingState);
            }
                
            if(hitData.transform.CompareTag("BaseFloor") && _machineToPlace.CompareTag("Computer"))
            {
                InventoryItemCostRemoval();
                _machineToPlace.transform.position = hitData.transform.position + Vector3.up * 0.5f + Vector3.forward * 1.5f;
                blueprint.SwitchState(blueprint.buildingState);
                
            }
        }
    }
        
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(5).gameObject.SetActive(false);
        
        if(_machineToPlace != null) _machineToPlace.layer = 6;
    }

    private void InventoryItemCostRemoval()
    {
        _machineCollider.enabled = false;
        
        for(var i = 0; i < _machineCost.buildingMaterialList.Count; i++)
        {
            _materialAmountLeft = _machineCost.buildingMaterialAmountList[i];
                        
            for(var j = 0; j < _playerInventoryItemList.Count; j++)
            {
                if(_playerInventoryItemList[j].item == _machineCost.buildingMaterialList[i])
                {
                    _playerInventoryItemList[j].count -= _materialAmountLeft;
                    _materialAmountLeft -= _playerInventoryItemList[j].count + _materialAmountLeft;
                    try
                    {
                        _playerInventoryItemList[j].RefreshCount();
                        if(_playerInventoryItemList[j].count <= 0) Object.DestroyImmediate(_playerInventoryItemList[j].gameObject);
                    }catch(MissingReferenceException){}

                    if(_materialAmountLeft <= 0) break;
                }
            }
        }
    }

    private void LayerChanger(int layer)
    {
        if(_machineToPlace.transform.childCount == 0) return;
        for(var i = 0; i < _machineToPlace.transform.childCount; i++)
        {
            _machineToPlace.transform.GetChild(i).gameObject.layer = layer;
        }
    }
}
