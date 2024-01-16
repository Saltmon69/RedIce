using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class BlueprintBuildingState : BlueprintBaseState
{
    private GameObject _machineBuildingDisplay;
    private GameObject[] _machinesPrefab;
    private GameObject _machineStock;
    private GameObject _machineSelectedPlacementMode;
    private GameObject _machineBuildingButton;
    private GameObject _thisMachineButton;
    private GameObject _playerInventory;
    private InventoryItem _thisPlayerInventoryItemList;
    private List<ItemClass> _playerItemList;
    private List<int> _playerAmountList;
    private GameObject _materialRecipePrefab;
    private GameObject _instantiatedMachineUIRecipeMaterial;
    private Text _machineRecipeMaterial;
    private GameObject _plusSignPrefab;
    private bool _hasEnoughMaterial;
    private int _materialsReady;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(4).gameObject.SetActive(true);

        _machineStock = GameObject.Find("MachineStock");

        //active l'interface de sélection des machines
        _machineBuildingDisplay = Object.Instantiate(Resources.Load<GameObject>("MachineUI/UIMachineBuildingCanvas"));
        _machineBuildingDisplay = _machineBuildingDisplay.transform.GetChild(0).GetChild(1).gameObject;

        _machineBuildingButton = Resources.Load<GameObject>("MachineUI/MachineButton");
        _machinesPrefab = Resources.LoadAll<GameObject>("Machines");
        _materialRecipePrefab = Resources.Load<GameObject>("MachineUI/MaterialRecipe");
        _plusSignPrefab = Resources.Load<GameObject>("MachineUI/PlusSignImage");

        _playerInventory = GameObject.FindWithTag("Player").GetComponent<PlayerMenuing>().inventory.transform.GetChild(0).GetChild(1).gameObject;

        _playerItemList = new List<ItemClass>();
        _playerAmountList = new List<int>();
            
        for(var i = 0; i < _playerInventory.transform.childCount; i++)
        {
            if(_playerInventory.transform.GetChild(i).childCount == 0) continue;

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
            _thisMachineButton.transform.GetChild(0).GetComponent<Text>().text = _machinesPrefab[a].name;
            _hasEnoughMaterial = false;
            
            RecipeMaterialManager(_machinesPrefab[a].GetComponent<MachineCost>().buildingMaterialList, _machinesPrefab[a].GetComponent<MachineCost>().buildingMaterialAmountList);
            
            if(_hasEnoughMaterial) _thisMachineButton.GetComponent<Button>().onClick.AddListener(() => { MachineChosen(a, blueprint); });
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //fonction sur chacun des boutons permettant de crée la machine en plus de nous faire passer au mode de placement de la machine
    public void MachineChosen(int machineNumber, BlueprintStateMachineManager blueprint)
    {
        _machineSelectedPlacementMode = Object.Instantiate(_machinesPrefab[machineNumber], _machineStock.transform);
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
    
    private void RecipeMaterialManager(List<ItemClass> materialList, List<int> materialAmountList)
    {
        for(var i = 0; i < materialList.Count; i++)
        {
            _instantiatedMachineUIRecipeMaterial = Object.Instantiate(_materialRecipePrefab, _thisMachineButton.transform.GetChild(1));
            _machineRecipeMaterial = _instantiatedMachineUIRecipeMaterial.transform.GetChild(2).GetComponent<Text>();

            _instantiatedMachineUIRecipeMaterial.transform.GetChild(0).GetComponent<Image>().sprite = materialList[i].sprite;

            if(i != materialList.Count - 1)
            {
                Object.Instantiate(_plusSignPrefab, _thisMachineButton.transform.GetChild(1));
            }
            
            _machineRecipeMaterial.transform.parent.GetChild(1).gameObject.SetActive(true);
            _machineRecipeMaterial.color = new Color(1,0,0);
            _machineRecipeMaterial.text = "0";
            
            //si l'inventaire de la machine contient ce materiaux alors il associe le nombre qu'il y en a dans l'inventaire avec l'ui 
            try
            {
                if (_playerItemList.Contains(materialList[i]))
                {
                    _machineRecipeMaterial.text = _playerAmountList[_playerItemList.IndexOf(materialList[i])] + "";

                    //si il y a assez de ce materiaux pour le craft alors l'image rouge par dessus le materiaux disparait et le texte n'est plus rouge
                    if(_playerAmountList[_playerItemList.IndexOf(materialList[i])] >= materialAmountList[i])
                    {
                        _machineRecipeMaterial.transform.parent.GetChild(1).gameObject.SetActive(false);
                        _machineRecipeMaterial.color = new Color(0, 0, 0);
                        _materialsReady++;
                    }
                }
            }
            catch (NullReferenceException)
            {
                _hasEnoughMaterial = false;
            }

            _machineRecipeMaterial.text += "/" + materialAmountList[i] + " " + materialList[i].nom;
        }

        _hasEnoughMaterial = _materialsReady == materialList.Count;
    }
}