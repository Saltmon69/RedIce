using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;

public class MachineUIDisplay : MonoBehaviour
{
    public GameObject machineUIPrefab;
    private GameObject _thisMachineUIDisplay;
    private GameObject _inventory;
    private GameObject _upgradeSlot;
    private GameObject _crafting;
    private GameObject _recipe;
    private GameObject _progressBar;
    private GameObject _outputSlot;
    private GameObject _instantiatedButton;
    private GameObject _instantiatedImage;
    private Recipe _craft;
    public GameObject basicImage;
    public GameObject plusSign;
    public GameObject equalSign;

    public List<GameObject> craftingButtonList;

    //charge tous les endroit clés que le code utilise régulierement
    public void OnDisplayInstantiate()
    {
        _inventory = _thisMachineUIDisplay.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        _upgradeSlot = _thisMachineUIDisplay.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        _crafting = _thisMachineUIDisplay.transform.GetChild(3).GetChild(0).GetChild(0).GetChild(0).gameObject;
        _recipe = _thisMachineUIDisplay.transform.GetChild(4).GetChild(0).GetChild(0).gameObject;
        _progressBar = _thisMachineUIDisplay.transform.GetChild(5).gameObject;
        _outputSlot = _thisMachineUIDisplay.transform.GetChild(6).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
    }

    public void ActivateUIDisplay()
    {
        _thisMachineUIDisplay = Instantiate(machineUIPrefab);

        OnDisplayInstantiate();

        //cree des bouton dans le menu de craft celon les bouton selectionner dans l'inspecteur, puis leur ajoute leur fonctionnalité de craft
        for(var i = 0; i < craftingButtonList.Count; i++)
        {
            _instantiatedButton = Instantiate(craftingButtonList[i], new Vector3(_crafting.transform.position.x, _crafting.transform.position.y - i * 40, 0), Quaternion.identity, _crafting.transform);
            var a = i;
            _instantiatedButton.GetComponent<Button>().onClick.AddListener(() => { OnCraftButtonClick(a); });
        }
    }

    public void OnCraftButtonClick(int a)
    {
        for(var i = 0; i < _recipe.transform.childCount; i++)
        {
            Destroy(_recipe.transform.GetChild(i).gameObject);
        }

        _craft = craftingButtonList[a].GetComponent<ButtonCraft>().craft;

        for(var j = 0; j < _craft.inputs.Count; j++)
        {
            _instantiatedImage = Instantiate(basicImage, new Vector3(_recipe.transform.position.x + 50 + j * 120, _recipe.transform.position.y - 50, 0), Quaternion.identity, _recipe.transform);
            _instantiatedImage.GetComponent<Image>().sprite = _craft.inputs[j].sprite;

            if(j + 1 != _craft.inputs.Count)
            {
                Instantiate(plusSign, new Vector3(_recipe.transform.position.x + 110 + j * 120, _recipe.transform.position.y - 50, 0), Quaternion.identity, _recipe.transform);
            }
        }

        Instantiate(equalSign, new Vector3(_recipe.transform.position.x + 110 + 120 * (_craft.inputs.Count - 1), _recipe.transform.position.y - 50, 0), Quaternion.identity, _recipe.transform);
        
        for(var k = 0; k < _craft.outputs.Count; k++)
        {
            _instantiatedImage = Instantiate(basicImage, new Vector3(_recipe.transform.position.x + 50 + (k + _craft.inputs.Count) * 120, _recipe.transform.position.y - 50, 0), Quaternion.identity, _recipe.transform);
            _instantiatedImage.GetComponent<Image>().sprite = _craft.outputs[k].sprite;

            if(k + 1 != _craft.outputs.Count)
            {
                Instantiate(plusSign, new Vector3(_recipe.transform.position.x + 110 + (k + _craft.inputs.Count) * 120, _recipe.transform.position.y - 50, 0), Quaternion.identity, _recipe.transform);
            }
        }

    }

    public void DeactivateUIDisplay()
    {
        Destroy(_thisMachineUIDisplay);
    }
}
