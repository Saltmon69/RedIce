using UnityEngine;

public class BlueprintStartState : BlueprintBaseState
{
    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(1).gameObject.SetActive(true);
    }
    
    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        //mode de sélection de la machine à déplacer
        if(Input.GetKeyDown(KeyCode.B))
        {
            blueprint.SwitchState(blueprint.moveState);
        }

        //ouvre l'interface de construction des machines
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            blueprint.SwitchState(blueprint.buildingState);
        }

        //mode sélection de la première machine afin de relier par cable deux machine ensemble
        if(Input.GetKeyDown(KeyCode.C))
        {
            blueprint.SwitchState(blueprint.cableState);
        }
    }
    
    public override void RayState(BlueprintStateMachineManager blueprint, Ray ray, float distance){}
    
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(1).gameObject.SetActive(false);
    }
}
