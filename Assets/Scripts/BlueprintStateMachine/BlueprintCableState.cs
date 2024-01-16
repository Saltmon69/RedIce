using UnityEngine;
using Object = UnityEngine.Object;

public class BlueprintCableState : BlueprintBaseState
{
    private GameObject _cableStock;
    private Object _cable;

    private GameObject _thisCable;
    private CableLaserBehaviour _cableLaserBehaviour;

    private MachineUIDisplay _machineUIDisplay;
    private GameObject _machineCable;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(6).gameObject.SetActive(true);
        
        _cableStock = GameObject.Find("CableStock");

        _cable = Resources.Load("Cables/Cable", typeof(GameObject));

        _thisCable = GameObject.Instantiate((GameObject)_cable, _cableStock.transform);

        _cableLaserBehaviour = _thisCable.GetComponent<CableLaserBehaviour>();
        _cableLaserBehaviour.isSetup = false;
    }
    
    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        //bouton retour au mode de sélection des modes du blueprint
        if(Input.GetKeyDown(KeyCode.C))
        {
            blueprint.SwitchState(blueprint.startState);
            GameObject.Destroy(_thisCable);
        }
    }
    
    public override void RayState(BlueprintStateMachineManager blueprint, RaycastHit hitData, RaycastHit oldHitData, bool hadHit)
    {
        if(hitData.transform.gameObject.layer == 6 && hitData.transform.CompareTag("Output"))
        {
            //si l'on sélectionne une sortie de machine, on assigne au cable qu il est attaché a cette sortie et a quel machine elle appartient
            //avance au mode de sélection de l'entrée de la seconde machine pour le cablage
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                _machineUIDisplay = hitData.transform.parent.gameObject.GetComponent<MachineUIDisplay>();

                if(_machineUIDisplay.thisMachineOutputList.Contains(hitData.transform.gameObject))
                {
                    GameObject.Destroy(_thisCable);
                    
                    _machineCable = _machineUIDisplay.thisMachineOutputCableList[_machineUIDisplay.thisMachineOutputList.IndexOf(hitData.transform.gameObject)].gameObject;
                    
                    //change la position du cable dans la hiérarchie à la derniere position pour etre retrouver facilement dans le prochain état 
                    _machineCable.transform.SetSiblingIndex(_machineCable.transform.parent.childCount - 1);

                    blueprint.SwitchState(blueprint.checkpointState);
                }
                else
                {
                    _cableLaserBehaviour.outputMachine = hitData.transform.parent.gameObject;
                    _cableLaserBehaviour.outputGameObject = hitData.transform.gameObject;
                    
                    _machineUIDisplay.thisMachineOutputList.Add(_cableLaserBehaviour.outputGameObject);
                    _machineUIDisplay.thisMachineOutputCableList.Add(_thisCable.GetComponent<CableLaserBehaviour>());
                    
                    blueprint.SwitchState(blueprint.linkMachinesState);
                }
            }
                
            //feedback pour voir quel sortie on vise / regarde
            if(hitData.transform.gameObject != oldHitData.transform.gameObject || !hadHit)
            {
                hitData.transform.GetComponent<HighlightComponent>().Outline();
            }
        }
    }
        
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(6).gameObject.SetActive(false);
    }
}
