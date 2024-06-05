using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class BaseRepair : MonoBehaviour
{
    private GameObject _playerInventory;
    private MachineCost _machineCost;
    public List<ItemClass> playerItemList;
    public List<int> playerItemAmountList;
    private InventoryItem _thisInventoryItem;
    private GameObject _itemSprite;
    private GameObject _thisItemSprite;
    public float altitude;
    public List<GameObject> itemSpriteList;
    public bool isBuildable;
    public int materialsReady;
    public bool isBuilt;

    private void Awake()
    {
        _playerInventory = GameObject.FindWithTag("Player").GetComponent<PlayerMenuing>().inventory.transform.GetChild(0).GetChild(1).gameObject;
        _machineCost = this.gameObject.GetComponent<MachineCost>();
        _itemSprite = Resources.Load<GameObject>("MachineUI/ItemSprite");
        Debug.Log(_itemSprite);
    }
    
    void Start()
    {
        for (var i = 0; i < _machineCost.buildingMaterialList.Count; i++)
        {
            _thisItemSprite = Instantiate(_itemSprite, this.gameObject.transform.parent);
            _thisItemSprite.transform.localPosition = new Vector3((i + 0.5f - _machineCost.buildingMaterialList.Count / 2) * 10, altitude, 0);
            _thisItemSprite.transform.localRotation = Quaternion.Euler(0,0,0);
                
            _thisItemSprite.GetComponent<SpriteRenderer>().sprite = _machineCost.buildingMaterialList[i].sprite;
            itemSpriteList.Add(_thisItemSprite);
            _thisItemSprite.transform.GetChild(0).GetComponent<TextMesh>().text = _machineCost.buildingMaterialAmountList[i] + " manquant";
        }

        StartCoroutine(PlayerInventoryItem());
    }

    private IEnumerator PlayerInventoryItem()
    {
        while(true)
        {
            materialsReady = 0;
            playerItemList = new List<ItemClass>();
            playerItemAmountList = new List<int>();

            for(var i = 0; i < _playerInventory.transform.childCount; i++)
            {
                if(_playerInventory.transform.GetChild(i).childCount == 0) continue;
                _thisInventoryItem = _playerInventory.transform.GetChild(i).GetChild(0).GetComponent<InventoryItem>();
            
                if(!playerItemList.Contains(_thisInventoryItem.item))
                {
                    playerItemList.Add(_playerInventory.transform.GetChild(i).GetChild(0).GetComponent<InventoryItem>().item);
                    playerItemAmountList.Add(_thisInventoryItem.count);
                }
                else
                {
                    playerItemAmountList[playerItemList.IndexOf(_thisInventoryItem.item)] += _thisInventoryItem.count;
                }
            }

            for(var i = 0; i < playerItemList.Count; i++)
            {
                if(!_machineCost.buildingMaterialList.Contains(playerItemList[i])) continue;
                if(_machineCost.buildingMaterialAmountList[_machineCost.buildingMaterialList.IndexOf(playerItemList[i])] - playerItemAmountList[i] <= 0)
                {
                    itemSpriteList[_machineCost.buildingMaterialList.IndexOf(playerItemList[i])].transform.GetChild(0).GetComponent<TextMesh>().text = "Prêt";
                    materialsReady++;
                    continue;
                }
                
                itemSpriteList[_machineCost.buildingMaterialList.IndexOf(playerItemList[i])].transform.GetChild(0).GetComponent<TextMesh>().text = _machineCost.buildingMaterialAmountList[i] - playerItemAmountList[i] + " manquant";
            }

            isBuildable = materialsReady == _machineCost.buildingMaterialList.Count;
            
            yield return new WaitForSeconds(1);
        }
    }
}
