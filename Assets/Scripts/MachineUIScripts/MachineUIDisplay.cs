using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class MachineUIDisplay : MonoBehaviour
{
    public GameObject machineUIPrefab;
    private GameObject _thisMachineUIDisplay;
    private GameObject _inventory;
    private GameObject _upgradeSlot;
    private GameObject _crafting;
    private GameObject _recipe;
    private float _recipeXPos;
    private float _recipeYPos;
    private GameObject _progressBar;
    private GameObject _outputSlot;
    private GameObject _instantiatedButton;
    private GameObject _instantiatedImage;
    private Recipe _craft;
    public GameObject basicImage;
    public GameObject plusSign;
    public GameObject equalSign;

    private GameObject _savedInventory;
    public List<GameObject> craftingButtonList;
    private List<GameObject> _itemSlotsList;
    public List<InventoryItem> inventoryItemList;
    public List<ItemClass> itemList;
    public List<int> itemAmountList;

    //charge tous les endroit clés que le code utilise régulierement
    public void OnDisplayInstantiate()
    {
        _inventory = _thisMachineUIDisplay.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        _upgradeSlot = _thisMachineUIDisplay.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        _crafting = _thisMachineUIDisplay.transform.GetChild(3).GetChild(0).GetChild(0).GetChild(0).gameObject;
        _recipe = _thisMachineUIDisplay.transform.GetChild(4).GetChild(0).GetChild(0).gameObject;
        _progressBar = _thisMachineUIDisplay.transform.GetChild(5).gameObject;
        _outputSlot = _thisMachineUIDisplay.transform.GetChild(6).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        
        _recipeXPos = _recipe.transform.position.x + 50;
        _recipeYPos = _recipe.transform.position.y + 90;
    }

    public void CachingItemSlots()
    {
        _itemSlotsList = new List<GameObject>();

        for(var i = 0; i < _inventory.transform.childCount; i++)
        {
            _itemSlotsList.Add(_inventory.transform.GetChild(i).gameObject);
        }
    }

    public void ActivateUIDisplay()
    {
        _thisMachineUIDisplay = Instantiate(machineUIPrefab);

        OnDisplayInstantiate();

        if(_savedInventory != null)
        {
            LoadInventory();
        }

        CachingItemSlots();

        InventoryItemManager();

        //cree des bouton dans le menu de craft celon les bouton selectionner dans l'inspecteur, puis leur ajoute leur fonctionnalité de craft
        for(var i = 0; i < craftingButtonList.Count; i++)
        {
            _instantiatedButton = Instantiate(craftingButtonList[i], new Vector3(_crafting.transform.position.x, _crafting.transform.position.y - i * 40, 0), Quaternion.identity, _crafting.transform);
            var a = i;
            _instantiatedButton.GetComponent<Button>().onClick.AddListener(() => { SetRecipeOnClick(a); });
        }
    }

    //fonction qu il y a sur chacun des boutons affin d'afficher la nouvelle recette pour ce craft
    public void SetRecipeOnClick(int a)
    {
        //supprime l ancienne recette afficher
        for(var i = 0; i < _recipe.transform.childCount; i++)
        {
            Destroy(_recipe.transform.GetChild(i).gameObject);
        }

        _craft = craftingButtonList[a].GetComponent<ButtonCraft>().craft;

        //génere la nouvelle recette avec les images contenu dans les scriptable objects qui compose le craft tout en rajoutant des signes "+" et "="
        for(var y = 0; y < _craft.inputs.Count + _craft.outputs.Count; y++)
        {
            _instantiatedImage = Instantiate(basicImage, new Vector3(_recipeXPos + y * 120, _recipeYPos, 0), Quaternion.identity, _recipe.transform);
            
            if(y < _craft.inputs.Count)
            {
                _instantiatedImage.GetComponent<Image>().sprite = _craft.inputs[y].sprite;
            }
            else
            {
                _instantiatedImage.GetComponent<Image>().sprite = _craft.outputs[y - _craft.inputs.Count].sprite;
            }

            if(y != _craft.inputs.Count - 1 && y != _craft.inputs.Count + _craft.outputs.Count - 1)
            {
                Instantiate(plusSign, new Vector3(_recipeXPos + 60 + y * 120, _recipeYPos, 0), Quaternion.identity, _recipe.transform);
            }

            if(y == _craft.inputs.Count - 1)
            {
                Instantiate(equalSign, new Vector3(_recipeXPos + 60 + y * 120, _recipeYPos, 0), Quaternion.identity, _recipe.transform);
            }
        }
    }

    public void DeactivateUIDisplay()
    {
        SaveInventory();
        Destroy(_thisMachineUIDisplay);
    }

    //put the inventory on the machine
    public void SaveInventory()
    {
        _inventory.transform.SetParent(this.transform);
        _savedInventory = _inventory;
    }

    //place the inventory on the machine onto the ui display and delete the obsolete one
    public void LoadInventory()
    {
        _savedInventory.transform.SetParent(_inventory.transform.parent);
        _savedInventory.transform.position = _inventory.transform.position;
        Destroy(_inventory);
        _inventory = _savedInventory;
    }

    //repere tous les items dans l inventaire et les quantifies
    public void InventoryItemManager()
    {
        inventoryItemList = new List<InventoryItem>(new InventoryItem[_itemSlotsList.Count]);
        itemList = new List<ItemClass>();
        itemAmountList = new List<int>();

        //passe pour tous les slots d items
        for(var i = 0; i < _itemSlotsList.Count; i++)
        {
            if(_itemSlotsList[i].transform.childCount > 0)
            {
                inventoryItemList[i] = _itemSlotsList[i].transform.GetChild(0).GetComponent<InventoryItem>();

                //si c'est le premier item de son type, il le rajoute et lui donne un montant null, cela sert car on a besoin d'étendre la capacité de la liste des montants
                if(!itemList.Contains(inventoryItemList[i].item))
                {
                    itemList.Add(inventoryItemList[i].item);
                    itemAmountList.Add(0);
                }
                //passe par tous les items trouver jusque la pour voir dans quel position se trouve l'item dans le slot actuel affin de lui attribuer son montant
                for(var j = 0; j < itemList.Count; j++)
                {
                    if(itemList[j] == inventoryItemList[i].item)
                    {
                        itemAmountList[j] += inventoryItemList[i].count;
                    }
                }
            }
        }
    }
}
