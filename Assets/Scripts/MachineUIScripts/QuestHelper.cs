using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHelper : MonoBehaviour
{

    private bool _zPressed;
    private bool _qPressed;
    private bool _sPressed;
    private bool _dPressed;
    public bool zqsdPressed;

    public bool isComputerPlaced;

    private GameObject _machineCableStock;
    public int numberOfCablesPlaced;

    private GameObject _machineStock;
    public bool isDisplacementUsed;

    private void Start()
    {
        _machineCableStock = GameObject.Find("CableStock");
        _machineStock = GameObject.Find("MachineStock");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W)) _zPressed = true;
        if(Input.GetKeyDown(KeyCode.A)) _qPressed = true;
        if(Input.GetKeyDown(KeyCode.S)) _sPressed = true;
        if(Input.GetKeyDown(KeyCode.D)) _dPressed = true;

        if(_zPressed && _qPressed && _sPressed && _dPressed) zqsdPressed = true;

        if(GameObject.FindWithTag("Computer") != null) isComputerPlaced = true;

        numberOfCablesPlaced = _machineCableStock.transform.childCount;

        if (_machineStock.transform.childCount > 1)
        {
            if(!_machineStock.transform.GetChild(_machineStock.transform.childCount - 1).GetComponent<MachineCollider>().enabled) isDisplacementUsed = false; 
        }
    }
}
