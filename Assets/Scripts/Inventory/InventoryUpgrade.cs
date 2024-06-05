using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUpgrade : MonoBehaviour
{
    public List<ItemClass> upgradeItemList;
    public List<ItemClass> upgradeItemInInventory;
    
    private GameObject _inventoryUpgradeUI;
    private GameObject _thisInventory;
    
    private GameObject _upgradeSlot;
    private ItemClass _thisUpgradeItem;

    private GameObject _thisInventoryUpgradeUI;
    
    public void Awake()
    {
        _inventoryUpgradeUI = Resources.Load<GameObject>("MachineUI/InventoryUpgrade");
            
        _upgradeSlot = this.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        _thisInventory = this.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
    }

    void Update()
    {
        if(_upgradeSlot.transform.childCount > 0)
        {
            _thisUpgradeItem = _upgradeSlot.transform.GetChild(0).GetComponent<InventoryItem>().item;
            
            if(upgradeItemList.Contains(_thisUpgradeItem) && !upgradeItemInInventory.Contains(_thisUpgradeItem))
            {
                upgradeItemInInventory.Add(_thisUpgradeItem);

                _thisInventoryUpgradeUI = Instantiate(_inventoryUpgradeUI, _thisInventory.transform);
                _thisInventoryUpgradeUI.transform.GetChild(0).GetComponent<Image>().sprite = _thisUpgradeItem.sprite;
                _thisInventoryUpgradeUI.transform.GetChild(1).GetComponent<Text>().text = _thisUpgradeItem.description;
                
                DestroyImmediate(_upgradeSlot.transform.GetChild(0).gameObject);
            }  
        }
    }
}
