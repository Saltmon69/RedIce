using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineUIDisplay : MonoBehaviour
{
    private GameObject _machineUIPrefab;
    private GameObject _machineInventoryTransferUIPrefab;
    
    private GameObject _thisMachineUIDisplay;
    private GameObject _machineBackgroundUI;
    private GameObject _machineInventoryUI;
    private GameObject _machineInventoryDropSlotUI;
    private GameObject _machineUpgradeSlotUI;
    private GameObject _machineCraftUI;
    private GameObject _machineRecipeUI;
    private Slider _machineProgressSliderUI;
    private GameObject _machineOutputMaterialsUI;
    private Slider _machineActivationSliderUI;
    private GameObject _machinePlayerInventoryUI;
    private Button _upgradePreviewButtonUI;

    private GameObject _instantiatedMachineUIButton;
    private GameObject _instantiatedMachineUIRecipeMaterial;
    
    [HideInInspector] public bool isUIOpen;
    
    private Recipe _machineCraftRecipe;
    private float _machineCraftingTimeLeft;

    private GameObject _materialRecipePrefab;
    private GameObject _plusSignPrefab;
    private GameObject _equalSignPrefab;

    private int _machineMaterialsReadyForCraft;
    private List<bool> _isMachineMaterialReadyList;
    public float machineCraftingTime;
    [HideInInspector] public float craftProgress;
    private bool _isCraftFailed;
    [HideInInspector] public bool isMachineActivated;
    private bool _isMachineForcedToDeactivate;
    [HideInInspector] public GameObject playerInventory;

    private GameObject _craftButtonPrefab;
    public List<Recipe> machineTier1CraftList;
    public List<Recipe> machineTier2CraftList;
    public List<Recipe> machineTier3CraftList;
    private List<Recipe> _machineCraftList;
    private List<GameObject> _machineCraftingButtonList;
    public List<ItemClass> machineUpgradeItemTier;
    private InventoryItem _thisUpgradeInventoryItemInSlot;
    private bool _hasItemInUpgradeSlot;
    private int _machineUpgradeTier;

    private InventoryItem _thisMachineInventoryItem;
    [HideInInspector] public List<ItemClass> machineItemList;
    [HideInInspector] public List<int> machineItemAmountList;

    private List<Text> _recipeMaterialList;
    private int _usedRecipeIndex;
    
    private GameObject _inventoryItemPrefab;
    private InventoryItem _itemInSlot;
    private InventoryItem _itemCreated;
    
    private int _thisMachineItemAmount;
    private int _machineItemInventorySlotOffset;

    private GameObject _inventoryItemButtonUIPrefab;
    private GameObject _thisMachineItemButtonUI;
    private InventoryItem _thisMachineOutputMaterial;
    private CanvasGroup _machineOutputMaterialGroup;

    private int _thisIndex;
    private List<Text> _machineInventoryAmountTextList;

    private GameObject _thisTransferAmountUI;
    private Slider _transferAmountUISlider;
    private Text _transferAmountUIText;
    private int _transferAmount;
    private bool _isMaterialReady;
    private List<Transform> _machinePlayerInventoryList;

    private GameObject _thisMachinePlayerInventorySlot;
    
    [HideInInspector] public List<GameObject> thisMachineOutputList;
    [HideInInspector] public List<CableLaserBehaviour> thisMachineOutputCableList;
    [HideInInspector] public List<GameObject> thisMachineInputList;
    [HideInInspector] public List<CableLaserBehaviour> thisMachineInputCableList;
    public List<MachineUIDisplay> thisMachineCableMachineUIDisplayList;
    private int _machineTransferAmount;
    private int _transferMachineItemIndex;
    private int _transferMachineItemAmount;

    private GameObject _particleSystemPrefab;
    private ParticleSystem _thisParticleSystem;

    private int _craftCount;
    private int _temporaryCount;

    private bool _isInPreview;
    
    //charge tous les endroit clés que le code utilise régulierement au sein de l'ui
    private void OnDisplayInstantiate()
    {
        _thisMachineUIDisplay = Instantiate(_machineUIPrefab);
        _thisMachineUIDisplay.GetComponent<Canvas>().worldCamera = Camera.main.transform.GetChild(0).GetChild(3).GetComponent<Camera>();

        _machineBackgroundUI = _thisMachineUIDisplay.transform.GetChild(0).GetChild(0).gameObject;
        _machineInventoryUI = _thisMachineUIDisplay.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).gameObject;
        _machineInventoryDropSlotUI = _thisMachineUIDisplay.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject;
        _machineUpgradeSlotUI = _thisMachineUIDisplay.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        _upgradePreviewButtonUI = _thisMachineUIDisplay.transform.GetChild(2).GetChild(1).GetComponent<Button>();
        _machineCraftUI = _thisMachineUIDisplay.transform.GetChild(3).GetChild(0).GetChild(0).GetChild(1).gameObject;
        _machineRecipeUI = _thisMachineUIDisplay.transform.GetChild(4).GetChild(1).GetChild(0).gameObject;
        _machineProgressSliderUI = _thisMachineUIDisplay.transform.GetChild(5).GetComponent<Slider>();
        _machineOutputMaterialsUI = _thisMachineUIDisplay.transform.GetChild(6).GetChild(0).gameObject;
        _machineActivationSliderUI = _thisMachineUIDisplay.transform.GetChild(7).GetComponent<Slider>();
        _machinePlayerInventoryUI = _thisMachineUIDisplay.transform.GetChild(8).GetChild(0).GetChild(0).GetChild(1).gameObject;
    }

    //charge les différents préfab pour l'ui de la machine
    private void LoadUIReferences()
    {
        _machineUIPrefab = Resources.Load<GameObject>("MachineUI/UIMachine");
        _machineInventoryTransferUIPrefab = Resources.Load<GameObject>("MachineUI/AmountUI");
        _materialRecipePrefab = Resources.Load<GameObject>("MachineUI/MaterialRecipe");
        _plusSignPrefab = Resources.Load<GameObject>("MachineUI/PlusSignImage");
        _equalSignPrefab = Resources.Load<GameObject>("MachineUI/EqualSignImage");
        _craftButtonPrefab = Resources.Load<GameObject>("MachineUI/CraftButton");
        _inventoryItemPrefab = Resources.Load<GameObject>("MachineUI/InventoryItem");
        _inventoryItemButtonUIPrefab = Resources.Load<GameObject>("MachineUI/InventoryButton");
        _particleSystemPrefab = Resources.Load<GameObject>("MachineUI/OutputMaterialsParticle");
    }

    //fonction qui doit etre appelé par un script externe affin d'activer l'UI de la machine
    public void ActivateUIDisplay()
    {
        isUIOpen = true;

        LoadUIReferences();
        OnDisplayInstantiate();

        if(_hasItemInUpgradeSlot) this.gameObject.transform.GetChild(this.gameObject.transform.childCount - 1).SetParent(_machineUpgradeSlotUI.transform);

        LoadInventory();
        LoadPlayerInventory();


        _machineCraftingButtonList = new List<GameObject>();
        
        //assigne chaque tier de craft a la liste de craft
        _machineCraftList = new List<Recipe>();
        foreach(var t in machineTier1CraftList) _machineCraftList.Add(t);
        foreach(var t in machineTier2CraftList) _machineCraftList.Add(t);
        foreach(var t in machineTier3CraftList) _machineCraftList.Add(t);
        
        //cree des bouton dans le menu de craft celon les bouton selectionner dans l'inspecteur, puis leur ajoute leur fonctionnalité de craft
        for (var i = 0; i < _machineCraftList.Count; i++)
        {
            var a = i;
            _instantiatedMachineUIButton = Instantiate(_craftButtonPrefab, _machineCraftUI.transform);
            _instantiatedMachineUIButton.GetComponent<Button>().onClick.AddListener(() => { SetRecipeOnClick(a, true); });
            _instantiatedMachineUIButton.transform.GetChild(0).GetComponent<Text>().text = _machineCraftList[a].name;
            _machineCraftingButtonList.Add(_instantiatedMachineUIButton);
        }

        SetCraftingButtonToMachineTier(_machineUpgradeTier);
        
        _upgradePreviewButtonUI.onClick.AddListener(() => { SetMachineToUpgradePreview(); });

        if(_usedRecipeIndex != -1) SetRecipeOnClick(_usedRecipeIndex, false);
        
        StartCoroutine(InventoryItemManager());
        StartCoroutine(ActivateMachine());
        StartCoroutine(RecipeMaterialManager());
    }

    public void Update()
    {
        if(isMachineActivated)
        {
            craftProgress += Time.deltaTime;
            
            if(craftProgress > 0 && _isCraftFailed)
            {
                craftProgress -= Time.deltaTime * 5; 
            }
            else
            {
                _isCraftFailed = false;
            }
        }
        else if(craftProgress > 0)
        {
            craftProgress -= Time.deltaTime * 5; 
        }

        //quand le craft est terminer alors on supprime le nombre de materiaux utiliser et on reçois le nombre d'objet ou matériaux crafté
        if(craftProgress >= machineCraftingTime && isMachineActivated)
        {
            _machineMaterialsReadyForCraft = 0;

            for(var i = 0; i < _machineCraftRecipe.inputs.Count; i++)
            {
                _isMachineMaterialReadyList[i] = false;
                _machineTransferAmount = _machineCraftRecipe.inputsAmount[i];
                
                //regarde les ressources prête que le joueur utilise pour construire son objet/matériaux 
                if(machineItemList.Contains(_machineCraftRecipe.inputs[i]))
                {
                    CheckForMaterials(this, _machineCraftRecipe.inputs[i]);
                    _isMachineMaterialReadyList[i] = _isMaterialReady;
                }

                if(!_isMachineMaterialReadyList[i])
                {
                    //pour chaques machines connecté par cable sur les inputs de cette machines, on essaye de recuperer les matériaux manquant pour le craft
                    //si la ou les machines contiennent la ou les matériaux requis alors cette machine va venir prendre ces ressources dans les machines
                    for(var j = 0; j < thisMachineCableMachineUIDisplayList.Count; j++)
                    {
                        if(!thisMachineInputCableList[j].isSetup) continue;
                        Debug.Log("Is Taking From Another Machine");

                        if(thisMachineCableMachineUIDisplayList[j].machineItemList.Contains(_machineCraftRecipe.inputs[i]))
                        {
                            CheckForMaterials(thisMachineCableMachineUIDisplayList[j], _machineCraftRecipe.inputs[i]);
                        }

                        if(_isMachineMaterialReadyList[i]) break;
                    }
                    
                    _isMachineMaterialReadyList[i] = _isMaterialReady;
                    if(!_isMaterialReady) break;
                }
            }

            //si il y a toutes les ressources de prete pour cree l'objet/matériaux alors on supprime ce que la recette demande et on reçois le resultat
            if(_machineMaterialsReadyForCraft == _machineCraftRecipe.inputs.Count)
            {
                Debug.Log("has enough materials");
                //supprime les ressources requises pour la création du resultat de la recette
                for(var i = 0; i < _machineCraftRecipe.inputs.Count; i++)
                {
                    RemoveItemFromInventory(_machineCraftRecipe.inputs[i], _machineCraftRecipe.inputsAmount[i]);
                }
            
                //détruit ce qu'il y avait aupart avant dans l'output affin de pouvoir faire le feedback visuel correctement plus tard
                if(isUIOpen)
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

                    if(!isUIOpen) continue;
                    //_thisMachineOutputMaterial = Instantiate(_inventoryItemPrefab, _machineOutputMaterialsUI.transform).GetComponent<InventoryItem>();
                    //_thisMachineOutputMaterial.InitialiseItem(_machineCraftRecipe.outputs[i]);

                    _thisParticleSystem = Instantiate(_particleSystemPrefab, _machineOutputMaterialsUI.transform).GetComponent<ParticleSystem>();
                    Material mat = new Material(Shader.Find("Sprites/Default"));
                    mat.SetTexture("_MainTex", _machineCraftRecipe.outputs[i].sprite.texture);
                    _thisParticleSystem.GetComponent<Renderer>().material = mat;
                    _thisParticleSystem.maxParticles = _machineCraftRecipe.outputsAmount[i];
                }
                craftProgress = 0;
            }

            _isCraftFailed = true;
        }
    }

    //regarde dans une machine (qui peut etre elle meme) si combien elle contient de matériaux et sic est asser pour le craft voulut
    private void CheckForMaterials(MachineUIDisplay machineUIDisplay, ItemClass itemRequired)
    {
        _transferMachineItemIndex = machineUIDisplay.machineItemList.IndexOf(itemRequired);
        _transferMachineItemAmount = machineUIDisplay.machineItemAmountList[_transferMachineItemIndex];
                            
        _machineTransferAmount -= _transferMachineItemAmount;
        
        _isMaterialReady = false;
        
        //si la machine n'a pas donner asser de materiaux on continue, aussi non on arrete et remplis le materiaux comme pret
        if(_machineTransferAmount > 0)
        {
            machineUIDisplay.RemoveItemFromInventory(itemRequired, _transferMachineItemAmount);
            this.AddItemToInventory(itemRequired, _transferMachineItemAmount, false);  
        }
        else
        {
            machineUIDisplay.RemoveItemFromInventory(itemRequired, _transferMachineItemAmount + _machineTransferAmount);
            this.AddItemToInventory(itemRequired, _transferMachineItemAmount + _machineTransferAmount, false);
            _machineMaterialsReadyForCraft++;
            _isMaterialReady = true;
        }
    }
    
    //fonction qu il y a sur chacun des boutons affin d'afficher la nouvelle recette pour ce craft
    private void SetRecipeOnClick(int a, bool isClicked)
    {
        //supprime l ancienne recette affiché
        for(var i = 0; i < _machineRecipeUI.transform.childCount; i++)
        {
            Destroy(_machineRecipeUI.transform.GetChild(i).gameObject);
        }

        _recipeMaterialList = new List<Text>();
        _machineCraftRecipe = _machineCraftList[a];

        //fait que le bouton reste enfoncer après la selection de craft, en revanche, si le bouton était déja appuyer alors aucun craft n'est sélectionné
        if(_usedRecipeIndex != a || !isClicked)
        {
            //met le dernier craft appuyer dans son état normal et met le nouveaux bouton appuyer avec le feedback voulut
            if(_usedRecipeIndex != -1) _machineCraftUI.transform.GetChild(_usedRecipeIndex).GetComponent<Image>().color = new Color(1,1,1);
            _machineCraftUI.transform.GetChild(a).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            _usedRecipeIndex = a;
            
            //genere la nouvelle recette avec les images contenu dans les scriptable objects qui compose le craft tout en rajoutant des signes "+" et "="
            //il assigne aussi les textes qui gere le montant des materiaux de la recette qui sera défini dans "RecipeMaterialManager"
            for(var y = 0; y < _machineCraftRecipe.inputs.Count + _machineCraftRecipe.outputs.Count; y++)
            {
                _instantiatedMachineUIRecipeMaterial = Instantiate(_materialRecipePrefab, _machineRecipeUI.transform);
                _recipeMaterialList.Add(_instantiatedMachineUIRecipeMaterial.transform.GetChild(2).GetComponent<Text>());

                _instantiatedMachineUIRecipeMaterial.transform.GetChild(0).GetComponent<Image>().sprite = y < _machineCraftRecipe.inputs.Count ? _machineCraftRecipe.inputs[y].sprite : _machineCraftRecipe.outputs[y - _machineCraftRecipe.inputs.Count].sprite;

                if(y == _machineCraftRecipe.inputs.Count - 1)
                {                    
                    Instantiate(_equalSignPrefab, _machineRecipeUI.transform);
                }
                else if(y != _machineCraftRecipe.inputs.Count + _machineCraftRecipe.outputs.Count - 1)
                {
                    Instantiate(_plusSignPrefab, _machineRecipeUI.transform);
                }
            }
        }
        else
        {
            _machineCraftUI.transform.GetChild(a).GetComponent<Image>().color = new Color(1,1,1);
            _usedRecipeIndex = -1;
        }
        
        CraftAmount();
    }

    //rafraichi l'UI de la recette pour qu elle correspond et soit directement lié avec le nombre de matériaux dans l'inventaire de la machine
    //gere sur chaque materiaux requis un feedback d'une image rouge par dessus le materiaux et d'un texte rouge si l'on a pas asser de se materiaux
    //cela sert aussi a directement confirmer si le craft est pret et peut etre fait
    private IEnumerator RecipeMaterialManager()
    {
        while(true)
        {
            if(_usedRecipeIndex != -1)
            {
                _isMachineMaterialReadyList = new List<bool>(new bool[_machineCraftRecipe.inputs.Count]);
                
                for(var i = 0; i < _recipeMaterialList.Count; i++)
                {
                    //regarde si les matériaux que l'on est en train de définir si il sont requis ou reçus
                    if(i < _machineCraftRecipe.inputs.Count)
                    {
                        _recipeMaterialList[i].transform.parent.GetChild(1).gameObject.SetActive(true);
                        _recipeMaterialList[i].color = new Color(1,0,0);
                        _recipeMaterialList[i].text = "0";
                        
                        //si l'inventaire de la machine contient ce materiaux alors il associe le nombre qu'il y en a dans l'inventaire avec l'ui 
                        if(machineItemList.Contains(_machineCraftRecipe.inputs[i]))
                        {
                            _recipeMaterialList[i].text = machineItemAmountList[machineItemList.IndexOf(_machineCraftRecipe.inputs[i])] + "";
                            
                            //si il y a assez de ce materiaux pour le craft alors l'image rouge par dessus le materiaux disparait et le texte n'est plus rouge
                            if(machineItemAmountList[machineItemList.IndexOf(_machineCraftRecipe.inputs[i])] >= _machineCraftRecipe.inputsAmount[i])
                            {
                                _recipeMaterialList[i].transform.parent.GetChild(1).gameObject.SetActive(false);
                                _recipeMaterialList[i].color = new Color(0,0,0);
                            }
                        }

                        _recipeMaterialList[i].text += "/" + _machineCraftRecipe.inputsAmount[i] + " " + _machineCraftRecipe.inputs[i].nom;
                    }
                    else
                    {
                        //si le materiaux/objet est le resultat du craft et non requis alors on lui met juste le nombre que l'on en reçois sur le texte
                        _recipeMaterialList[i].transform.parent.GetChild(1).gameObject.SetActive(false);
                        _recipeMaterialList[i].text = _machineCraftRecipe.outputsAmount[i - _machineCraftRecipe.inputs.Count] + " " + _machineCraftRecipe.outputs[i - _machineCraftRecipe.inputs.Count].nom;  
                    }
                }  
            }
            CraftAmount();
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
                _thisMachineInventoryItem = _machineInventoryDropSlotUI.transform.GetChild(0).GetComponent<InventoryItem>();
                craftProgress = 0; 
                AddItemToInventory(_thisMachineInventoryItem.item, _thisMachineInventoryItem.count, false);
                Destroy(_thisMachineInventoryItem.gameObject);
            }
            
            //ajuste le nombre de materiaux sur l'ui de l'inventaire de la machine
            if(isUIOpen)
            {
                for(var i = 0; i < _machineInventoryAmountTextList.Count; i++)
                {
                    _machineInventoryAmountTextList[i].text = machineItemAmountList[i].ToString();
                }

                //regarde si un nouvel objet a été déposer ou retiré dans le case d'upgrade, si oui, il actualise les crafts
                
                if(_machineUpgradeSlotUI.transform.childCount > 0 && !_hasItemInUpgradeSlot)
                {
                    Debug.Log("an upgrade item has been placed");

                    _thisUpgradeInventoryItemInSlot = _machineUpgradeSlotUI.transform.GetChild(0).GetComponent<InventoryItem>();
                    _hasItemInUpgradeSlot = true;

                    for(var i = 1; i <= machineUpgradeItemTier.Count; i++)
                    {
                        if(_thisUpgradeInventoryItemInSlot.item == machineUpgradeItemTier[^i])
                        {
                            _machineUpgradeTier = machineUpgradeItemTier.Count - i;
                            SetCraftingButtonToMachineTier(_machineUpgradeTier);
                        }
                    }
                }

                if(_machineUpgradeSlotUI.transform.childCount == 0 && _hasItemInUpgradeSlot)
                {
                    Debug.Log("an upgrade item has been removed");
                    _machineUpgradeTier = 0;
                    SetCraftingButtonToMachineTier(_machineUpgradeTier);
                    _machineBackgroundUI.GetComponent<Image>().color = new Color(0.5f,0.5f,0.5f, 0.5f);
                    _hasItemInUpgradeSlot = false;
                    if(_usedRecipeIndex != -1) SetRecipeOnClick(_usedRecipeIndex, true);
                    
                    //supprime la recette 
                    for(var i = 0; i < _machineRecipeUI.transform.childCount; i++)
                    {
                        Destroy(_machineRecipeUI.transform.GetChild(i).gameObject);
                    }
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
    
    //permet celon l'upgrade installé sur la machine de débloquer de nouveau crafts
    private void SetCraftingButtonToMachineTier(int upgradeSlotItemTier)
    {
        switch(upgradeSlotItemTier)
        {
            case 0:
                _machineBackgroundUI.GetComponent<Image>().color = new Color(0.5f,0.5f,0.5f, 0.5f);
                break;
            case 1:
                _machineBackgroundUI.GetComponent<Image>().color = new Color(1f,0.65f,0f, 0.5f);
                break;
            case 2:                         
                _machineBackgroundUI.GetComponent<Image>().color = new Color(1f,0f,0f, 0.5f); 
                break;
        }

        for(var i = 0; i < _machineCraftingButtonList.Count; i++)
        {
            if(i < machineTier1CraftList.Count)
            {
                _machineCraftingButtonList[i].gameObject.SetActive(true);
            }
            else if(i < machineTier1CraftList.Count + machineTier2CraftList.Count)
            {
                _machineCraftingButtonList[i].gameObject.SetActive(upgradeSlotItemTier >= 1);
            }
            else
            {
                _machineCraftingButtonList[i].gameObject.SetActive(upgradeSlotItemTier >= 2);
            }
        }
    }

    private void SetMachineToUpgradePreview()
    {
        if(!_isInPreview)
        {
            _isInPreview = true;
            _upgradePreviewButtonUI.transform.GetChild(0).gameObject.SetActive(true);
            SetCraftingButtonToMachineTier(_machineUpgradeTier + 1);
        }
        else
        {
            _isInPreview = false;
            _upgradePreviewButtonUI.transform.GetChild(0).gameObject.SetActive(false);
            SetCraftingButtonToMachineTier(_machineUpgradeTier);
            
            if(_usedRecipeIndex != -1) SetRecipeOnClick(_usedRecipeIndex, true);
                    
            //supprime la recette 
            for(var i = 0; i < _machineRecipeUI.transform.childCount; i++)
            {
                Destroy(_machineRecipeUI.transform.GetChild(i).gameObject);
            }
        }
    }
    
    //ajoute l'objet/matériaux à l'inventaire, si il est déja présent alors il ajoute un montant a cet objet/matériaux
    //ceci inclut la liste des items, un montant nul qui sera calculer par la suite, son sprite et montant dans l'ui de l'inventaire
    public void AddItemToInventory(ItemClass thisItem, int thisAmount, bool isContained)
    {
        if(!machineItemList.Contains(thisItem))
        {
            machineItemList.Add(thisItem);
            machineItemAmountList.Add(0);
            isContained = true;
        }
        
        //ajoute le montant voulut
        machineItemAmountList[machineItemList.IndexOf(thisItem)] += thisAmount;
        
        if(isUIOpen && isContained)
        {
            _thisMachineItemButtonUI = Instantiate(_inventoryItemButtonUIPrefab, _machineInventoryUI.transform);
            _thisMachineItemButtonUI.transform.GetChild(0).GetComponent<Image>().sprite = thisItem.sprite;
            _machineInventoryAmountTextList.Add(_thisMachineItemButtonUI.transform.GetChild(2).gameObject.GetComponent<Text>());
            _machineInventoryAmountTextList[^1].text = thisAmount.ToString();
            _thisMachineItemButtonUI.GetComponent<Button>().onClick.AddListener(() => { StartCoroutine(TransferAmountUI(thisItem)); });
        }
    }

    //enleve un montant d'un objet/matériaux et le supprime si l'inventaire n'en possède plus 
    public void RemoveItemFromInventory(ItemClass thisItem, int thisAmount)
    {
        _thisIndex = machineItemList.IndexOf(thisItem);
        machineItemAmountList[_thisIndex] -= thisAmount;

        if(machineItemAmountList[_thisIndex] <= 0)
        {
            machineItemList.Remove(thisItem);
            machineItemAmountList.Remove(machineItemAmountList[_thisIndex]);
            
            if(isUIOpen)
            {
                _machineInventoryAmountTextList.Remove(_machineInventoryAmountTextList[_thisIndex]);
                DestroyImmediate(_machineInventoryUI.transform.GetChild(_thisIndex).gameObject);
            }
        }
    }

    private void CraftAmount()
    {
        if(!isUIOpen) return;
        _craftCount = 0;
        
        for(var i = 0; i < _machineCraftRecipe.inputs.Count; i++)
        {
            try
            {
                _temporaryCount = machineItemAmountList[machineItemList.IndexOf(_machineCraftRecipe.inputs[i])] / _machineCraftRecipe.inputsAmount[i];
            }
            catch (ArgumentOutOfRangeException)
            {
                break;
            }

            if(_temporaryCount == 0) break;

            if(i == 0)
            {
                _craftCount = _temporaryCount;
                continue;
            }

            if(_craftCount > _temporaryCount)
            {
                _craftCount = _temporaryCount;
            }
        }

        _machineOutputMaterialsUI.transform.parent.GetChild(1).GetComponent<Text>().text = _craftCount +"";
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
                if(_isInPreview) _isMachineForcedToDeactivate = true;
                
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
        _thisTransferAmountUI = Instantiate(_machineInventoryTransferUIPrefab, 
                                            _machineInventoryUI.transform.GetChild(machineItemList.IndexOf(thisItem)).transform.position,
                                            _thisMachineUIDisplay.transform.rotation,
                                            _thisMachineUIDisplay.transform);
        
        _transferAmountUISlider = _thisTransferAmountUI.transform.GetChild(1).GetComponent<Slider>();
        if(_transferAmountUISlider.maxValue > thisItem.stackSize) _transferAmountUISlider.maxValue = thisItem.stackSize;
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
        if(_transferAmount > machineItemAmountList[machineItemList.IndexOf(thisItem)])
            _transferAmount = machineItemAmountList[machineItemList.IndexOf(thisItem)];

        if (_transferAmount > thisItem.stackSize) _transferAmount = thisItem.stackSize;
        
        for(var i = 0; i < _machinePlayerInventoryList.Count; i ++)
        {
            if(_machinePlayerInventoryList[i].childCount != 0) continue;
            _thisMachineInventoryItem = Instantiate(_inventoryItemPrefab, _machinePlayerInventoryList[i]).GetComponent<InventoryItem>();
            _thisMachineInventoryItem.InitialiseItem(thisItem);

            _thisMachineInventoryItem.count = _transferAmount;
            RemoveItemFromInventory(thisItem, _transferAmount);
            _thisMachineInventoryItem.RefreshCount();
            Destroy(_thisTransferAmountUI);
            break;
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
        isUIOpen = false;
        SavePlayerInventory();
        StopAllCoroutines();
        if(_machineUpgradeSlotUI.transform.childCount > 0) _machineUpgradeSlotUI.gameObject.transform.SetParent(this.transform);
        Destroy(_thisMachineUIDisplay);
    }
}