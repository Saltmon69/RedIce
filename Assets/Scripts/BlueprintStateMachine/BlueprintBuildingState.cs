using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueprintBuildingState : BlueprintBaseState
{
    public GameObject machineChoiceCanvas;
    public UnityEngine.Object[] machinesPrefab;
    public GameObject machineStock;
    public GameObject machineSelectedPlacementMode;
    private bool _machinesLoaded;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(4).gameObject.SetActive(true);

        machineStock = GameObject.Find("MachineStock");
        machineChoiceCanvas = GameObject.Find("MachineBuildingPanel");
        machineChoiceCanvas = machineChoiceCanvas.transform.GetChild(0).gameObject;
        machineChoiceCanvas.SetActive(true);

        machinesPrefab = Resources.LoadAll("Machines", typeof(GameObject));

        if(!_machinesLoaded)
        {
            for (var i = 1; i < machineChoiceCanvas.transform.childCount - 1; i++)
            {
                var a = i - 1;
                machineChoiceCanvas.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => { MachineChosen(a, blueprint); });
            }
            _machinesLoaded = true;
        }
    }

    void MachineChosen(int machineNumber, BlueprintStateMachineManager blueprint)
    {
        machineSelectedPlacementMode = GameObject.Instantiate((GameObject)machinesPrefab[machineNumber], machineStock.transform);
        blueprint.SwitchState(blueprint.placementState);
    }
    
    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            blueprint.SwitchState(blueprint.startState);
        }
    }
    
    public override void RayState(BlueprintStateMachineManager blueprint, Ray ray, float distance){}
        
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(4).gameObject.SetActive(false);
        machineChoiceCanvas.SetActive(false);
    }
}
