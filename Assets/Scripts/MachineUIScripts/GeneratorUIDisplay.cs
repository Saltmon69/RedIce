using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorUIDisplay : MonoBehaviour
{
    private GameObject _generatorUIPrefab;
    private GameObject _thisGeneratorUIDisplay;
    private GameObject _generatorUpgradeSlotUI;

    private InventoryItem _itemInMeltingSlot;

    public GameObject playerInventory;
    private List<Transform> _computerPlayerInventoryList;
    private GameObject _thisComputerPlayerInventorySlot;
    private GameObject _computerPlayerInventoryUI;
    private Slider _thisSlider;
    
    private ComputerUIDisplay _computerUIDisplay;
    
    public float meltingBaseTime;
    
    public float meltingTime;
    
    private bool _isItemRemoved;
    private bool _isItemMelting;

    public int itemCount;

    public bool isUIUp;
    
    public void Awake()
    {
        _computerUIDisplay = GameObject.FindWithTag("Computer").GetComponent<ComputerUIDisplay>();
        _isItemRemoved = true;
    }
    
    public void ActivateUIDisplay()
    {
        _generatorUIPrefab = Resources.Load<GameObject>("MachineUI/UIGenerator");
        _thisGeneratorUIDisplay = Instantiate(_generatorUIPrefab);
        _thisGeneratorUIDisplay.GetComponent<Canvas>().worldCamera = Camera.main.transform.GetChild(0).GetComponent<Camera>();
        _thisGeneratorUIDisplay.GetComponent<Canvas>().planeDistance = 5;
        
        _generatorUpgradeSlotUI = _thisGeneratorUIDisplay.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        _computerPlayerInventoryUI = _thisGeneratorUIDisplay.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).gameObject;
        _thisSlider = _thisGeneratorUIDisplay.transform.GetChild(3).GetComponent<Slider>();
        _thisSlider.maxValue = meltingBaseTime;
        isUIUp = true;
        
        LoadGeneratorInventory();
        LoadPlayerInventory();
        StartCoroutine(UpgradeCheck());
        StartCoroutine(MeltingProcess());
    }
    
    private IEnumerator UpgradeCheck()
    {
        while(true)
        {
            if(_generatorUpgradeSlotUI.transform.childCount == 0 && !_isItemRemoved && !_isItemMelting)
            {
                Debug.Log("an item has been removed");
                _computerUIDisplay.maxPower -= _itemInMeltingSlot.item.atomicMass;
                _isItemRemoved = true;
                _itemInMeltingSlot = null;
            }

            if(_generatorUpgradeSlotUI.transform.childCount > 0 && _isItemRemoved && !_isItemMelting)
            {
                _isItemRemoved = false;
                _itemInMeltingSlot = _generatorUpgradeSlotUI.transform.GetChild(0).GetComponent<InventoryItem>();
                _computerUIDisplay.maxPower += _itemInMeltingSlot.item.atomicMass;
                itemCount = _itemInMeltingSlot.count;
                Debug.Log("a new item has been added");
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator MeltingProcess()
    {
        while(true)
        {
            if(meltingTime <= 0)
            {
                if(_generatorUpgradeSlotUI.transform.childCount > 0 && _itemInMeltingSlot.item.atomicMass > 0)
                {
                    _isItemMelting = true;
                    meltingTime = meltingBaseTime;
                    
                    _computerUIDisplay.maxPower -= _itemInMeltingSlot.item.atomicMass;
                    _itemInMeltingSlot = _generatorUpgradeSlotUI.transform.GetChild(0).GetComponent<InventoryItem>();
                    _computerUIDisplay.maxPower += _itemInMeltingSlot.item.atomicMass;
                    
                    _itemInMeltingSlot.count--;
                    itemCount = _itemInMeltingSlot.count;
                    _itemInMeltingSlot.RefreshCount();
                    
                    if(_itemInMeltingSlot.count <= 0)
                    {
                        Destroy(_itemInMeltingSlot.gameObject);
                    }
                }
                else
                {
                    _isItemMelting = false;
                }
            }

            _thisSlider.value = meltingTime;
            
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void Update()
    {
        if(meltingTime <= 0)
        {
            if(_isItemMelting && !isUIUp)
            {
                itemCount -= 1;
                meltingTime = meltingBaseTime;
            
                if(itemCount == 0)
                {
                    _isItemMelting = false;
                }
            }
        }
        else
        {
            meltingTime -= Time.deltaTime;
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

    private void LoadGeneratorInventory()
    {
        if(this.gameObject.transform.childCount == 1) return;
        this.gameObject.transform.GetChild(1).SetParent(_generatorUpgradeSlotUI.transform);
        _itemInMeltingSlot = _generatorUpgradeSlotUI.transform.GetChild(0).GetComponent<InventoryItem>();
        Debug.Log(_itemInMeltingSlot.count - itemCount + " items used while away");
        _itemInMeltingSlot.count = itemCount;
        _itemInMeltingSlot.RefreshCount();
        _itemInMeltingSlot.GetComponent<RectTransform>().localPosition = Vector3.zero;
        _itemInMeltingSlot.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0,0,0);
        _itemInMeltingSlot.GetComponent<RectTransform>().localScale = Vector3.one * 0.8f;
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

    private void SaveGeneratorInventory()
    {
        try
        {
            _itemInMeltingSlot.transform.SetParent(this.gameObject.transform);
        }catch(NullReferenceException){}
    }

    public void DeactivateUIDisplay()
    {
        meltingTime = meltingBaseTime;
        _computerUIDisplay.maxPower -= _itemInMeltingSlot.item.atomicMass;
        _itemInMeltingSlot = _generatorUpgradeSlotUI.transform.GetChild(0).GetComponent<InventoryItem>();
        _computerUIDisplay.maxPower += _itemInMeltingSlot.item.atomicMass;
        itemCount = _itemInMeltingSlot.count;
        
        SavePlayerInventory();
        SaveGeneratorInventory();
        StopAllCoroutines();
        isUIUp = false;
        Destroy(_thisGeneratorUIDisplay);
    }
}
