using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class ConstructionPanel : MonoBehaviour
{

    public GameObject machineChoiceCanvas;

    public List<Button> machineChoice;
    public List<GameObject> machinesPrefab;

    private BlueprintMode _blueprintMode;
    public GameObject machineSelectedPlacementMode;

    private void Awake()
    {
        _blueprintMode = this.gameObject.GetComponent<BlueprintMode>();
    }

    void OnEnable()
    {
        machineChoiceCanvas.SetActive(true);
    }

    void OnDisable()
    {
        machineChoiceCanvas.SetActive(false);
    }

    void Update()
    {
        for (var i = 0; i < machineChoice.Count; i++)
        {
            machineChoice[i].onClick.AddListener(() => MachineChosen(i));
        }
    }

    void MachineChosen(int machineNumber)
    {
        //instnatiate un holograme puis avec le mode deplacement, le machine suis la souris avec un raycast qui ne touche que le Layer Baseground
        machineSelectedPlacementMode = Instantiate(machinesPrefab[machineNumber]);
        _blueprintMode.nextModesRequirements[0] = true;
    }
}
