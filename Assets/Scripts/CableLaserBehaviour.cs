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
    
    public bool isLinked;
    public float offset;
    public List<Vector3> checkpoints;

    private LineRenderer _lineRenderer;
    private RaycastHit _hitData;

    private void Awake()
    {
        _lineRenderer = this.gameObject.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        CheckpointsUpdate();
        CheckpointCheck();
        InputOutputUpdater();
    }

    //Cette fonction crée une liste contenant les différents points de passages assigné grâce au cablage dans le mode blueprint
    //Cela permettra de cree un cable qui se branche dynamiquement a une sortie et une entrée de deux machines et se lie au point de passage posé
    private void CheckpointsUpdate()
    {
        checkpoints.Clear();

        try
        {
            checkpoints.Add(outputGameObject.transform.position);
            //se lie à x distance du devant du point de sortie de la machine pour avoir un branchement plus logique et sera utile plus tard
            checkpoints.Add(outputGameObject.transform.TransformPoint(0, 0, offset / outputGameObject.transform.lossyScale.z));
        }catch (UnassignedReferenceException){}

        for (var i = 0; i < this.gameObject.transform.childCount; i++)
        {
            checkpoints.Add(this.gameObject.transform.GetChild(i).position);
        }

        try
        {
            //se lie à x distance du devant du point d'entrée de la machine pour avoir un branchement plus logique et sera utile plus tard
            checkpoints.Add(inputGameObject.transform.TransformPoint(0, 0, offset / inputGameObject.transform.lossyScale.z));
            checkpoints.Add(inputGameObject.transform.position);
        }catch (UnassignedReferenceException){}
        
        _lineRenderer.positionCount = checkpoints.Count;
    }

    //Cette fonction permet d'afficher le cable et de regarder si le cable heurte un object en chemin, dans ce cas il se coupe
    private void CheckpointCheck()
    {
        isLinked = true;
        
        for (var i = 0; i < checkpoints.Count; i++)
        {
            //assigne les différentes positions définie dans notre liste de point de passage à notre Line renderer 
            _lineRenderer.SetPosition(i, checkpoints[i]);

            //on ne prend pas en compte l'entrée et la sortie car elle touche forcément la machine concerné
            //deplus, notre point de passage situé juste devans la sortie/entrée vérifie dans tous les cas le bon comportement du cable
            if (i == 0) continue;
            if (i == checkpoints.Count - 1) continue;

            //on va donc itéré pour chaque point de passage leur position, direction et distance 
            //si un object est touché, on modifier le Line Renderer en conséquence (le cable est donc pas lié entre les deux machines)
            if (Physics.Raycast(checkpoints[i], checkpoints[i + 1] - checkpoints[i], out _hitData, 
                Vector3.Distance(checkpoints[i], checkpoints[i + 1])) && i < checkpoints.Count - 2)
            {
                _lineRenderer.SetPosition(i + 1 , _hitData.point);
                _lineRenderer.positionCount = i + 2;
                isLinked = false;
                i = checkpoints.Count;
            }
        }
    }

    //permet d'activer et de désactiver les différents colliders des entrées et sorties
    //ceci sert a ne pas pouvoir associer un cable sur une entrée/sortie qui est déja utilisé par un cable
    private void InputOutputUpdater()
    {
        if (outputGameObject != _oldOutputGameObject)
        {
            if (_oldOutputGameObject == null) _oldOutputGameObject = outputGameObject;
            
            _oldOutputGameObject.GetComponent<BoxCollider>().enabled = true;
            outputGameObject.GetComponent<BoxCollider>().enabled = false;
            _oldOutputGameObject = outputGameObject;
        }
        
        if (inputGameObject != _oldInputGameObject)
        {
            if (_oldInputGameObject == null) _oldInputGameObject = inputGameObject;
            
            _oldInputGameObject.GetComponent<BoxCollider>().enabled = true;
            inputGameObject.GetComponent<BoxCollider>().enabled = false;
            _oldInputGameObject = inputGameObject;
        }  
    }
}
