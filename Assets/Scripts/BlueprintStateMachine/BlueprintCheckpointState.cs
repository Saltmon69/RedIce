using UnityEngine;

public class BlueprintCheckpointState : BlueprintBaseState
{
    public GameObject cableStock;
    public GameObject thisCable;
    public Object checkpoint;
    
    private GameObject _thisCheckpoint;
    private RaycastHit _hitData;
    public LayerMask layerMask;

    private MachineCollider _checkpointCollider;
    private CableLaserBehaviour _cableLaser;

    public override void EnterState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(8).gameObject.SetActive(true);
        
        layerMask = LayerMask.GetMask("Ground");
        cableStock = GameObject.Find("CableStock");

        thisCable = cableStock.transform.GetChild(cableStock.transform.childCount - 1).gameObject;
        
        checkpoint = Resources.Load("Cables/Checkpoint", typeof(GameObject));

        _thisCheckpoint = GameObject.Instantiate((GameObject)checkpoint, thisCable.transform);
        _checkpointCollider = _thisCheckpoint.GetComponent<MachineCollider>();
        _checkpointCollider.isActive = true;

        _cableLaser = thisCable.transform.GetComponent<CableLaserBehaviour>();
    }
    
    public override void UpdateState(BlueprintStateMachineManager blueprint)
    {
        //si le chemin emprunté par le cable est valide, alors le joueur confirme la connection entre les deux machines
        //retour à la sélection d'une sortie pour le cablage entre deux machines
        if(Input.GetKeyDown(KeyCode.Mouse1) && _cableLaser.isLinked)
        {
            GameObject.Destroy(thisCable.transform.GetChild(thisCable.transform.childCount - 1).gameObject);
            blueprint.SwitchState(blueprint.cableState);
        }

        //si le point de passage peut etre placé alors on crée un point de passage à cet endroit meme
        if (Input.GetKeyDown(KeyCode.Mouse0) && _checkpointCollider.canBePlaced)
        {
            GameObject.Instantiate((GameObject)checkpoint,  _thisCheckpoint.transform.position, Quaternion.identity, thisCable.transform);
            _thisCheckpoint.transform.SetSiblingIndex(_thisCheckpoint.transform.parent.childCount - 1);
        }

        //supprime le dernier point de passage posé
        //si il n'y a eu aucun point de passage placé, on retourne au mode de sélection de l'entrée de la seconde machine pour le cablage
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(thisCable.transform.childCount > 1)
            {
                GameObject.Destroy(thisCable.transform.GetChild(thisCable.transform.childCount - 2).gameObject);
            }
            else
            {
                GameObject.Destroy(thisCable.transform.GetChild(thisCable.transform.childCount - 1).gameObject);
                blueprint.SwitchState(blueprint.linkMachinesState);
            }
        }
    }
    
    public override void RayState(BlueprintStateMachineManager blueprint, Ray ray, float distance)
    {
        if (Physics.Raycast(ray, out _hitData, distance, layerMask))
        {
            _thisCheckpoint.transform.position = _hitData.point + Vector3.up * 1;
        }
    }
    
    public override void ExitState(BlueprintStateMachineManager blueprint)
    {
        GameObject.Find("UIStateCanvas").transform.GetChild(8).gameObject.SetActive(false);
    }
}
