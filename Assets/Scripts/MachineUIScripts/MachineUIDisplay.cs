using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        //cree des bouton dans le menu de craft celon les bouton selectionner dans l'inspecteur
        for(var i = 0; i < craftingButtonList.Count; i++)
        {
            Instantiate(craftingButtonList[i], new Vector3(_crafting.transform.position.x, _crafting.transform.position.y - i * 40, 0), Quaternion.identity, _crafting.transform);
        }
    }

    public void DeactivateUIDisplay()
    {
        Destroy(_thisMachineUIDisplay);
    }
}
