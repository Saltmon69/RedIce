using UnityEngine;

public class BlueprintStartState : BlueprintBaseState
{
    private PlayerMenuing _playerMenuing;
    
    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(1).gameObject.SetActive(true);
        GameObject.Find("ModeSelectionManager").GetComponent<PlayerModeSelect>().canPlayerSwitchMode = true;
        
        _playerMenuing = GameObject.FindWithTag("Player").GetComponent<PlayerMenuing>();
    }
    
    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        //mode de sélection de la machine à déplacer
        if(Input.GetKeyDown(KeyCode.B))
        {
            blueprint.SwitchState(blueprint.moveState);
        }

        //ouvre l'interface de construction des machines
        if(Input.GetKeyDown(KeyCode.Mouse1) && !_playerMenuing.inMenu)
        {
            blueprint.SwitchState(blueprint.buildingState);
        }

        //mode sélection de la première machine afin de relier par cable deux machine ensemble
        if(Input.GetKeyDown(KeyCode.C))
        {
            blueprint.SwitchState(blueprint.cableState);
        }
    }
    
    public override void RayState(BlueprintStateMachineManager blueprint, RaycastHit hitData, RaycastHit oldHitData, bool hadHit){}
    
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(1).gameObject.SetActive(false);
        GameObject.Find("ModeSelectionManager").GetComponent<PlayerModeSelect>().canPlayerSwitchMode = false;
    }
}
