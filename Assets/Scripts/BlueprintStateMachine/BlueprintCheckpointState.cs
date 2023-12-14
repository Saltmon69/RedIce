using UnityEngine;

public class BlueprintCheckpointState : BlueprintBaseState
{
    private GameObject _cableStock;
    private GameObject _thisCable;
    private Object _checkpoint;

    private GameObject _thisCheckpoint;

    private MachineCollider _checkpointCollider;
    private CableLaserBehaviour _cableLaser;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(8).gameObject.SetActive(true);
        
        _cableStock = GameObject.Find("CableStock");

        _thisCable = _cableStock.transform.GetChild(_cableStock.transform.childCount - 1).gameObject;
        
        _checkpoint = Resources.Load("Cables/Checkpoint", typeof(GameObject));

        _thisCheckpoint = GameObject.Instantiate((GameObject)_checkpoint, _thisCable.transform);
        _thisCheckpoint.layer = 2;
        
        _checkpointCollider = _thisCheckpoint.GetComponent<MachineCollider>();
        _checkpointCollider.isActive = true;

        _cableLaser = _thisCable.transform.GetComponent<CableLaserBehaviour>();
    }
    
    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        //supprime le dernier point de passage posé
        //si il n'y a eu aucun point de passage placé, on retourne au mode de sélection de l'entrée de la seconde machine pour le cablage
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(_thisCable.transform.childCount > 1)
            {
                GameObject.Destroy(_thisCable.transform.GetChild(_thisCable.transform.childCount - 2).gameObject);
            }
            else
            {
                GameObject.Destroy(_thisCable.transform.GetChild(_thisCable.transform.childCount - 1).gameObject);
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
        if(Input.GetKeyDown(KeyCode.Mouse1) && _cableLaser.isLinked)
        {
            if (hitData.transform.CompareTag("BaseFloor"))
            {
                GameObject.Destroy(_thisCheckpoint);
                blueprint.SwitchState(blueprint.cableState);
            }
        }

        //si le point de passage peut etre placé alors on crée un point de passage à cet endroit meme
        if (Input.GetKeyDown(KeyCode.Mouse0) && _checkpointCollider.canBePlaced)
        {
            if (hitData.transform.CompareTag("BaseFloor"))
            {
                GameObject.Instantiate((GameObject)_checkpoint,  _thisCheckpoint.transform.position, Quaternion.identity, _thisCable.transform);
                _thisCheckpoint.transform.SetSiblingIndex(_thisCheckpoint.transform.parent.childCount - 1);
            }
        }
    }
    
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(8).gameObject.SetActive(false);
    }
}
