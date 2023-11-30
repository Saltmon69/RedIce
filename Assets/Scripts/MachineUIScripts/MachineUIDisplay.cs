using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MachineUIDisplay : MonoBehaviour
{
    public GameObject machineUIPrefab;
    private GameObject _thisMachineUIDisplay;
    private GameObject _inventory;
    private GameObject _upgradeSlot;
    private GameObject _crafting;
    private GameObject _recipe;
    private Slider _progressBar;
    private GameObject _outputSlot;
    private Slider _machineActivationSlider;
    private GameObject _machinePlayerInventory;
    private float _recipeXPos;
    private float _recipeYPos;

    private GameObject _instantiatedButton;
    private GameObject _instantiatedImage;
    private GameObject _instantiatedText;

    private Recipe _craft;
    private float _craftingTimeLeft;

    public GameObject basicImage;
    public GameObject plusSign;
    public GameObject equalSign;
    public GameObject redImage;
    public GameObject basicText;
    private int _numberOfMaterialsReadyForCraft;
    public float baseCraftingTime;


    private GameObject _savedInventory;
    public List<GameObject> craftingButtonList;
    private List<GameObject> _itemSlotsList;
    public List<InventoryItem> inventoryItemList;
    public List<ItemClass> itemList;
    public List<int> itemAmountList;

    private List<Text> _recipeMaterialList;

    public GameObject inventoryItemPrefab;
    private InventoryItem _itemInSlot;
    private InventoryItem _itemCreated;

    public GameObject playerInventory;
    public float craftProgress;

    //charge tous les endroit clés que le code utilise régulierement au sein de l'ui
    private void OnDisplayInstantiate()
    {
        _inventory = _thisMachineUIDisplay.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        _upgradeSlot = _thisMachineUIDisplay.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        _crafting = _thisMachineUIDisplay.transform.GetChild(3).GetChild(0).GetChild(0).GetChild(0).gameObject;
        _recipe = _thisMachineUIDisplay.transform.GetChild(4).GetChild(0).GetChild(0).gameObject;
        _progressBar = _thisMachineUIDisplay.transform.GetChild(5).GetComponent<Slider>();
        _outputSlot = _thisMachineUIDisplay.transform.GetChild(6).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        _machineActivationSlider = _thisMachineUIDisplay.transform.GetChild(7).GetComponent<Slider>();
        _machinePlayerInventory = _thisMachineUIDisplay.transform.GetChild(8).GetChild(0).GetChild(0).GetChild(1).gameObject;

        var position = _recipe.transform.position;
        _recipeXPos = position.x + 50;
        _recipeYPos = position.y + 90;
    }

    private void CachingItemSlots()
    {
        _itemSlotsList = new List<GameObject>();

        for(var i = 0; i < _inventory.transform.childCount; i++)
        {
            _itemSlotsList.Add(_inventory.transform.GetChild(i).gameObject);
        }
    }

    //fonction qui doit etre applé par un script externe affin d'activer l'UI de la machine
    public void ActivateUIDisplay()
    {
        _thisMachineUIDisplay = Instantiate(machineUIPrefab);

        OnDisplayInstantiate();

        if(_savedInventory != null)
        {
            LoadInventory();
        }
        LoadPlayerInventory();

        CachingItemSlots();

        //cree des bouton dans le menu de craft celon les bouton selectionner dans l'inspecteur, puis leur ajoute leur fonctionnalité de craft
        for(var i = 0; i < craftingButtonList.Count; i++)
        {
            var position = _crafting.transform.position;
            _instantiatedButton = Instantiate(craftingButtonList[i], new Vector3(position.x, position.y - i * 40, 0), Quaternion.identity, _crafting.transform);
            var a = i;
            _instantiatedButton.GetComponent<Button>().onClick.AddListener(() => { SetRecipeOnClick(a); });
        }

        StartCoroutine(InventoryItemManager());
        StartCoroutine(ActivateMachine());
    }
/*
    private void Update()
    {
        if (_machineActivationSlider.value > 0.95f && _numberOfMaterialsReadyForCraft == _craft.inputs.Count)
        {
            
        }
    }*/

    //fonction qu il y a sur chacun des boutons affin d'afficher la nouvelle recette pour ce craft
    private void SetRecipeOnClick(int a)
    {
        //supprime l ancienne recette affiché
        for(var i = 0; i < _recipe.transform.childCount; i++)
        {
            Destroy(_recipe.transform.GetChild(i).gameObject);
        }

        _recipeMaterialList = new List<Text>();

        _craft = craftingButtonList[a].GetComponent<ButtonCraft>().craft;

        //genere la nouvelle recette avec les images contenu dans les scriptable objects qui compose le craft tout en rajoutant des signes "+" et "="
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

    //rafraichi l'UI de la recette pour qu elle correspond et soit directement lié avec le nombre de matériaux dans l'inventaire de la machine
    //gere sur chaque materiaux requis un feedback d'une image rouge par dessus le materiaux et d'un texte rouge si l'on a pas asser de se materiaux
    //cela sert aussi a directement confirmer si le craft est pret et peut etre fait
    private IEnumerator RecipeMaterialManager()
    {
        while(true)
        {
            _numberOfMaterialsReadyForCraft = 0;
            for(var i = 0; i < _recipeMaterialList.Count; i++)
            {
                //regarde si les matériaux que l'on est en train de définir si il sont requis ou reçus
                if(i < _craft.inputs.Count)
                {
                    if(itemList.Contains(_craft.inputs[i]))
                    {
                        //si l'inventaire de la machine contient ce materiaux alors il associe le nombre qu'il y en a dans l'inventaire avec l'ui 
                        _recipeMaterialList[i].text = itemAmountList[itemList.IndexOf(_craft.inputs[i])].ToString() + " /" + _craft.inputsAmount[i].ToString() + " " + _craft.inputs[i].nom;

                        //si il y a assez de ce materiaux pour le craft alors l'image rouge par dessus le materiaux disparait et le texte n'est plus rouge
                        if(itemAmountList[itemList.IndexOf(_craft.inputs[i])] >= _craft.inputsAmount[i])
                        {
                            _recipe.transform.GetChild(i * 4 + 2).gameObject.SetActive(false);
                            _recipeMaterialList[i].color = new Color(0,0,0);
                            //ajoute 1 a ce int, si il est égal au nombre de matériaux requis on peut donc faire notre recette
                            _numberOfMaterialsReadyForCraft++;
                        }
                        else
                        {
                            _recipe.transform.GetChild(i * 4 + 2).gameObject.SetActive(true);
                            _recipeMaterialList[i].color = new Color(1,0,0);
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
                    //si le materiaux/objet est le resultat du craft et non requis alors on lui met juste le nombre que l'on en reçois sur le texte
                    _recipeMaterialList[i].text = _craft.outputsAmount[i - _craft.inputs.Count].ToString() + " " + _craft.outputs[i - _craft.inputs.Count].nom;  
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
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
                    for(var j = 0; j < itemList.Count; j++) // changer en indexof
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

    //regarde si l'on a activer la machine avec le slider et permet de gerer le temps de craft ainsi que le resultat
    private IEnumerator ActivateMachine()
    {
        while(true)
        {
            if(_machineActivationSlider.value > 0.95f && _numberOfMaterialsReadyForCraft == _craft.inputs.Count)
            {
                _progressBar.value += 0.02f;
            }
            else
            {
                _progressBar.value = 0f;
                _machineActivationSlider.value -= 0.1f;
            }
            
            //quand le temps de craft est terminé: supprimer les ressource utiliser et generer la ressource voulut 
            if(_progressBar.value > 0.95f)
            {
                for(var i = 0; i < _craft.inputs.Count; i++)
                {
                    RemoveMaterialAmount(_craft.inputs[i], _craft.inputsAmount[i]);
                }

                for(var i = 0; i < _craft.outputs.Count; i++)
                {
                    AddMaterialAmount(_craft.outputs[i], _craft.outputsAmount[i], i);
                }

                _progressBar.value = 0f;
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    //trouve l'objet dans l inventaire que l'on veut enlever pour ensuite leur en enlever un montant
    private void RemoveMaterialAmount(ItemClass thisItem, int amount)
    {
        for(var i = 0; i < _itemSlotsList.Count; i++)
        {
            if(_itemSlotsList[i].transform.childCount > 0 && amount > 0)
            {
                inventoryItemList[i] = _itemSlotsList[i].transform.GetChild(0).GetComponent<InventoryItem>();

                //ceci permet de voir si le montant de l'objet était plus grand ou moins que le montant que l'on veut enlever
                //si l'objet trouver a un moins grand nombre que ce que l'on veut enlever alors on supprme l objet et rafraichi le montant restant a enlever 
                if(inventoryItemList[i].item == thisItem)
                {
                    amount = amount - inventoryItemList[i].count;

                    inventoryItemList[i].count = -amount;

                    if(inventoryItemList[i].count <= 0)
                    {
                        Destroy(inventoryItemList[i].gameObject);
                    }
                    else
                    {
                        inventoryItemList[i].RefreshCount();
                    }
                }
            }
        }
    }

    //permet d'ajouter un materiaux dans le slot de sortie. cependant si l'output est déja plein ou que l'on recois plusieurs objet, le reste va dans l'inventaire  
    private void AddMaterialAmount(ItemClass thisItem, int amount, int j)
    {
        if(j == 0) 
        {   
            //ajout du materiaux dans le slot de sortie, cependant si il y a deja un materiaux different il va le mettre dans l'inventaire
            if (_outputSlot.transform.childCount > 0)
            {
                _itemInSlot = _outputSlot.transform.GetChild(0).GetComponent<InventoryItem>();
                
                if(_itemInSlot.item == thisItem && _itemInSlot.count < thisItem.stackSize)
                {
                    _itemInSlot.count += amount;
                    _itemInSlot.RefreshCount();
                }
                else
                {
                    AddMaterialAmount(thisItem, amount, 1);
                }
            }

            //si le slot est libre alors on peut tout simplement l'ajouter dedans
            if (_outputSlot.transform.childCount == 0)
            {
                _itemCreated = Instantiate(inventoryItemPrefab, _outputSlot.transform).GetComponent<InventoryItem>();
                _itemCreated.InitialiseItem(thisItem);
                _itemCreated.count = amount;
                _itemCreated.RefreshCount();
            }
        }
        else 
        {
            //permet de voir tous les slots et si il y en a un qui contient déja l'objet que l'on veut rajouter, dans ce cas on modifie juste son nombre
            for(var i = 0; i < _itemSlotsList.Count; i++)
            {
                if(_itemSlotsList[i].transform.childCount > 0)
                {
                    inventoryItemList[i] = _itemSlotsList[i].transform.GetChild(0).GetComponent<InventoryItem>();

                    if(inventoryItemList[i].item == thisItem && inventoryItemList[i].count < thisItem.stackSize)
                    {
                        inventoryItemList[i].count += amount;
                        inventoryItemList[i].RefreshCount();
                        amount = 0;
                    }
                }
            }
            //s'il n'y a pas deja l'objet obtenu dans l'inventaire alors on prend le premier slot libre et on le met dedans 
            if(amount > 0)
            {
                for(var i = 0; i < _itemSlotsList.Count; i++)
                {
                    if(_itemSlotsList[i].transform.childCount == 0 && amount > 0)
                    {
                        _itemCreated = Instantiate(inventoryItemPrefab, _itemSlotsList[i].transform).GetComponent<InventoryItem>();
                        _itemCreated.InitialiseItem(thisItem);
                        _itemCreated.count = amount;
                        _itemCreated.RefreshCount();
                        amount = 0;
                    }
                }
            }
        }
    }

    //place l'inventaire de la machine sur la machine et supprime celui obsolete aui est déja présent sur l'ui
    private void LoadInventory()
    {
        _savedInventory.transform.SetParent(_inventory.transform.parent);
        _savedInventory.transform.position = _inventory.transform.position;
        Destroy(_inventory);
        _inventory = _savedInventory;
    }
    
    //met l'inventaire sur la machine pour qu'il ne sois pas detruit avec le reste de l'ui affin de pouvoir le récupérer plus tard
    private void SaveInventory()
    {
        _inventory.transform.SetParent(this.transform);
        _savedInventory = _inventory;
    }

    //prend tous les objets stocké dans l'inventaire du joueur et le met sur l'inventaire joueur qu il y a déja disposer sur la machine
    private void LoadPlayerInventory()
    {
        for(var i = 0; i < playerInventory.transform.childCount; i++)
        {
            Instantiate(playerInventory.transform.GetChild(i), _machinePlayerInventory.transform);
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

            if(_machinePlayerInventory.transform.GetChild(i).childCount > 0)
            {
                Instantiate(_machinePlayerInventory.transform.GetChild(i).GetChild(0).gameObject, playerInventory.transform.GetChild(i));
            }
        }
    }

    public void DeactivateUIDisplay()
    {
        SaveInventory();
        SavePlayerInventory();
        Destroy(_thisMachineUIDisplay);
    }
}