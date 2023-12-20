using System;
using System.Collections.Generic;
using UnityEngine;

public class CableLaserBehaviour : MonoBehaviour
{
    public GameObject outputMachine;
    public GameObject outputGameObject;
    private GameObject _oldOutputGameObject;

    public GameObject inputMachine;
    public GameObject inputGameObject;
    private GameObject _oldInputGameObject;

    private GameObject _linkCable;
    private GameObject _blueprintCable;
    
    public bool isLinked;
    public bool wasLinked;
    public bool isSetup;

    public float offset;
    public List<Vector3> checkpointList;
    public int checkpointsCount;

    public List<Vector3> blueprintCheckpointList;
    
    private LineRenderer _lineRenderer;
    private LineRenderer _blueprintLineRenderer;
    private RaycastHit _hitData;

    private void Awake()
    {
        _linkCable = this.gameObject.transform.GetChild(0).gameObject;
        _blueprintCable = this.gameObject.transform.GetChild(1).gameObject;
        
        _lineRenderer = _linkCable.GetComponent<LineRenderer>();
        _blueprintLineRenderer = _blueprintCable.GetComponent<LineRenderer>();
        isSetup = false;
        wasLinked = false;
        checkpointsCount = 0;
    }

    private void Update()
    {
        if (!isSetup)
        {
            CheckpointsUpdate();
            
            if(checkpointList.Count >= 4)
            {
                if(_blueprintCable.transform.childCount > 0) BlueprintCheckpointUpdate();
                checkpointsCount = checkpointList.Count;
                wasLinked = isLinked;
            }

            if(checkpointList.Count > 0)CheckpointPosition();
            if(checkpointList.Count >= 4) BlueprintCheckpointPosition();
        }

        CheckpointCheck();
    }

    //Cette fonction crée une liste contenant les différents points de passages assigné grâce au cablage dans le mode blueprint
    //Cela permettra de cree un cable qui se branche dynamiquement a une sortie et une entrée de deux machines et se lie au point de passage posé
    private void CheckpointsUpdate()
    {
        checkpointList.Clear();

        try
        {
            checkpointList.Add(outputGameObject.transform.position);
            //se lie à x distance du devant du point de sortie de la machine pour avoir un branchement plus logique et sera utile plus tard
            checkpointList.Add(outputGameObject.transform.TransformPoint(0, 0, offset / outputGameObject.transform.lossyScale.z));
        }catch (UnassignedReferenceException){}

        for (var i = 0; i < _linkCable.transform.childCount; i++)
        {
            checkpointList.Add(_linkCable.transform.GetChild(i).position);
        }

        try
        {
            //se lie à x distance du devant du point d'entrée de la machine pour avoir un branchement plus logique et sera utile plus tard
            checkpointList.Add(inputGameObject.transform.TransformPoint(0, 0, offset / inputGameObject.transform.lossyScale.z));
            checkpointList.Add(inputGameObject.transform.position);
        }catch(UnassignedReferenceException){}catch(NullReferenceException){}
        
        _lineRenderer.positionCount = checkpointList.Count;
    }

    private void BlueprintCheckpointUpdate()
    {
        blueprintCheckpointList.Clear();

        blueprintCheckpointList.Add(checkpointList[^3]);
        blueprintCheckpointList.Add(_blueprintCable.transform.GetChild(0).position);
        blueprintCheckpointList.Add(checkpointList[^2]);

        _blueprintLineRenderer.positionCount = blueprintCheckpointList.Count;
    }

    //met sur le line renderer la position des points de passage affin d'afficher le cable
    private void CheckpointPosition()
    {
        for (var i = 0; i < _lineRenderer.positionCount; i++)
        {
            _lineRenderer.SetPosition(i, checkpointList[i]);
        }
    }

    //permet d'afficher le resultat de la nouvelle direction du cable avec le placement du point de passage que l'on tente de placer
    private void BlueprintCheckpointPosition()
    {
        for (var i = 0; i < _blueprintLineRenderer.positionCount; i++)
        {
            _blueprintLineRenderer.SetPosition(i, blueprintCheckpointList[i]);
        }
    }
    
    /// <summary>
    /// Cette fonction permet d'afficher le cable et de regarder si le cable heurte un object en chemin, dans ce cas il se coupe
    /// </summary>
    private void CheckpointCheck() 
    {
        isLinked = true;
        _lineRenderer.positionCount = checkpointList.Count;
        
        for (var i = 0; i < checkpointList.Count; i++)
        {
            //on ne prend pas en compte l'entrée et la sortie car elle touche forcément la machine concerné
            //deplus, notre point de passage situé juste devans la sortie/entrée vérifie dans tous les cas le bon comportement du cable
            if (i == 0) continue;
            if (i == checkpointList.Count - 1) continue;

            //on va donc itéré pour chaque point de passage leur position, direction et distance 
            //si un object est touché, on modifier le Line Renderer en conséquence (le cable est donc pas lié entre les deux machines)
            if (Physics.Raycast(checkpointList[i], checkpointList[i + 1] - checkpointList[i], out _hitData, 
                Vector3.Distance(checkpointList[i], checkpointList[i + 1])) && i < checkpointList.Count - 2)
            {
                if(_hitData.transform.CompareTag("Player")) continue;
                _lineRenderer.positionCount = i + 2;
                isLinked = false;
                _lineRenderer.SetPosition(i + 1, _hitData.point);
                i = checkpointList.Count;
            }
        }
        
        //si le cable a été couper mais qu'il ne l'est plus ou du moins au meme endroit, réactualise le line renderer
        if (checkpointsCount != _lineRenderer.positionCount && isSetup)
        {
            checkpointsCount = _lineRenderer.positionCount;
            CheckpointPosition();
        }
    }
}
