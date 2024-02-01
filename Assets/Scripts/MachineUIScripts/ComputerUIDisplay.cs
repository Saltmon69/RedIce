using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ComputerUIDisplay : MonoBehaviour
{
    private GameObject _computerUIPrefab;
    private GameObject _thisComputerUIDisplay;
    private List<GameObject> _computerUpgradeSlotUIList;

    private List<InventoryItem> _itemInUpgradeSlotList;
    
    public List<ItemClass> computerUpgradeItemTierList;
    
    public GameObject playerInventory;
    private List<Transform> _computerPlayerInventoryList;
    private GameObject _thisComputerPlayerInventorySlot;
    private GameObject _computerPlayerInventoryUI;
    
    public GameObject thisBase;

    public int upgradeState;
    private bool _isItemRemoved;
    
    public int maxPower;
    public int currentPowerUsage;


    public void Awake()
    {
        thisBase = GameObject.FindWithTag("BaseFloor");
    }

    public void ActivateUIDisplay()
    {
        _computerUIPrefab = Resources.Load<GameObject>("MachineUI/UIComputer");
        _thisComputerUIDisplay = Instantiate(_computerUIPrefab);

        _computerUpgradeSlotUIList = new List<GameObject>();
        _itemInUpgradeSlotList = new List<InventoryItem>(new InventoryItem[_thisComputerUIDisplay.transform.childCount - 2]);

        for(var i = 2; i < _thisComputerUIDisplay.transform.childCount; i++)
        {
            _computerUpgradeSlotUIList.Add(_thisComputerUIDisplay.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject);
            if(i > 2 + upgradeState) _thisComputerUIDisplay.transform.GetChild(i).gameObject.SetActive(false);
        }

        _computerPlayerInventoryUI = _thisComputerUIDisplay.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).gameObject;
        
        LoadComputerInventory();
        LoadPlayerInventory();
        StartCoroutine(UpgradeCheck());
        MaxPowerUpgrade();
    }

    private IEnumerator UpgradeCheck()
    {
        while(true)
        {
            if(_computerUpgradeSlotUIList[upgradeState].transform.childCount == 0 && !_isItemRemoved)
            {
                _isItemRemoved = true;
                Debug.Log("an item has been removed");
                _itemInUpgradeSlotList[upgradeState] = null;
            }

            if(_computerUpgradeSlotUIList[upgradeState].transform.childCount > 0 && _isItemRemoved)
            {
                _isItemRemoved = false;
                Debug.Log("a new item has been added");
                
                _itemInUpgradeSlotList[upgradeState] = _computerUpgradeSlotUIList[upgradeState].transform.GetChild(0).GetComponent<InventoryItem>();
                
                if(_itemInUpgradeSlotList[upgradeState].item == computerUpgradeItemTierList[upgradeState])
                {
                    Debug.Log("the right upgrade has been added");
                    if(upgradeState + 3 < _computerUpgradeSlotUIList.Count) _thisComputerUIDisplay.transform.GetChild(upgradeState + 3).gameObject.SetActive(true);
                    thisBase.transform.GetChild(upgradeState).gameObject.SetActive(true);
                    upgradeState++;
                    MaxPowerUpgrade();
                }
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    private void MaxPowerUpgrade()
    {
        switch (upgradeState)
        {
            case 0:
                maxPower = 50;
                break;
            case 1:
                maxPower = 70;
                break;
            case 2:
                maxPower = 90;
                break;
            case 3:
                maxPower = 110;
                break;
            case 4:
                maxPower = 140;
                break;
            case 5:
                maxPower = 180;
                break;
        }
    }

    //prend tous les objets stocké dans l'inventaire du joueur et le met sur l'inventaire joueur qu il y a déja disposer sur la machine
    private void LoadPlayerInventory()
    {
        _computerPlayerInventoryList = new List<Transform>();
        
        for(var i = 0; i < playerInventory.transform.childCount; i++)
        {
            _thisComputerPlayerInventorySlot = Instantiate(playerInventory.transform.GetChild(i).gameObject, _computerPlayerInventoryUI.transform);
            _computerPlayerInventoryList.Add(_thisComputerPlayerInventorySlot.transform);
        }
    }

    private void LoadComputerInventory()
    {
        if(this.gameObject.transform.childCount == 1) return;
        for(var i = 0; i < upgradeState; i++)
        {
            this.gameObject.transform.GetChild(1).SetParent(_computerUpgradeSlotUIList[i].transform);
            _itemInUpgradeSlotList[i] = _computerUpgradeSlotUIList[i].transform.GetChild(0).GetComponent<InventoryItem>();
        }
    }
    
    //change tous les objets que contient l inventaire du joueur par l'inventaire joueur qu'il y a sur la machine
    private void SavePlayerInventory()
    {
        for(var i = 0; i < playerInventory.transform.childCount; i++)
        {
            if(playerInventory.transform.GetChild(i).childCount > 0)
            {
                Destroy(playerInventory.transform.GetChild(i).GetChild(0).gameObject);
            }

            if(_computerPlayerInventoryList[i].childCount > 0)
            {
                Instantiate(_computerPlayerInventoryList[i].GetChild(0).gameObject, playerInventory.transform.GetChild(i));
            }
        }
    }

    private void SaveComputerInventory()
    {
        for(var i = 0; i < _itemInUpgradeSlotList.Count; i++)
        {
            try
            {
                _itemInUpgradeSlotList[i].transform.SetParent(this.gameObject.transform);
            }catch(NullReferenceException){}
        }
    }

    public void DeactivateUIDisplay()
    {
        SavePlayerInventory();
        SaveComputerInventory();
        StopAllCoroutines();
        Destroy(_thisComputerUIDisplay);
    }
}
