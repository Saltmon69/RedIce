using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlueprintMode : MonoBehaviour
{
    public List<KeyCode> keyCodeToPreviousModes;
    public List<Boolean> previousModesRequirements;
    public List<GameObject> previousModes;

    public List<KeyCode> keyCodeToNextModes;
    public List<Boolean> nextModesRequirements;
    public List<GameObject> nextModes;

    private List<MonoBehaviour> _attachedScripts;
    private List<Boolean> _previousStartingRequirements;
    private List<Boolean> _nextStartingRequirements;
    void Awake()
    {
        _attachedScripts = this.gameObject.GetComponents<MonoBehaviour>().ToList();
        _previousStartingRequirements = new List<Boolean>(previousModesRequirements);
        _nextStartingRequirements = new List<Boolean>(nextModesRequirements);
    }

    public void AllComponentsOff()
    {
        for (var i = 0; i < _attachedScripts.Count; i++)
        {
            _attachedScripts[i].enabled = false;
        }
    }
    
    public void AllComponentsOn()
    {
        for (var i = 0; i < _attachedScripts.Count; i++)
        {
            _attachedScripts[i].enabled = true;
        }
    }

    void Update()
    {
        GoBack(); 
        GoForward();
    }

    public void GoBack()
    {
        for (var i = 0; i < previousModes.Count; i++)
        {
            if (Input.GetKeyDown(keyCodeToPreviousModes[i]) && previousModesRequirements[i])
            {
                previousModes[i].SetActive(true);
                previousModes[i].GetComponent<BlueprintMode>().AllComponentsOn();
                ResetRequirements();
                this.gameObject.SetActive(false);
            }
        }
    }

    public void GoForward()
    {
        for (var i = 0; i < nextModes.Count; i++)
        {
            if (Input.GetKeyDown(keyCodeToNextModes[i]) && nextModesRequirements[i])
            {
                nextModes[i].SetActive(true);
                ResetRequirements();
                AllComponentsOff();
            }
        }
    }

    public void ResetRequirements()
    {
        previousModesRequirements = new List<Boolean>(_previousStartingRequirements);
        nextModesRequirements = new List<Boolean>(_nextStartingRequirements);
    }
}
