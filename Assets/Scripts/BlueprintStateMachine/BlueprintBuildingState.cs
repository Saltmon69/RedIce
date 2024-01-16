using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class BlueprintBuildingState : BlueprintBaseState
{
    private GameObject _machineBuildingDisplay;
    private UnityEngine.Object[] _machinesPrefab;
    private GameObject _machineStock;
    private GameObject _machineSelectedPlacementMode;
    private GameObject _machineBuildingButton;
    private GameObject _thisMachineButton;
    private GameObject _playerInventory;
    private InventoryItem _thisPlayerInventoryItemList;
    private List<ItemClass> _playerItemList;
    private List<int> _playerAmountList;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(4).gameObject.SetActive(true);

        _machineStock = GameObject.Find("MachineStock");

        //active l'interface de sélection des machines
        _machineBuildingDisplay = Object.Instantiate(Resources.Load<GameObject>("MachineUI/UIMachineBuildingCanvas"));
        _machineBuildingDisplay = _machineBuildingDisplay.transform.GetChild(0).GetChild(1).gameObject;

        _machineBuildingButton = Resources.Load<GameObject>("MachineUI/MachineButton");
        
        _machinesPrefab = Resources.LoadAll<GameObject>("Machines");

        _playerInventory = GameObject.FindWithTag("Player").GetComponent<PlayerMenuing>().inventory.transform.GetChild(0).GetChild(1).gameObject;
        
        for(var i = 0; i < _playerInventory.transform.childCount; i++)
        {
            if (_playerInventory.transform.GetChild(i).childCount == 0) continue;

            _thisPlayerInventoryItemList = _playerInventory.transform.GetChild(i).GetChild(0).GetComponent<InventoryItem>();
                
            if(!_playerItemList.Contains(_thisPlayerInventoryItemList.item))
            {
                _playerItemList.Add(_thisPlayerInventoryItemList.item);
                _playerAmountList.Add(_thisPlayerInventoryItemList.count);
            }
            else
            {
                _playerAmountList[_playerItemList.IndexOf(_thisPlayerInventoryItemList.item)] += _thisPlayerInventoryItemList.count;
            }
        }

        //si les machine non pas précédement été chargé, alors on assigne chaque bouton a sa machine correspondante
        for(var i = 0; i < _machinesPrefab.Length; i++)
        {
            var a = i;
            _thisMachineButton = Object.Instantiate(_machineBuildingButton, _machineBuildingDisplay.transform); 
            _thisMachineButton.GetComponent<Button>().onClick.AddListener(() => { MachineChosen(a, blueprint); });
            _thisMachineButton.transform.GetChild(0).GetComponent<Text>().text = _machinesPrefab[a].name;
            //RecipeMaterialManager(_machinesPrefab[a].GetComponent<MachineCost>().buildingMaterialList, _machinesPrefab[a].GetComponent<MachineCost>().buildingMaterialAmountList);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //fonction sur chacun des boutons permettant de crée la machine en plus de nous faire passer au mode de placement de la machine
    void MachineChosen(int machineNumber, BlueprintStateMachineManager blueprint)
    {
        _machineSelectedPlacementMode = GameObject.Instantiate((GameObject)_machinesPrefab[machineNumber], _machineStock.transform);
        blueprint.SwitchState(blueprint.placementState);
    }
    
    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        //retour au mode de sélection de la machine a construire
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            blueprint.SwitchState(blueprint.startState);
        }
    }
    
    public override void RayState(BlueprintStateMachineManager blueprint, RaycastHit hitData, RaycastHit oldHitData, bool hadHit){}
        
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(4).gameObject.SetActive(false);
        Object.Destroy(_machineBuildingDisplay.transform.parent.parent.gameObject);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    /*
    private void RecipeMaterialManager(List<ItemClass> _materialList, List<int> _materialAmountList)
    {
        for(var i = 0; i < _materialList.Count; i++)
        {
            //regarde si les matériaux que l'on est en train de définir si il sont requis ou reçus
            if(i < _playerItemList.Count)
            {
                _recipeMaterialList[i].transform.parent.GetChild(1).gameObject.SetActive(true);
                _recipeMaterialList[i].color = new Color(1,0,0);
                _recipeMaterialList[i].text = "0";
                        
                //si l'inventaire de la machine contient ce materiaux alors il associe le nombre qu'il y en a dans l'inventaire avec l'ui 
                if(_playerItemList.Contains(_materialList[i]))
                {
                    _recipeMaterialList[i].text = _playerAmountList[_playerItemList.IndexOf(_materialList[i])] + "";
                            
                    //si il y a assez de ce materiaux pour le craft alors l'image rouge par dessus le materiaux disparait et le texte n'est plus rouge
                    if(_playerAmountList[_playerItemList.IndexOf(_materialList[i])] >= _materialAmountList[i])
                    {
                        _recipeMaterialList[i].transform.parent.GetChild(1).gameObject.SetActive(false);
                        _recipeMaterialList[i].color = new Color(0,0,0);
                    }
                }

                _recipeMaterialList[i].text += "/" + _machineCraftRecipe.inputsAmount[i] + " " + _machineCraftRecipe.inputs[i].nom;
            }
            else
            {
                //si le materiaux/objet est le resultat du craft et non requis alors on lui met juste le nombre que l'on en reçois sur le texte
                _recipeMaterialList[i].transform.parent.GetChild(1).gameObject.SetActive(true);
                _recipeMaterialList[i].text = _machineCraftRecipe.outputsAmount[i - _machineCraftRecipe.inputs.Count] + " " + _machineCraftRecipe.outputs[i - _machineCraftRecipe.inputs.Count].nom;  
            }
        }
    }*/
}