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

    private void Awake()
    {
        _infoCursorDisplayPrefab = Resources.Load<GameObject>("MachineUI/OnCursorInformationUI");
        
        _materialRecipePrefab = Resources.Load<GameObject>("MachineUI/MaterialRecipe");
        _plusSignPrefab = Resources.Load<GameObject>("MachineUI/PlusSignImage");
        _equalSignPrefab = Resources.Load<GameObject>("MachineUI/EqualSignImage");

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
            Debug.Log(_resultList[0].gameObject);
            _itemDisplaying = _resultList[0].gameObject.GetComponent<InventoryItem>().item;
        }
        catch(NullReferenceException){ return; }
        
        _isUIUp = true;
        Debug.Log("in");
        
        _thisInfoCursorDisplay = Instantiate(_infoCursorDisplayPrefab, _resultList[0].gameObject.transform.position, this.gameObject.transform.GetChild(0).rotation, this.gameObject.transform.GetChild(0));
        _thisInfoCursorDisplay.GetComponent<RectTransform>().position += displayOffset;
        
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

        for(var i = 0; i < _allMachineArray.Length; i++)
        {
            try
            {
                if(_allMachineArray[i].GetComponent<MachineUIDisplay>()._machineCraftList.Contains(outputRecipeList[0]))
                {
                    Debug.Log("sprite");
                    _thisDisplayCraft.GetChild(3).GetChild(1).GetComponent<Image>().sprite = _allMachineArray[i].GetComponent<SpriteRenderer>().sprite;
                    Debug.Log("sprite2");
                }
            }catch(NullReferenceException){}
        }
        
        //genere la nouvelle recette avec les images contenu dans les scriptable objects qui compose le craft tout en rajoutant des signes "+" et "="
        for(var y = 0; y < outputRecipeList[0].inputs.Count + outputRecipeList[0].outputs.Count; y++)
        {
            _instantiatedMachineUIRecipeMaterial = Instantiate(_materialRecipePrefab, _thisDisplayCraft.GetChild(3).GetChild(3).transform);
            
            _instantiatedMachineUIRecipeMaterial.transform.GetChild(2).GetComponent<Text>().text = y < outputRecipeList[0].inputs.Count ? outputRecipeList[0].inputsAmount[y] + "" : outputRecipeList[0].outputsAmount[y - outputRecipeList[0].inputs.Count] + "";
            _instantiatedMachineUIRecipeMaterial.transform.GetChild(0).GetComponent<Image>().sprite = y < outputRecipeList[0].inputs.Count ? outputRecipeList[0].inputs[y].sprite : outputRecipeList[0].outputs[y - outputRecipeList[0].inputs.Count].sprite;

            if(y == outputRecipeList[0].inputs.Count - 1)
            {                    
                Instantiate(_equalSignPrefab, _thisDisplayCraft.GetChild(3).GetChild(3).transform);
            }
            else if(y != outputRecipeList[0].inputs.Count + outputRecipeList[0].outputs.Count - 1)
            {
                Instantiate(_plusSignPrefab, _thisDisplayCraft.GetChild(3).GetChild(3).transform);
            }
        }
    }

    private void DisplayUses()
    {
        _thisDisplayUses = _thisInfoCursorDisplay.transform.GetChild(0);
    }
}
