using UnityEngine;
using UnityEngine.UI;

public class BlueprintBuildingState : BlueprintBaseState
{
    private GameObject _machineChoiceCanvas;
    private UnityEngine.Object[] _machinesPrefab;
    private GameObject _machineStock;
    private GameObject _machineSelectedPlacementMode;
    private bool _machinesLoaded;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(4).gameObject.SetActive(true);

        _machineStock = GameObject.Find("MachineStock");

        //active l'interface de sélection des machines
        _machineChoiceCanvas = GameObject.Find("MachineBuildingPanel");
        _machineChoiceCanvas = _machineChoiceCanvas.transform.GetChild(0).gameObject;
        _machineChoiceCanvas.SetActive(true);

        _machinesPrefab = Resources.LoadAll("Machines", typeof(GameObject));

        //si les machine non pas précédement été chargé, alors on assigne chaque bouton a sa machine correspondante
        if(!_machinesLoaded)
        {
            for (var i = 1; i < _machineChoiceCanvas.transform.childCount - 1; i++)
            {
                var a = i - 1;
                _machineChoiceCanvas.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => { MachineChosen(a, blueprint); });
            }
            _machinesLoaded = true;
        }
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //fonction sur chacun des boutons permettant de crée la machine en plus de nous faire passer au mode de placement de la machine
    void MachineChosen(int machineNumber, BlueprintStateMachineManager blueprint)
    {
        _machineSelectedPlacementMode = GameObject.Instantiate((GameObject)_machinesPrefab[machineNumber], _machineStock.transform);
        blueprint.SwitchState(blueprint.placementState);
    }
    
    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        //retour au mode de sélection de la machine a construire
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            blueprint.SwitchState(blueprint.startState);
        }
    }
    
    public override void RayState(BlueprintStateMachineManager blueprint, RaycastHit hitData, RaycastHit oldHitData){}
        
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(4).gameObject.SetActive(false);
        _machineChoiceCanvas.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}