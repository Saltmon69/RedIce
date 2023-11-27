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
    private GameObject _instantiatedText;
    private Recipe _craft;
    public GameObject basicImage;
    public GameObject plusSign;
    public GameObject equalSign;
    public GameObject redImage;
    public GameObject basicText;

    private GameObject _savedInventory;
    public List<GameObject> craftingButtonList;
    private List<GameObject> _itemSlotsList;
    public List<InventoryItem> inventoryItemList;
    public List<ItemClass> itemList;
    public List<int> itemAmountList;

    private List<Text> _recipeMaterialList;

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

        //cree des bouton dans le menu de craft celon les bouton selectionner dans l'inspecteur, puis leur ajoute leur fonctionnalité de craft
        for(var i = 0; i < craftingButtonList.Count; i++)
        {
            _instantiatedButton = Instantiate(craftingButtonList[i], new Vector3(_crafting.transform.position.x, _crafting.transform.position.y - i * 40, 0), Quaternion.identity, _crafting.transform);
            var a = i;
            _instantiatedButton.GetComponent<Button>().onClick.AddListener(() => { SetRecipeOnClick(a); });
        }

        StartCoroutine(InventoryItemManager());
    }

    //fonction qu il y a sur chacun des boutons affin d'afficher la nouvelle recette pour ce craft
    public void SetRecipeOnClick(int a)
    {
        //supprime l ancienne recette affiché
        for(var i = 0; i < _recipe.transform.childCount; i++)
        {
            Destroy(_recipe.transform.GetChild(i).gameObject);
        }

        _recipeMaterialList = new List<Text>();

        _craft = craftingButtonList[a].GetComponent<ButtonCraft>().craft;

        //genere la nouvelle recette avec les images contenu dans les scriptable objects qui compose le craft tout en rajoutant des signes "+" et "="
        //ajout du texte de nombre de matériaux requis ainsi qui si et combien la machine en contient grâce l'inventory manager qui y calcule en permanence
        for(var y = 0; y < _craft.inputs.Count + _craft.outputs.Count; y++)
        {
            _instantiatedImage = Instantiate(basicImage, new Vector3(_recipeXPos + y * 120, _recipeYPos, 0), Quaternion.identity, _recipe.transform);
            _instantiatedText = Instantiate(basicText, new Vector3(_recipeXPos + y * 120, _recipeYPos - 50, 0), Quaternion.identity, _recipe.transform);
            _recipeMaterialList.Add(_instantiatedText.GetComponent<Text>());

            if(y < _craft.inputs.Count)
            {
                _instantiatedImage.GetComponent<Image>().sprite = _craft.inputs[y].sprite;
                Instantiate(redImage, _instantiatedImage.transform.position, Quaternion.identity, _recipe.transform.transform);
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

        StartCoroutine(RecipeMaterialManager());
    }

    public IEnumerator RecipeMaterialManager()
    {
        while(true)
        {
            for(var i = 0; i < _recipeMaterialList.Count; i++)
            {
                if(i < _craft.inputs.Count)
                {
                    if(itemList.Contains(_craft.inputs[i]))
                    {
                        _recipeMaterialList[i].text = itemAmountList[itemList.IndexOf(_craft.inputs[i])].ToString() + " /" + _craft.inputsAmount[i].ToString() + " " + _craft.inputs[i].nom;
                        _recipe.transform.GetChild(i * 4 + 2).gameObject.SetActive(true);
                        _recipeMaterialList[i].color = new Color(1,0,0);

                        if(itemAmountList[itemList.IndexOf(_craft.inputs[i])] >= _craft.inputsAmount[i])
                        {
                            _recipe.transform.GetChild(i * 4 + 2).gameObject.SetActive(false);
                            _recipeMaterialList[i].color = new Color(0,0,0);
                        }
                    }
                    else
                    {
                        _recipeMaterialList[i].text = "0 /" + _craft.inputsAmount[i].ToString() + " " + _craft.inputs[i].nom;
                        _recipe.transform.GetChild(i * 4 + 2).gameObject.SetActive(true);
                        _recipeMaterialList[i].color = new Color(1,0,0);
                    }
                }
                else
                {
                    _recipeMaterialList[i].text = _craft.outputsAmount[i - _craft.inputs.Count].ToString() + " " + _craft.outputs[i - _craft.inputs.Count].nom;  
                }
            }
            yield return new WaitForSeconds(0.2f);
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
    private IEnumerator InventoryItemManager()
    {  
        while(true)
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
            yield return new WaitForSeconds(0.2f);
        }
    }
}