using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MachineUIDisplay : MonoBehaviour
{
    public GameObject machineUIPrefab;
    
    private GameObject _thisMachineUIDisplay;
    private GameObject _machineInventoryUI;
    private GameObject _machineUpgradeSlotUI;
    private GameObject _machineCraftUI;
    private GameObject _machineRecipeUI;
    private Slider _machineProgressSliderUI;
    private GameObject _machineOutputSlotUI;
    private Slider _machineActivationSliderUI;
    private GameObject _machinePlayerInventoryUI;
    
    private float _machineRecipeUIxPosition;
    private float _machineRecipeUIyPosition;

    private GameObject _instantiatedMachineUIButton;
    private GameObject _instantiatedMachineUIImage;
    private GameObject _instantiatedMachineUIText;

    private Recipe _thisMachineCraftRecipe;
    private float _machineCraftingTimeLeft;

    public GameObject basicImage;
    public GameObject plusSign;
    public GameObject equalSign;
    public GameObject redImage;
    public GameObject basicText;
    
    private int _machineMaterialsReadyForCraft;
    public float machineCraftingTime;
    
    public List<GameObject> machineCraftingButtonList;
    private List<GameObject> _machineInventoryItemSlotsList;
    public InventoryItem thisMachineInventoryItem;
    public List<ItemClass> machineItemList;
    public List<int> machineItemAmountList;

    private List<Text> _recipeMaterialList;

    [FormerlySerializedAs("machineInventoryItemPrefab")] public GameObject inventoryItemPrefab;
    private InventoryItem _itemInSlot;
    private InventoryItem _itemCreated;

    public GameObject playerInventory;
    public float craftProgress;
    private int _thisMachineItemAmount;
    private int _machineItemInventorySlotOffset;

    //charge tous les endroit clés que le code utilise régulierement au sein de l'ui
    private void OnDisplayInstantiate()
    {
        _machineInventoryUI = _thisMachineUIDisplay.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        _machineUpgradeSlotUI = _thisMachineUIDisplay.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        _machineCraftUI = _thisMachineUIDisplay.transform.GetChild(3).GetChild(0).GetChild(0).GetChild(0).gameObject;
        _machineRecipeUI = _thisMachineUIDisplay.transform.GetChild(4).GetChild(0).GetChild(0).gameObject;
        _machineProgressSliderUI = _thisMachineUIDisplay.transform.GetChild(5).GetComponent<Slider>();
        _machineOutputSlotUI = _thisMachineUIDisplay.transform.GetChild(6).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        _machineActivationSliderUI = _thisMachineUIDisplay.transform.GetChild(7).GetComponent<Slider>();
        _machinePlayerInventoryUI = _thisMachineUIDisplay.transform.GetChild(8).GetChild(0).GetChild(0).GetChild(1).gameObject;

        var position = _machineRecipeUI.transform.position;
        _machineRecipeUIxPosition = position.x + 50;
        _machineRecipeUIyPosition = position.y + 90;
    }

    private void CachingItemSlots()
    {
        _machineInventoryItemSlotsList = new List<GameObject>();

        for(var i = 0; i < _machineInventoryUI.transform.childCount; i++)
        {
            _machineInventoryItemSlotsList.Add(_machineInventoryUI.transform.GetChild(i).gameObject);
        }
    }

    //fonction qui doit etre applé par un script externe affin d'activer l'UI de la machine
    public void ActivateUIDisplay()
    {
        _thisMachineUIDisplay = Instantiate(machineUIPrefab);

        OnDisplayInstantiate();
        CachingItemSlots();
        
        LoadInventory();
        LoadPlayerInventory();
        
        //cree des bouton dans le menu de craft celon les bouton selectionner dans l'inspecteur, puis leur ajoute leur fonctionnalité de craft
        for(var i = 0; i < machineCraftingButtonList.Count; i++)
        {
            var position = _machineCraftUI.transform.position;
            _instantiatedMachineUIButton = Instantiate(machineCraftingButtonList[i], new Vector3(position.x, position.y - i * 40, 0), Quaternion.identity, _machineCraftUI.transform);
            var a = i;
            _instantiatedMachineUIButton.GetComponent<Button>().onClick.AddListener(() => { SetRecipeOnClick(a); });
        }

        StartCoroutine(InventoryItemManager());
        StartCoroutine(ActivateMachine());
    }
/*
    private void Update()
    {
        if (_machineActivationSliderUI.value > 0.95f && _machineMaterialsReadyForCraft == _thisMachineCraftRecipe.inputs.Count)
        {
            
        }
    }*/

    //fonction qu il y a sur chacun des boutons affin d'afficher la nouvelle recette pour ce craft
    private void SetRecipeOnClick(int a)
    {
        //supprime l ancienne recette affiché
        for(var i = 0; i < _machineRecipeUI.transform.childCount; i++)
        {
            Destroy(_machineRecipeUI.transform.GetChild(i).gameObject);
        }

        _recipeMaterialList = new List<Text>();

        _thisMachineCraftRecipe = machineCraftingButtonList[a].GetComponent<ButtonCraft>().craft;

        //genere la nouvelle recette avec les images contenu dans les scriptable objects qui compose le craft tout en rajoutant des signes "+" et "="
        for(var y = 0; y < _thisMachineCraftRecipe.inputs.Count + _thisMachineCraftRecipe.outputs.Count; y++)
        {
            _instantiatedMachineUIImage = Instantiate(basicImage, new Vector3(_machineRecipeUIxPosition + y * 120, _machineRecipeUIyPosition, 0), Quaternion.identity, _machineRecipeUI.transform);
            _instantiatedMachineUIText = Instantiate(basicText, new Vector3(_machineRecipeUIxPosition + y * 120, _machineRecipeUIyPosition - 50, 0), Quaternion.identity, _machineRecipeUI.transform);
            _recipeMaterialList.Add(_instantiatedMachineUIText.GetComponent<Text>());

            if(y < _thisMachineCraftRecipe.inputs.Count)
            {
                _instantiatedMachineUIImage.GetComponent<Image>().sprite = _thisMachineCraftRecipe.inputs[y].sprite;
                Instantiate(redImage, _instantiatedMachineUIImage.transform.position, Quaternion.identity, _machineRecipeUI.transform.transform);
            }
            else
            {
                _instantiatedMachineUIImage.GetComponent<Image>().sprite = _thisMachineCraftRecipe.outputs[y - _thisMachineCraftRecipe.inputs.Count].sprite;
            }

            if(y != _thisMachineCraftRecipe.inputs.Count - 1 && y != _thisMachineCraftRecipe.inputs.Count + _thisMachineCraftRecipe.outputs.Count - 1)
            {
                Instantiate(plusSign, new Vector3(_machineRecipeUIxPosition + 60 + y * 120, _machineRecipeUIyPosition, 0), Quaternion.identity, _machineRecipeUI.transform);
            }

            if(y == _thisMachineCraftRecipe.inputs.Count - 1)
            {
                Instantiate(equalSign, new Vector3(_machineRecipeUIxPosition + 60 + y * 120, _machineRecipeUIyPosition, 0), Quaternion.identity, _machineRecipeUI.transform);
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
            _machineMaterialsReadyForCraft = 0;
            for(var i = 0; i < _recipeMaterialList.Count; i++)
            {
                //regarde si les matériaux que l'on est en train de définir si il sont requis ou reçus
                if(i < _thisMachineCraftRecipe.inputs.Count)
                {
                    if(machineItemList.Contains(_thisMachineCraftRecipe.inputs[i]))
                    {
                        //si l'inventaire de la machine contient ce materiaux alors il associe le nombre qu'il y en a dans l'inventaire avec l'ui 
                        _recipeMaterialList[i].text = machineItemAmountList[machineItemList.IndexOf(_thisMachineCraftRecipe.inputs[i])].ToString() + " /" + _thisMachineCraftRecipe.inputsAmount[i].ToString() + " " + _thisMachineCraftRecipe.inputs[i].nom;

                        //si il y a assez de ce materiaux pour le craft alors l'image rouge par dessus le materiaux disparait et le texte n'est plus rouge
                        if(machineItemAmountList[machineItemList.IndexOf(_thisMachineCraftRecipe.inputs[i])] >= _thisMachineCraftRecipe.inputsAmount[i])
                        {
                            _machineRecipeUI.transform.GetChild(i * 4 + 2).gameObject.SetActive(false);
                            _recipeMaterialList[i].color = new Color(0,0,0);
                            //ajoute 1 a ce int, si il est égal au nombre de matériaux requis on peut donc faire notre recette
                            _machineMaterialsReadyForCraft++;
                        }
                        else
                        {
                            _machineRecipeUI.transform.GetChild(i * 4 + 2).gameObject.SetActive(true);
                            _recipeMaterialList[i].color = new Color(1,0,0);
                        }
                    }
                    else
                    {
                        _recipeMaterialList[i].text = "0 /" + _thisMachineCraftRecipe.inputsAmount[i].ToString() + " " + _thisMachineCraftRecipe.inputs[i].nom;
                        _machineRecipeUI.transform.GetChild(i * 4 + 2).gameObject.SetActive(true);
                        _recipeMaterialList[i].color = new Color(1,0,0);
                    }
                }
                else
                {
                    //si le materiaux/objet est le resultat du craft et non requis alors on lui met juste le nombre que l'on en reçois sur le texte
                    _recipeMaterialList[i].text = _thisMachineCraftRecipe.outputsAmount[i - _thisMachineCraftRecipe.inputs.Count].ToString() + " " + _thisMachineCraftRecipe.outputs[i - _thisMachineCraftRecipe.inputs.Count].nom;  
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
            //machineInventoryItem = new List<InventoryItem>(new InventoryItem[_machineInventoryItemSlotsList.Count]);
            machineItemList = new List<ItemClass>();
            machineItemAmountList = new List<int>();

            //passe pour tous les slots d items
            for(var i = 0; i < _machineInventoryItemSlotsList.Count; i++)
            {
                if(_machineInventoryItemSlotsList[i].transform.childCount > 0)
                {
                    thisMachineInventoryItem = _machineInventoryItemSlotsList[i].transform.GetChild(0).GetComponent<InventoryItem>();
                    //si c'est le premier item de son type, il le rajoute et lui donne un montant nul
                    if(!machineItemList.Contains(thisMachineInventoryItem.item))
                    {
                        machineItemList.Add(thisMachineInventoryItem.item);
                        machineItemAmountList.Add(0);
                    }
                    
                    //ajoute le montant voulut
                    machineItemAmountList[machineItemList.IndexOf(thisMachineInventoryItem.item)] += thisMachineInventoryItem.count;
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
            if(_machineActivationSliderUI.value > 0.95f && _machineMaterialsReadyForCraft == _thisMachineCraftRecipe.inputs.Count)
            {
                _machineProgressSliderUI.value += 0.02f;
            }
            else
            {
                _machineProgressSliderUI.value = 0f;
                _machineActivationSliderUI.value -= 0.1f;
            }
            
            //quand le temps de craft est terminé: supprimer les ressource utiliser et generer la ressource voulut 
            if(_machineProgressSliderUI.value > 0.95f)
            {
                for(var i = 0; i < _thisMachineCraftRecipe.inputs.Count; i++)
                {
                    RemoveMaterialAmount(_thisMachineCraftRecipe.inputs[i], _thisMachineCraftRecipe.inputsAmount[i]);
                }

                for(var i = 0; i < _thisMachineCraftRecipe.outputs.Count; i++)
                {
                    AddMaterialAmount(_thisMachineCraftRecipe.outputs[i], _thisMachineCraftRecipe.outputsAmount[i], i);
                }

                _machineProgressSliderUI.value = 0f;
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    //trouve l'objet dans l inventaire que l'on veut enlever pour ensuite leur en enlever un montant
    private void RemoveMaterialAmount(ItemClass thisItem, int amount)
    {
        for(var i = 0; i < _machineInventoryItemSlotsList.Count; i++)
        {
            if(_machineInventoryItemSlotsList[i].transform.childCount > 0 && amount > 0)
            {
                thisMachineInventoryItem = _machineInventoryItemSlotsList[i].transform.GetChild(0).GetComponent<InventoryItem>();

                //ceci permet de voir si le montant de l'objet était plus grand ou moins que le montant que l'on veut enlever
                //si l'objet trouver a un moins grand nombre que ce que l'on veut enlever alors on supprme l objet et rafraichi le montant restant a enlever 
                if(thisMachineInventoryItem.item == thisItem)
                {
                    amount -= thisMachineInventoryItem.count;

                    thisMachineInventoryItem.count = -amount;

                    if(thisMachineInventoryItem.count <= 0)
                    {
                        Destroy(thisMachineInventoryItem.gameObject);
                    }
                    else
                    {
                        thisMachineInventoryItem.RefreshCount();
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
            if (_machineOutputSlotUI.transform.childCount > 0)
            {
                _itemInSlot = _machineOutputSlotUI.transform.GetChild(0).GetComponent<InventoryItem>();
                
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
            if (_machineOutputSlotUI.transform.childCount == 0)
            {
                _itemCreated = Instantiate(inventoryItemPrefab, _machineOutputSlotUI.transform).GetComponent<InventoryItem>();
                _itemCreated.InitialiseItem(thisItem);
                _itemCreated.count = amount;
                _itemCreated.RefreshCount();
            }
        }
        else 
        {
            //permet de voir tous les slots et si il y en a un qui contient déja l'objet que l'on veut rajouter, dans ce cas on modifie juste son nombre
            for(var i = 0; i < _machineInventoryItemSlotsList.Count; i++)
            {
                if(_machineInventoryItemSlotsList[i].transform.childCount > 0)
                {
                    thisMachineInventoryItem = _machineInventoryItemSlotsList[i].transform.GetChild(0).GetComponent<InventoryItem>();

                    if(thisMachineInventoryItem.item == thisItem && thisMachineInventoryItem.count < thisItem.stackSize)
                    {
                        thisMachineInventoryItem.count += amount;
                        thisMachineInventoryItem.RefreshCount();
                        amount = 0;
                    }
                }
            }
            //s'il n'y a pas deja l'objet obtenu dans l'inventaire alors on prend le premier slot libre et on le met dedans 
            if(amount > 0)
            {
                for(var i = 0; i < _machineInventoryItemSlotsList.Count; i++)
                {
                    if(_machineInventoryItemSlotsList[i].transform.childCount == 0 && amount > 0)
                    {
                        _itemCreated = Instantiate(inventoryItemPrefab, _machineInventoryItemSlotsList[i].transform).GetComponent<InventoryItem>();
                        _itemCreated.InitialiseItem(thisItem);
                        _itemCreated.count = amount;
                        _itemCreated.RefreshCount();
                        amount = 0;
                    }
                }
            }
        }
    }

    //charge l'inventaire de la machine grace à sa liste d'item de son inventaire qu'elle possède
    private void LoadInventory()
    {
        _machineItemInventorySlotOffset = 0;
        for (var i = 0; i < machineItemList.Count; i++)
        {
            _thisMachineItemAmount = machineItemAmountList[i];
            while(machineItemList[i].stackSize < _thisMachineItemAmount)
            {
                _itemCreated = Instantiate(inventoryItemPrefab, _machineInventoryItemSlotsList[_machineItemInventorySlotOffset].transform).GetComponent<InventoryItem>();
                _itemCreated.InitialiseItem(machineItemList[i]);
                _itemCreated.count = machineItemList[i].stackSize;
                _itemCreated.RefreshCount();
                _thisMachineItemAmount -= machineItemList[i].stackSize;
                _machineItemInventorySlotOffset++;
            }
            
            _itemCreated = Instantiate(inventoryItemPrefab, _machineInventoryItemSlotsList[_machineItemInventorySlotOffset].transform).GetComponent<InventoryItem>();
            _itemCreated.InitialiseItem(machineItemList[i]);
            _itemCreated.count = _thisMachineItemAmount;
            _itemCreated.RefreshCount();
            _machineItemInventorySlotOffset++;
        }
    }
    
    //charge le manager d'item de l'inventaire pour etre sur d'avoir le dernier compte de tous les items au sein de l'inventaire
    private void SaveInventory()
    {
        StartCoroutine(InventoryItemManager());
    }

    //prend tous les objets stocké dans l'inventaire du joueur et le met sur l'inventaire joueur qu il y a déja disposer sur la machine
    private void LoadPlayerInventory()
    {
        for(var i = 0; i < playerInventory.transform.childCount; i++)
        {
            Instantiate(playerInventory.transform.GetChild(i), _machinePlayerInventoryUI.transform);
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

            if(_machinePlayerInventoryUI.transform.GetChild(i).childCount > 0)
            {
                Instantiate(_machinePlayerInventoryUI.transform.GetChild(i).GetChild(0).gameObject, playerInventory.transform.GetChild(i));
            }
        }
    }

    public void DeactivateUIDisplay()
    {
        SaveInventory();
        SavePlayerInventory();
        StopAllCoroutines();
        Destroy(_thisMachineUIDisplay);
    }
}