using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlueprintMode : MonoBehaviour
{
    public List<KeyCode> keyCodeToPreviousModes;
    public List<bool> previousModesRequirements;
    public List<GameObject> previousModes;

    public List<KeyCode> keyCodeToNextModes;
    public List<bool> nextModesRequirements;
    public List<GameObject> nextModes;

    private List<MonoBehaviour> _attachedScripts;
    private List<bool> _previousStartingRequirements;
    private List<bool> _nextStartingRequirements;
    private void Awake()
    {
        _attachedScripts = this.gameObject.GetComponents<MonoBehaviour>().ToList();
        _previousStartingRequirements = new List<bool>(previousModesRequirements);
        _nextStartingRequirements = new List<bool>(nextModesRequirements);
    }

    public void AllComponentsOff()
    {
        foreach (var scripts in _attachedScripts)
        {
            scripts.enabled = false;
        }
    }
    
    public void AllComponentsOn()
    {
        foreach (var scripts in _attachedScripts)
        {
            scripts.enabled = true;
        }
    }

    private void Update()
    {
        GoBack(); 
        GoForward();
    }

    private void GoBack()
    {
        for (var i = 0; i < previousModes.Count; i++)
        {
            if (Input.GetKeyDown(keyCodeToPreviousModes[i]) && previousModesRequirements[i])
            {
                previousModes[i].GetComponent<BlueprintMode>().AllComponentsOn();
                ResetRequirements();
                this.gameObject.SetActive(false);
            }
        }
    }

    private void GoForward()
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
        previousModesRequirements = new List<bool>(_previousStartingRequirements);
        nextModesRequirements = new List<bool>(_nextStartingRequirements);
    }
}
