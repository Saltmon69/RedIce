using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;


public class UICursorDataDisplay : MonoBehaviour
{
    private PointerEventData _pointer;
    private List<RaycastResult> _resultList;
    private List<RaycastResult> _oldResultList;
    private GameObject _infoCursorDisplayPrefab;
    private GameObject _thisInfoCursorDisplay;
    public float baseDelay;
    public float currentDelay;

    private ItemClass _itemDisplaying;
    public bool _isUIUp;

    private Vector3 _mousePosition;
    public Vector3 displayOffset;

    private Transform _thisDisplayOverview;
    private Transform _thisDisplayCraft;
    private Transform _thisDisplayUses;

    private Recipe[] _allCraftArray;
    private GameObject[] _allMachineArray;

    public List<Recipe> inputRecipeList;
    public List<Recipe> outputRecipeList;

    private GameObject _instantiatedMachineUIRecipeMaterial;
    private GameObject _materialRecipePrefab;
    private GameObject _plusSignPrefab;
    private GameObject _equalSignPrefab;
    private GameObject _machineMaterialPrefab;
    private GameObject _thisMachineMaterial;

    private void Awake()
    {
        _infoCursorDisplayPrefab = Resources.Load<GameObject>("MachineUI/OnCursorInformationUI");
        
        _materialRecipePrefab = Resources.Load<GameObject>("MachineUI/MaterialRecipe");
        _plusSignPrefab = Resources.Load<GameObject>("MachineUI/PlusSignImage");
        _equalSignPrefab = Resources.Load<GameObject>("MachineUI/EqualSignImage");
        _machineMaterialPrefab = Resources.Load<GameObject>("MachineUI/MachineCrafts");

        currentDelay = baseDelay;

        //_allCraftArray = new Recipe[]{};
        _allCraftArray = Resources.LoadAll<Recipe>("Crafts");

        _allMachineArray = Resources.LoadAll<GameObject>("Machines");

        
        StartCoroutine(CursorOnMaterial());
    }

    private IEnumerator CursorOnMaterial()
    {
        while(true)
        {
            _pointer = new PointerEventData(EventSystem.current);
            _pointer.position = Mouse.current.position.ReadValue();
            _resultList = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_pointer, _resultList);
            
            try
            {
                if (!_isUIUp)
                {
                    if(_resultList[0].gameObject != _oldResultList[0].gameObject)
                    {
                        currentDelay = baseDelay;

                        _oldResultList = _resultList;
                    }
                    
                    currentDelay -= 0.05f;
                    
                    if(currentDelay <= 0.05f && currentDelay >= 0f) CursorUIInformationDisplay();
                }
                else if(!_thisInfoCursorDisplay.gameObject.activeInHierarchy)
                {
                    Destroy(_thisInfoCursorDisplay.gameObject);
                    _isUIUp = false;
                }
            }catch(ArgumentException){ _oldResultList = _resultList; }

            yield return new WaitForSeconds(0.05f);
        }
    }

    private void CursorUIInformationDisplay()
    {
        currentDelay = baseDelay;

        try
        {
            //Debug.Log(_resultList[0].gameObject);
            _itemDisplaying = _resultList[0].gameObject.GetComponent<InventoryItem>().item;
        }
        catch(NullReferenceException){ return; }
        
        _isUIUp = true;
        Debug.Log("in");
        
        _thisInfoCursorDisplay = Instantiate(_infoCursorDisplayPrefab, _resultList[0].gameObject.transform.position, this.gameObject.transform.GetChild(0).rotation, this.gameObject.transform.GetChild(0));
        _thisInfoCursorDisplay.GetComponent<RectTransform>().localPosition += displayOffset;
        
        DisplayOverview();

        DisplayCraftRecipeListMaker();
        DisplayCraft();
        DisplayUses();
    }

    private void DisplayOverview()
    {
        _thisDisplayOverview = _thisInfoCursorDisplay.transform.GetChild(2);
        _thisDisplayOverview.GetChild(1).GetComponent<Image>().sprite = _itemDisplaying.sprite;
        _thisDisplayOverview.GetChild(2).GetChild(0).GetComponent<Text>().text = _itemDisplaying.nom;
        _thisDisplayOverview.GetChild(2).GetChild(2).GetComponent<Text>().text = _itemDisplaying.atomicMass + "";
        _thisDisplayOverview.GetChild(2).GetChild(3).GetComponent<Text>().text = _itemDisplaying.description;
    }

    private void DisplayCraftRecipeListMaker()
    {
        inputRecipeList = new List<Recipe>();
        outputRecipeList = new List<Recipe>();
        
        for(var i = 0; i < _allCraftArray.Length; i++)
        {
            if(_allCraftArray[i].inputs.Contains(_itemDisplaying))
            {
                inputRecipeList.Add(_allCraftArray[i]);
            } 
            
            if(_allCraftArray[i].outputs.Contains(_itemDisplaying))
            {
                outputRecipeList.Add(_allCraftArray[i]);
            }
        }
    }
    
    private void DisplayCraft()
    {
        _thisDisplayCraft = _thisInfoCursorDisplay.transform.GetChild(1);
        _thisDisplayCraft.GetChild(1).GetComponent<Image>().sprite = _itemDisplaying.sprite;
        _thisDisplayCraft.GetChild(2).GetChild(0).GetComponent<Text>().text = _itemDisplaying.nom;

        DisplayMaterialFromMachine(outputRecipeList[0], _thisDisplayCraft.GetChild(3).GetChild(0).GetChild(0));
    }

    private void DisplayUses()
    {
        _thisDisplayUses = _thisInfoCursorDisplay.transform.GetChild(0);

        for (var i = 0; i < inputRecipeList.Count; i++)
        {
            DisplayMaterialFromMachine(inputRecipeList[i], _thisDisplayUses.GetChild(1).GetChild(0).GetChild(0));
        }
    }

    private void DisplayMaterialFromMachine(Recipe thisRecipe, Transform thisDisplayLocation)
    {
        _thisMachineMaterial = Instantiate(_machineMaterialPrefab, thisDisplayLocation);
        
        for(var i = 0; i < _allMachineArray.Length; i++)
        {
            try
            {
                if(_allMachineArray[i].GetComponent<MachineUIDisplay>().machineTier1CraftList.Contains(thisRecipe))
                {
                    _thisMachineMaterial.transform.GetChild(0).GetComponent<Image>().color = Color.gray;
                    _thisMachineMaterial.transform.GetChild(1).GetComponent<Image>().sprite = _allMachineArray[i].GetComponent<SpriteRenderer>().sprite;
                    _thisMachineMaterial.transform.GetChild(2).GetComponent<Text>().text = _allMachineArray[i].name;
                }
                if(_allMachineArray[i].GetComponent<MachineUIDisplay>().machineTier2CraftList.Contains(thisRecipe))
                {
                    _thisMachineMaterial.transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
                    _thisMachineMaterial.transform.GetChild(1).GetComponent<Image>().sprite = _allMachineArray[i].GetComponent<SpriteRenderer>().sprite;
                    _thisMachineMaterial.transform.GetChild(2).GetComponent<Text>().text = _allMachineArray[i].name;
                }
                if(_allMachineArray[i].GetComponent<MachineUIDisplay>().machineTier3CraftList.Contains(thisRecipe))
                {
                    _thisMachineMaterial.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                    _thisMachineMaterial.transform.GetChild(1).GetComponent<Image>().sprite = _allMachineArray[i].GetComponent<SpriteRenderer>().sprite;
                    _thisMachineMaterial.transform.GetChild(2).GetComponent<Text>().text = _allMachineArray[i].name;
                }
                
            }catch(NullReferenceException){}
        }
        
        //genere la nouvelle recette avec les images contenu dans les scriptable objects qui compose le craft tout en rajoutant des signes "+" et "="
        for(var y = 0; y < thisRecipe.inputs.Count + thisRecipe.outputs.Count; y++)
        {
            _instantiatedMachineUIRecipeMaterial = Instantiate(_materialRecipePrefab, _thisMachineMaterial.transform.GetChild(3).transform);

            _instantiatedMachineUIRecipeMaterial.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            _instantiatedMachineUIRecipeMaterial.transform.GetChild(2).GetComponent<Text>().text = y < thisRecipe.inputs.Count ? thisRecipe.inputsAmount[y] + "" : thisRecipe.outputsAmount[y - thisRecipe.inputs.Count] + "";
            _instantiatedMachineUIRecipeMaterial.transform.GetChild(1).gameObject.SetActive(false);
            _instantiatedMachineUIRecipeMaterial.transform.GetChild(0).GetComponent<Image>().sprite = y < thisRecipe.inputs.Count ? thisRecipe.inputs[y].sprite : thisRecipe.outputs[y - thisRecipe.inputs.Count].sprite;

            if(y == thisRecipe.inputs.Count - 1)
            {                    
                Instantiate(_equalSignPrefab, _thisMachineMaterial.transform.GetChild(3).transform);
            }
            else if(y != thisRecipe.inputs.Count + thisRecipe.outputs.Count - 1)
            {
                Instantiate(_plusSignPrefab, _thisMachineMaterial.transform.GetChild(3).transform);
            }
        }
    }
}
