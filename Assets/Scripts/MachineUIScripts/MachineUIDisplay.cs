using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineUIDisplay : MonoBehaviour
{
    public GameObject machineUIPrefab;
    public GameObject machineInventoryTransferUI;
    
    private GameObject _thisMachineUIDisplay;
    private GameObject _machineInventoryUI;
    private GameObject _machineInventoryDropSlotUI;
    private GameObject _machineUpgradeSlotUI;
    private GameObject _machineCraftUI;
    private GameObject _machineRecipeUI;
    private Slider _machineProgressSliderUI;
    private GameObject _machineOutputMaterialsUI;
    private Slider _machineActivationSliderUI;
    private GameObject _machinePlayerInventoryUI;

    private GameObject _instantiatedMachineUIButton;
    private GameObject _instantiatedMachineUIRecipeMaterial;

    private Recipe _machineCraftRecipe;
    private float _machineCraftingTimeLeft;

    public GameObject materialRecipePrefab;
    public GameObject plusSign;
    public GameObject equalSign;

    public int machineMaterialsReadyForCraft;
    public float machineCraftingTime;
    public float craftProgress;
    
    public List<GameObject> machineCraftingButtonList;
    public InventoryItem thisMachineInventoryItem;
    public List<ItemClass> machineItemList;
    public List<int> machineItemAmountList;

    private List<Text> _recipeMaterialList;
    private int _usedRecipeIndex;
    
    public GameObject inventoryItemPrefab;
    private InventoryItem _itemInSlot;
    private InventoryItem _itemCreated;

    public GameObject playerInventory;
    private int _thisMachineItemAmount;
    private int _machineItemInventorySlotOffset;
    public bool isMachineActivated;
    private bool _isMachineForcedToDeactivate;

    public GameObject inventoryItemButtonUIPrefab;
    private GameObject _thisMachineItemButtonUI;
    private InventoryItem _thisMachineOutputMaterial;
    private CanvasGroup _machineOutputMaterialGroup;

    private int _thisIndex;
    private List<Text> _machineInventoryAmountTextList;
    public bool isUIOpen;

    private GameObject _thisTransferAmountUI;
    private Slider _transferAmountUISlider;
    private Text _transferAmountUIText;
    private int _transferAmount;
    private List<Transform> _machinePlayerInventoryList;

    private GameObject _thisMachinePlayerInventorySlot;
    
    public List<GameObject> thisMachineOutputList;
    public List<GameObject> thisMachineOutputCableList;
    public List<GameObject> thisMachineInputList;
    public List<GameObject> thisMachineInputCableList;
    public List<GameObject> thisMachineCableMachineInputList;

    //charge tous les endroit clés que le code utilise régulierement au sein de l'ui
    private void OnDisplayInstantiate()
    {
        _machineInventoryUI = _thisMachineUIDisplay.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).gameObject;
        _machineInventoryDropSlotUI = _thisMachineUIDisplay.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject;
        _machineUpgradeSlotUI = _thisMachineUIDisplay.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        _machineCraftUI = _thisMachineUIDisplay.transform.GetChild(3).GetChild(0).GetChild(0).GetChild(1).gameObject;
        _machineRecipeUI = _thisMachineUIDisplay.transform.GetChild(4).GetChild(1).GetChild(0).gameObject;
        _machineProgressSliderUI = _thisMachineUIDisplay.transform.GetChild(5).GetComponent<Slider>();
        _machineOutputMaterialsUI = _thisMachineUIDisplay.transform.GetChild(6).GetChild(0).gameObject;
        _machineActivationSliderUI = _thisMachineUIDisplay.transform.GetChild(7).GetComponent<Slider>();
        _machinePlayerInventoryUI = _thisMachineUIDisplay.transform.GetChild(8).GetChild(0).GetChild(0).GetChild(1).gameObject;
    }

    //fonction qui doit etre appelé par un script externe affin d'activer l'UI de la machine
    public void ActivateUIDisplay()
    {
        isUIOpen = true;
        _thisMachineUIDisplay = Instantiate(machineUIPrefab);

        OnDisplayInstantiate();

        LoadInventory();
        LoadPlayerInventory();

        StartCoroutine(InventoryItemManager());
        StartCoroutine(ActivateMachine());
        
        //cree des bouton dans le menu de craft celon les bouton selectionner dans l'inspecteur, puis leur ajoute leur fonctionnalité de craft
        for(var i = 0; i < machineCraftingButtonList.Count; i++)
        {
            _instantiatedMachineUIButton = Instantiate(machineCraftingButtonList[i], _machineCraftUI.transform);
            var a = i;
            _instantiatedMachineUIButton.GetComponent<Button>().onClick.AddListener(() => { SetRecipeOnClick(a); });
        }
        SetRecipeOnClick(_usedRecipeIndex);
    }

    public void Update()
    {
        if(isMachineActivated)
        {
            craftProgress += Time.deltaTime; 
        }
        else if(craftProgress > 0)
        {
            craftProgress -= Time.deltaTime * 5; 
        }
        
        //quand le craft est terminer alors on supprime le nombre de materiaux utiliser et on reçois le nombre d'objet ou matériaux crafté
        //le nombre de matériaux crafter on un feedback grâce à la coroutine "OutputMaterialFadeOut"
        if(craftProgress >= machineCraftingTime && isMachineActivated)
        {
            machineMaterialsReadyForCraft = 0;
            
            for (var i = 0; i < _recipeMaterialList.Count; i++)
            {
                //regarde les ressources prête que le joueur utilise pour construire son objet/matériaux 
                if (i < _machineCraftRecipe.inputs.Count && machineItemList.Contains(_machineCraftRecipe.inputs[i]) && 
                    machineItemAmountList[machineItemList.IndexOf(_machineCraftRecipe.inputs[i])] >= _machineCraftRecipe.inputsAmount[i])
                {
                    machineMaterialsReadyForCraft++;
                }
            }

            //si il y a toutes les ressources de prete pour cree l'objet/matériaux alors on supprime ce que la recette demande et on reçois le resultat
            if (machineMaterialsReadyForCraft == _machineCraftRecipe.inputs.Count)
            {
                //supprime les ressources requises pour la création du resultat de la recette
                for(var i = 0; i < _machineCraftRecipe.inputs.Count; i++)
                {
                    RemoveItemFromInventory(_machineCraftRecipe.inputs[i], _machineCraftRecipe.inputsAmount[i]);
                }
            
                //détruit ce qu'il y avait aupart avant dans l'output affin de pouvoir faire le feedback visuel correctement plus tard
                if (isUIOpen)
                {
                    for(var i = _machineOutputMaterialsUI.transform.childCount - 1; i >= 0; i--)
                    {
                        Destroy(_machineOutputMaterialsUI.transform.GetChild(i).gameObject);
                    }
                }
                
                //ajoute le resultat dans l'inventaire
                for(var i = 0; i < _machineCraftRecipe.outputs.Count; i++)
                {
                    AddItemToInventory(_machineCraftRecipe.outputs[i], _machineCraftRecipe.outputsAmount[i], false);

                    if (!isUIOpen) continue;
                    _thisMachineOutputMaterial = Instantiate(inventoryItemPrefab, _machineOutputMaterialsUI.transform).GetComponent<InventoryItem>();
                    _thisMachineOutputMaterial.InitialiseItem(_machineCraftRecipe.outputs[i]);
                    _thisMachineOutputMaterial.count = 0;
                    _thisMachineOutputMaterial.RefreshCount();
                }
                
                //fait un feedback visuel de ou des ressource que l'on a obtenu avec le resultat de la recette
                if (isUIOpen)
                {
                    StartCoroutine(OutputMaterialFadeOut(0.5f));
                }
                
                craftProgress = 0;
            }
            else
            {
                isMachineActivated = false;
                _isMachineForcedToDeactivate = true;
            }
        }
    }

    //fonction qu il y a sur chacun des boutons affin d'afficher la nouvelle recette pour ce craft
    private void SetRecipeOnClick(int a)
    {
        _usedRecipeIndex = a;
        //supprime l ancienne recette affiché
        for(var i = 0; i < _machineRecipeUI.transform.childCount; i++)
        {
            Destroy(_machineRecipeUI.transform.GetChild(i).gameObject);
        }

        _recipeMaterialList = new List<Text>();

        _machineCraftRecipe = machineCraftingButtonList[a].GetComponent<ButtonCraft>().craft;

        //genere la nouvelle recette avec les images contenu dans les scriptable objects qui compose le craft tout en rajoutant des signes "+" et "="
        //il assigne aussi les textes qui gere le montant des materiaux de la recette qui sera défini dans "RecipeMaterialManager"
        for(var y = 0; y < _machineCraftRecipe.inputs.Count + _machineCraftRecipe.outputs.Count; y++)
        {
            _instantiatedMachineUIRecipeMaterial = Instantiate(materialRecipePrefab, _machineRecipeUI.transform);
            _recipeMaterialList.Add(_instantiatedMachineUIRecipeMaterial.transform.GetChild(2).GetComponent<Text>());

            if(y < _machineCraftRecipe.inputs.Count)
            {
                _instantiatedMachineUIRecipeMaterial.transform.GetChild(0).GetComponent<Image>().sprite = _machineCraftRecipe.inputs[y].sprite;
            }
            else
            {
                _instantiatedMachineUIRecipeMaterial.transform.GetChild(0).GetComponent<Image>().sprite = _machineCraftRecipe.outputs[y - _machineCraftRecipe.inputs.Count].sprite;
            }

            if(y != _machineCraftRecipe.inputs.Count - 1 && y != _machineCraftRecipe.inputs.Count + _machineCraftRecipe.outputs.Count - 1)
            {
                Instantiate(plusSign, _machineRecipeUI.transform);
            }

            if(y == _machineCraftRecipe.inputs.Count - 1)
            {
                Instantiate(equalSign, _machineRecipeUI.transform);
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
            machineMaterialsReadyForCraft = 0;
            for(var i = 0; i < _recipeMaterialList.Count; i++)
            {
                //regarde si les matériaux que l'on est en train de définir si il sont requis ou reçus
                if(i < _machineCraftRecipe.inputs.Count)
                {
                    if(machineItemList.Contains(_machineCraftRecipe.inputs[i]))
                    {
                        //si l'inventaire de la machine contient ce materiaux alors il associe le nombre qu'il y en a dans l'inventaire avec l'ui 
                        _recipeMaterialList[i].text = machineItemAmountList[machineItemList.IndexOf(_machineCraftRecipe.inputs[i])].ToString() + " /" + _machineCraftRecipe.inputsAmount[i].ToString() + " " + _machineCraftRecipe.inputs[i].nom;

                        //si il y a assez de ce materiaux pour le craft alors l'image rouge par dessus le materiaux disparait et le texte n'est plus rouge
                        if(machineItemAmountList[machineItemList.IndexOf(_machineCraftRecipe.inputs[i])] >= _machineCraftRecipe.inputsAmount[i])
                        {
                            _machineRecipeUI.transform.GetChild(i * 2).GetChild(1).gameObject.SetActive(false);
                            _recipeMaterialList[i].color = new Color(0,0,0);
                        }
                        else
                        {
                            _machineRecipeUI.transform.GetChild(i * 2).GetChild(1).gameObject.SetActive(true);
                            _recipeMaterialList[i].color = new Color(1,0,0);
                        }
                    }
                    else
                    {
                        _machineRecipeUI.transform.GetChild(i * 2).GetChild(1).gameObject.SetActive(true);
                        _recipeMaterialList[i].text = "0 /" + _machineCraftRecipe.inputsAmount[i].ToString() + " " + _machineCraftRecipe.inputs[i].nom;
                        _recipeMaterialList[i].color = new Color(1,0,0);
                    }
                }
                else
                {
                    //si le materiaux/objet est le resultat du craft et non requis alors on lui met juste le nombre que l'on en reçois sur le texte
                    _machineRecipeUI.transform.GetChild(i * 2).GetChild(1).gameObject.SetActive(false);
                    _recipeMaterialList[i].text = _machineCraftRecipe.outputsAmount[i - _machineCraftRecipe.inputs.Count].ToString() + " " + _machineCraftRecipe.outputs[i - _machineCraftRecipe.inputs.Count].nom;  
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
            //regarde si un nouvel objet a été déposer dans l'inventaire, si oui il le crée
            if(_machineInventoryDropSlotUI.transform.childCount > 0)
            {
                thisMachineInventoryItem = _machineInventoryDropSlotUI.transform.GetChild(0).GetComponent<InventoryItem>();

                AddItemToInventory(thisMachineInventoryItem.item, thisMachineInventoryItem.count, false);
                Destroy(thisMachineInventoryItem.gameObject);
            }

            //ajuste le nombre de materiaux sur l'ui de l'inventaire de la machine
            try
            {
                for (var i = 0; i < _machineInventoryAmountTextList.Count; i++)
                {
                    _machineInventoryAmountTextList[i].text = machineItemAmountList[i].ToString();
                }
            }catch(NullReferenceException){}

            yield return new WaitForSeconds(0.05f);
        }
    }
    
    //ajoute l'objet/matériaux à l'inventaire, si il est déja présent alors il ajoute un montant a cet objet/matériaux
    //ceci inclut la liste des items, un montant nul qui sera calculer par la suite, son sprite et montant dans l'ui de l'inventaire
    private void AddItemToInventory(ItemClass thisItem, int thisAmount, bool isContained)
    {
        if(!machineItemList.Contains(thisItem))
        {
            machineItemList.Add(thisItem);
            machineItemAmountList.Add(0);
            isContained = true;
        }
        
        if (isUIOpen && isContained)
        {
            _thisMachineItemButtonUI = Instantiate(inventoryItemButtonUIPrefab, _machineInventoryUI.transform);
            _thisMachineItemButtonUI.transform.GetChild(0).GetComponent<Image>().sprite = thisItem.sprite;
            _machineInventoryAmountTextList.Add(_thisMachineItemButtonUI.transform.GetChild(2).gameObject.GetComponent<Text>());
            _thisMachineItemButtonUI.GetComponent<Button>().onClick.AddListener(() => { StartCoroutine(TransferAmountUI(thisItem)); });
        }
        
        //ajoute le montant voulut
        machineItemAmountList[machineItemList.IndexOf(thisItem)] += thisAmount;
    }

    //enleve un montant d'un objet/matériaux et le supprime si l'inventaire n'en possède plus 
    private void RemoveItemFromInventory(ItemClass thisItem, int thisAmount)
    {
        _thisIndex = machineItemList.IndexOf(thisItem);
        machineItemAmountList[_thisIndex] -= thisAmount;

        if(machineItemAmountList[_thisIndex] <= 0)
        {
            machineItemList.Remove(thisItem);
            machineItemAmountList.Remove(machineItemAmountList[_thisIndex]);
            
            if (isUIOpen)
            {
                Destroy(_machineInventoryUI.transform.GetChild(_thisIndex).gameObject);
                _machineInventoryAmountTextList.Remove(_machineInventoryUI.transform.GetChild(_thisIndex).GetChild(2).GetComponent<Text>());
            }
        }
    }
    
    //permet de modifier le fonctionnement du levier d'activation de la machine
    //ceci inclut: si le joueur active ou désactive la machine et si la machine est désactiver de force car il n'y a pas asser de materiaux requis pour la recette 
    private IEnumerator ActivateMachine()
    {
        _machineActivationSliderUI.value = isMachineActivated ? 1 : 0;

        while (isUIOpen)
        {
            if (!Input.GetKey(KeyCode.Mouse0))
            {
                if (_machineActivationSliderUI.value <= 0.6f || _isMachineForcedToDeactivate)
                {
                    isMachineActivated = false;
                    _machineActivationSliderUI.value -= 0.05f;
                }
                
                if (!isMachineActivated && _machineActivationSliderUI.value <= 0.6f)
                {
                    _isMachineForcedToDeactivate = false;
                }
                
                if (_machineActivationSliderUI.value > 0.6f && !_isMachineForcedToDeactivate)
                {
                    isMachineActivated = true;
                    _machineActivationSliderUI.value += 0.05f;
                }
            }

            _machineProgressSliderUI.value = craftProgress / machineCraftingTime;
            
            yield return new WaitForSeconds(0.02f);
        }
    }

    //crée l'ui pour la transaction des items de la machine a déplacer dans notre inventaire
    private IEnumerator TransferAmountUI(ItemClass thisItem)
    {
        _thisTransferAmountUI = Instantiate(machineInventoryTransferUI, 
                                            _machineInventoryUI.transform.GetChild(machineItemList.IndexOf(thisItem)).transform.position,
                                            Quaternion.identity, 
                                            _thisMachineUIDisplay.transform);
        
        _transferAmountUISlider = _thisTransferAmountUI.transform.GetChild(1).GetComponent<Slider>();
        _transferAmountUISlider.maxValue = machineItemAmountList[machineItemList.IndexOf(thisItem)];
        _transferAmountUIText = _thisTransferAmountUI.transform.GetChild(2).GetComponent<Text>();
        _thisTransferAmountUI.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => { Destroy(_thisTransferAmountUI); });
        _thisTransferAmountUI.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => { ConfirmTransaction(thisItem); });
        _thisTransferAmountUI.transform.GetChild(5).GetComponent<Image>().sprite = thisItem.sprite;
        
        while (true)
        {
            if(_transferAmount != (int)_transferAmountUISlider.value)
            {
                _transferAmount = (int)_transferAmountUISlider.value;
                _transferAmountUIText.text = _transferAmount.ToString();
            }

            if(_transferAmount != int.Parse(_transferAmountUIText.text))
            {
                _transferAmount = int.Parse(_transferAmountUIText.text);
                _transferAmountUISlider.value = _transferAmount;
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    //quand le joueur accepte la transaction alors le montant voulut est réparti dans l inventaire du joueur et enlever de la machine
    private void ConfirmTransaction(ItemClass thisItem)
    {
        if (_transferAmount > machineItemAmountList[machineItemList.IndexOf(thisItem)])
            _transferAmount = machineItemAmountList[machineItemList.IndexOf(thisItem)];
        
        for(var i = 0; i < _machinePlayerInventoryList.Count; i ++)
        {
            if(_machinePlayerInventoryList[i].childCount == 0 && _transferAmount > 0)
            {
                thisMachineInventoryItem = Instantiate(inventoryItemPrefab, _machinePlayerInventoryList[i]).GetComponent<InventoryItem>();
                thisMachineInventoryItem.InitialiseItem(thisItem);
                
                _transferAmount -= thisItem.stackSize;

                if (_transferAmount < 0)
                {
                    thisMachineInventoryItem.count = _transferAmount + thisItem.stackSize;
                    RemoveItemFromInventory(thisItem, _transferAmount + thisItem.stackSize);
                    thisMachineInventoryItem.RefreshCount();
                    Destroy(_thisTransferAmountUI);
                }
                else
                {
                    thisMachineInventoryItem.count = thisItem.stackSize;
                    RemoveItemFromInventory(thisItem, thisItem.stackSize);
                    thisMachineInventoryItem.RefreshCount();
                }
            }
        }
    }

    //gere le fade out des matériaux produits sur l'output
    private IEnumerator OutputMaterialFadeOut(float time)
    {
        _machineOutputMaterialGroup = _machineOutputMaterialsUI.GetComponent<CanvasGroup>();
        _machineOutputMaterialGroup.alpha = 1;
            
        while (_machineOutputMaterialGroup.alpha > 0 && isUIOpen)
        {
            _machineOutputMaterialGroup.alpha -= 0.02f / time;
            yield return new WaitForSeconds(0.02f);
        }
    }
    
    //charge l'inventaire de la machine grace à sa liste d'item de son inventaire qu'elle possède
    private void LoadInventory()
    {
        _machineInventoryAmountTextList = new List<Text>();
        
        for (var i = 0; i < machineItemList.Count; i++)
        {
            AddItemToInventory(machineItemList[i], 0, true);
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
        _machinePlayerInventoryList = new List<Transform>();
        
        for(var i = 0; i < playerInventory.transform.childCount; i++)
        {
            _thisMachinePlayerInventorySlot = Instantiate(playerInventory.transform.GetChild(i).gameObject, _machinePlayerInventoryUI.transform);
            _machinePlayerInventoryList.Add(_thisMachinePlayerInventorySlot.transform);
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

            if(_machinePlayerInventoryList[i].childCount > 0)
            {
                Instantiate(_machinePlayerInventoryList[i].GetChild(0).gameObject, playerInventory.transform.GetChild(i));
            }
        }
    }

    //cett fonction doit être appelé par un script externe affin de de fermer correctement l'ui de la machine
    public void DeactivateUIDisplay()
    {
        SaveInventory();
        SavePlayerInventory();
        StopAllCoroutines();
        isUIOpen = false;
        Destroy(_thisMachineUIDisplay);
    }
}