using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestUIDisplay : MonoBehaviour
{
 private GameObject _chestUIPrefab;
    private GameObject _thisChestUIDisplay;
    
    private GameObject _chestUpgradeSlot;
    private GameObject _chestBackgroundUI;

    private InventoryItem _itemInUpgradeSlot;
    
    public List<ItemClass> chestUpgradeItemTierList;
    
    public GameObject playerInventory;
    private List<Transform> _chestPlayerInventoryList;
    private GameObject _thisChestPlayerInventorySlot;
    private GameObject _chestPlayerInventoryUI;

    private GameObject _chestInventory;
    private int _numberOfInventorySlots;

    public int upgradeState;
    private bool _hasItemInUpgradeSlot;
    private bool _upgradeItemIsSaved;

    private GameObject _chestGameObjectSelected;
    private bool _gotOpen;

    public void ActivateUIDisplay()
    {
        _chestUIPrefab = Resources.Load<GameObject>("MachineUI/UIChest");
        _thisChestUIDisplay = Instantiate(_chestUIPrefab);
        
        _chestBackgroundUI = _thisChestUIDisplay.transform.GetChild(0).GetChild(2).gameObject;
        _chestInventory = _thisChestUIDisplay.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(1).gameObject;
        _chestPlayerInventoryUI = _thisChestUIDisplay.transform.GetChild(3).GetChild(0).GetChild(0).GetChild(1).gameObject;
        _chestUpgradeSlot = _thisChestUIDisplay.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        
        _hasItemInUpgradeSlot = false;
        
        if(_gotOpen) LoadChestInventory();
        LoadPlayerInventory();
        LoadUpgradeSlot();
        StartCoroutine(UpgradeCheck());
    }

    private IEnumerator UpgradeCheck()
    {
        while(true)
        {
            //regarde si un nouvel objet a été déposer ou retiré dans le case d'upgrade, si oui, il actualise les crafts
            if(_chestUpgradeSlot.transform.childCount > 0 && !_hasItemInUpgradeSlot)
            {
                Debug.Log("an upgrade item has been placed");

                _itemInUpgradeSlot = _chestUpgradeSlot.transform.GetChild(0).GetComponent<InventoryItem>(); 
                
                if(_itemInUpgradeSlot.item == chestUpgradeItemTierList[1])
                {
                    upgradeState = 2;
                    _chestBackgroundUI.GetComponent<Image>().color = new Color(1f,0f,0f, 1f);
                }
                else if(_itemInUpgradeSlot.item == chestUpgradeItemTierList[0])
                {
                    upgradeState = 1;
                    _chestBackgroundUI.GetComponent<Image>().color = new Color(1f,0.65f,0f, 1f);
                }
                else
                {
                    upgradeState = 0;
                    _chestBackgroundUI.GetComponent<Image>().color = new Color(0.5f,0.5f,0.5f, 0f);
                }
                
                _hasItemInUpgradeSlot = true;
            }

            if(_chestUpgradeSlot.transform.childCount == 0 && _hasItemInUpgradeSlot)
            {
                Debug.Log("an upgrade item has been removed");
                upgradeState = 0;
                _chestBackgroundUI.GetComponent<Image>().color = new Color(0.5f,0.5f,0.5f, 0f);
                _hasItemInUpgradeSlot = false;
            }
            
            LoadUpgradeSlot();
                       
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void LoadUpgradeSlot()
    {
        switch(upgradeState)
        {
            case 0:
                _numberOfInventorySlots = 25;
                break;
                
            case 1:
                _numberOfInventorySlots = 42;
                break;
                
            case 2:
                _numberOfInventorySlots = 63;
                break;
        }
            
        for (var i = 0; i < _chestInventory.transform.childCount; i++)
        {
            _chestInventory.transform.GetChild(i).gameObject.SetActive(i < _numberOfInventorySlots);
        }
    }

    //prend tous les objets stocké dans l'inventaire du joueur et le met sur l'inventaire joueur qu il y a déja disposer sur la machine
    private void LoadPlayerInventory()
    {
        _chestPlayerInventoryList = new List<Transform>();
        
        for(var i = 0; i < playerInventory.transform.childCount; i++)
        {
            _thisChestPlayerInventorySlot = Instantiate(playerInventory.transform.GetChild(i).gameObject, _chestPlayerInventoryUI.transform);
            _chestPlayerInventoryList.Add(_thisChestPlayerInventorySlot.transform);
        }
    }

    private void LoadChestInventory()
    {
        _chestGameObjectSelected = this.gameObject.transform.GetChild(this.gameObject.transform.childCount - 1).gameObject;

        if(_chestGameObjectSelected.transform.childCount > 60)
        {
            _chestGameObjectSelected = this.gameObject.transform.GetChild(this.gameObject.transform.childCount - 2).gameObject;
            
            if(_chestGameObjectSelected.transform.childCount > 0)
            {
                _chestGameObjectSelected.transform.GetChild(0).SetParent(_chestUpgradeSlot.transform);
            }
            
            Destroy(_chestGameObjectSelected);
            
            _chestGameObjectSelected = this.gameObject.transform.GetChild(this.gameObject.transform.childCount - 1).gameObject;
            
            while(_chestGameObjectSelected.transform.childCount > 0)
            {
                _chestGameObjectSelected.transform.GetChild(0).SetParent(_chestInventory.transform);
                DestroyImmediate(_chestInventory.transform.GetChild(0).gameObject);
            }
            
            Destroy(_chestGameObjectSelected);
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

            if(_chestPlayerInventoryList[i].childCount > 0)
            {
                Instantiate(_chestPlayerInventoryList[i].GetChild(0).gameObject, playerInventory.transform.GetChild(i));
            }
        }
    }

    private void SaveChestInventory()
    {
        _chestUpgradeSlot.transform.SetParent(this.gameObject.transform);
        _chestInventory.transform.SetParent(this.gameObject.transform);
    }

    public void DeactivateUIDisplay()
    {
        SavePlayerInventory();
        SaveChestInventory();
        StopAllCoroutines();
        _gotOpen = true;
        Destroy(_thisChestUIDisplay);
    }
}
