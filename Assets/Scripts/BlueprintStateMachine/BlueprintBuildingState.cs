using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class BlueprintBuildingState : BlueprintBaseState
{
    private GameObject _machineBuildingDisplay;
    private GameObject[] _machinesPrefab;
    private GameObject[] _machinesPrefabTier1;
    private GameObject[] _machinesPrefabTier2;
    private GameObject[] _machinesPrefabTier3;
    private GameObject _machineStock;
    private GameObject _machineSelectedPlacementMode;
    private GameObject _machineBuildingButton;
    private GameObject _thisMachineButton;
    private GameObject _playerInventory;
    private InventoryItem _thisPlayerInventoryItem;
    private List<ItemClass> _playerItemList;
    private List<int> _playerAmountList;
    private GameObject _materialRecipePrefab;
    private GameObject _instantiatedMachineUIRecipeMaterial;
    private Text _machineRecipeMaterial;
    private GameObject _plusSignPrefab;
    private bool _hasEnoughMaterial;
    private int _materialsReady;
    private GameObject _computerMachine;
    private bool _isComputerPlaced;
    private Vector3 _machineStartPlacement;
    
    private PlayerMenuing _playerMenuing;
    private InventoryUpgrade _inventoryUpgrade;
    public int thisMachineNumber;
    
    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(4).gameObject.SetActive(true);

        _machineStock = GameObject.Find("MachineStock");
        
        _playerMenuing = GameObject.FindWithTag("Player").GetComponent<PlayerMenuing>();
        _playerMenuing.enabled = true;
        _playerMenuing.InMenu();
        _playerMenuing.enabled = false;

        //active l'interface de sélection des machines
        _machineBuildingDisplay = Object.Instantiate(Resources.Load<GameObject>("MachineUI/UIMachineBuildingCanvas"));

        _machineBuildingButton = Resources.Load<GameObject>("MachineUI/MachineButton");
        
        _machinesPrefab = Resources.LoadAll<GameObject>("Machines");
        
        _machinesPrefabTier1 = Resources.LoadAll<GameObject>("Machines/Tier1");
        _machinesPrefabTier2 = Resources.LoadAll<GameObject>("Machines/Tier2");
        _machinesPrefabTier3 = Resources.LoadAll<GameObject>("Machines/Tier3");

        _materialRecipePrefab = Resources.Load<GameObject>("MachineUI/MaterialRecipe");
        _plusSignPrefab = Resources.Load<GameObject>("MachineUI/PlusSignImage");

        _playerInventory = GameObject.FindWithTag("Player").GetComponent<PlayerMenuing>().inventory.transform.GetChild(0).GetChild(1).gameObject;

        _playerItemList = new List<ItemClass>();
        _playerAmountList = new List<int>();
            
        for(var i = 0; i < _playerInventory.transform.childCount; i++)
        {
            if(_playerInventory.transform.GetChild(i).childCount == 0) continue;

            _thisPlayerInventoryItem = _playerInventory.transform.GetChild(i).GetChild(0).GetComponent<InventoryItem>();
                
            if(!_playerItemList.Contains(_thisPlayerInventoryItem.item))
            {
                _playerItemList.Add(_thisPlayerInventoryItem.item);
                _playerAmountList.Add(_thisPlayerInventoryItem.count);
            }
            else
            {
                _playerAmountList[_playerItemList.IndexOf(_thisPlayerInventoryItem.item)] += _thisPlayerInventoryItem.count;
            }
        }

        _computerMachine = GameObject.FindWithTag("Computer");
        _isComputerPlaced = _computerMachine != null;
        
        //on assigne chaque bouton a sa machine correspondante
        for(var i = 0; i < _machinesPrefab.Length; i++)
        {
            var a = i;
            if(_machinesPrefabTier1.Contains(_machinesPrefab[i])) _thisMachineButton = Object.Instantiate(_machineBuildingButton, _machineBuildingDisplay.transform.GetChild(0).GetChild(1).gameObject.transform); 
            if(_machinesPrefabTier2.Contains(_machinesPrefab[i])) _thisMachineButton = Object.Instantiate(_machineBuildingButton, _machineBuildingDisplay.transform.GetChild(1).GetChild(1).gameObject.transform); 
            if(_machinesPrefabTier3.Contains(_machinesPrefab[i])) _thisMachineButton = Object.Instantiate(_machineBuildingButton, _machineBuildingDisplay.transform.GetChild(2).GetChild(1).gameObject.transform);

            _thisMachineButton.transform.GetComponent<Image>().sprite = _machinesPrefab[i].GetComponent<SpriteRenderer>().sprite;
            
            if(_machinesPrefab[a].CompareTag("Computer"))
            {
                _thisMachineButton.GetComponent<Button>().interactable = !_isComputerPlaced;
            }
            else
            {
                _thisMachineButton.GetComponent<Button>().interactable = _isComputerPlaced;
            }
            
            _thisMachineButton.transform.GetChild(0).GetComponent<Text>().text = _machinesPrefab[a].name;

            RecipeMaterialManager(_machinesPrefab[a].GetComponent<MachineCost>().buildingMaterialList, _machinesPrefab[a].GetComponent<MachineCost>().buildingMaterialAmountList);
            if(_hasEnoughMaterial) _thisMachineButton.GetComponent<Button>().onClick.AddListener(() => { MachineChosen(a, blueprint); });
        }

        _inventoryUpgrade = GameObject.FindWithTag("Respawn").gameObject.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<InventoryUpgrade>();

        Debug.Log(_inventoryUpgrade);
        
        //checks for upgrade to unlock new machine menus
        if(_inventoryUpgrade.upgradeItemInInventory.Contains(Resources.Load<ItemClass>("SO/Upgrades/OrangeToolModule")))
        {
            _machineBuildingDisplay.transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<Button>().interactable = true;
            
            if (_inventoryUpgrade.upgradeItemInInventory.Contains(Resources.Load<ItemClass>("SO/Upgrades/RedToolModule")))
            {
                _machineBuildingDisplay.transform.GetChild(1).GetChild(0).GetChild(3).GetComponent<Button>().interactable = true;
            }
        } 
    }

    //fonction sur chacun des boutons permettant de crée la machine en plus de nous faire passer au mode de placement de la machine
    public void MachineChosen(int machineNumber, BlueprintStateMachineManager blueprint)
    {
        thisMachineNumber = machineNumber;
        _machineSelectedPlacementMode = Object.Instantiate(_machinesPrefab[thisMachineNumber], _machineStartPlacement, quaternion.identity, _machineStock.transform);
        _playerMenuing.enabled = true;
        _playerMenuing.OutMenu();
        _playerMenuing.enabled = false;
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

    public override void RayState(BlueprintStateMachineManager blueprint, RaycastHit hitData, RaycastHit oldHitData, bool hadHit)
    {
        if(hitData.transform.gameObject.layer == 3)
        {
            _machineStartPlacement = hitData.point;
        }
    }
        
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(4).gameObject.SetActive(false);
        Object.Destroy(_machineBuildingDisplay);
    }
    
    private void RecipeMaterialManager(List<ItemClass> materialList, List<int> materialAmountList)
    {
        _hasEnoughMaterial = false;
        _materialsReady = 0;
        
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
            catch (NullReferenceException){}

            _machineRecipeMaterial.text += "/" + materialAmountList[i] + " " + materialList[i].name;
        }
        
        _hasEnoughMaterial = (_materialsReady == materialList.Count);
    }
}