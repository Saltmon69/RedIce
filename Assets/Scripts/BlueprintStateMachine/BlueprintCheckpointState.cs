using UnityEngine;

public class BlueprintCheckpointState : BlueprintBaseState
{
    private GameObject _cableStock;
    private GameObject _thisCable;
    private GameObject _linkCable;
    private GameObject _blueprintCable;
    
    private Object _checkpoint;

    private GameObject _thisCheckpoint;

    private MachineCollider _checkpointCollider;
    private CableLaserBehaviour _cableLaserBehaviour;
    private MachineUIDisplay _machineUIDisplay;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(8).gameObject.SetActive(true);
        
        _cableStock = GameObject.Find("CableStock");

        _thisCable = _cableStock.transform.GetChild(_cableStock.transform.childCount - 1).gameObject;
        _linkCable = _thisCable.transform.GetChild(0).gameObject;
        _blueprintCable = _thisCable.transform.GetChild(1).gameObject;
        
        _checkpoint = Resources.Load("Cables/Checkpoint", typeof(GameObject));

        _thisCheckpoint = GameObject.Instantiate((GameObject)_checkpoint, _cableLaserBehaviour.outputMachine.transform.position, Quaternion.identity, _linkCable.transform);
        _thisCheckpoint.layer = 2;
        
        _checkpointCollider = _thisCheckpoint.GetComponent<MachineCollider>();
        _checkpointCollider.isActive = true;

        _cableLaserBehaviour = _thisCable.transform.GetComponent<CableLaserBehaviour>();
        _cableLaserBehaviour.isSetup = false;

        _machineUIDisplay = _cableLaserBehaviour.inputMachine.GetComponent<MachineUIDisplay>();


        _blueprintCable.SetActive(true);
    }
    
    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        //supprime le dernier point de passage posé
        //si il n'y a eu aucun point de passage placé, on retourne au mode de sélection de l'entrée de la seconde machine pour le cablage
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(_linkCable.transform.childCount > 0)
            {
                GameObject.Destroy(_linkCable.transform.GetChild(_linkCable.transform.childCount - 1).gameObject);
            }
            else
            {
                GameObject.Destroy(_blueprintCable.transform.GetChild(_blueprintCable.transform.childCount - 1).gameObject);
                
                _machineUIDisplay.thisMachineInputList.Remove(_cableLaserBehaviour.inputGameObject);
                _machineUIDisplay.thisMachineInputCableList.Remove(_thisCable);
                _machineUIDisplay.thisMachineCableMachineInputList.Remove(_cableLaserBehaviour.outputMachine);
                
                _cableLaserBehaviour.inputMachine = null;
                _cableLaserBehaviour.inputGameObject = null;
                
                blueprint.SwitchState(blueprint.linkMachinesState);
            }
        }
    }
    
    public override void RayState(BlueprintStateMachineManager blueprint, RaycastHit hitData, RaycastHit oldHitData)
    {
        if (hitData.transform.gameObject.layer == 3)
        {
            _thisCheckpoint.transform.position = hitData.point + Vector3.up * 1;
        }
        
        //si le chemin emprunté par le cable est valide, alors le joueur confirme la connection entre les deux machines
        //retour à la sélection d'une sortie pour le cablage entre deux machines
        if (hitData.transform.CompareTag("BaseFloor"))
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && _cableLaserBehaviour.wasLinked)
            {
                GameObject.Destroy(_thisCheckpoint);
                _cableLaserBehaviour.isSetup = true;
                blueprint.SwitchState(blueprint.cableState);
            }

            //si le point de passage peut etre placé alors on crée un point de passage à cet endroit meme
            if (Input.GetKeyDown(KeyCode.Mouse0) && _checkpointCollider.canBePlaced)
            {
                GameObject.Instantiate((GameObject)_checkpoint, _thisCheckpoint.transform.position, Quaternion.identity, _thisCable.transform.GetChild(0));
            }
        }
    }
    
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(8).gameObject.SetActive(false);
        _blueprintCable.SetActive(false);
    }
}
