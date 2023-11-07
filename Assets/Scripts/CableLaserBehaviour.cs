using System.Collections.Generic;
using UnityEngine;

public class CableLaserBehaviour : MonoBehaviour
{
    public GameObject outputMachine;
    public GameObject outputGameObject;

    public GameObject inputMachine;
    public GameObject inputGameObject;
    
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
    }

    private void CheckpointsUpdate()
    {
        checkpoints.Clear();
        if (outputGameObject != null)
        {
            checkpoints.Add(outputGameObject.transform.position);
            checkpoints.Add(outputGameObject.transform.TransformPoint(0, 0, offset / outputGameObject.transform.lossyScale.z));
        }

        for (var i = 0; i < this.gameObject.transform.childCount; i++)
        {
            checkpoints.Add(this.gameObject.transform.GetChild(i).position);
        }

        if (inputGameObject != null)
        {
            checkpoints.Add(inputGameObject.transform.TransformPoint(0, 0, offset / inputGameObject.transform.lossyScale.z));
            checkpoints.Add(inputGameObject.transform.position);
        }
        
        _lineRenderer.positionCount = checkpoints.Count;
    }

    private void CheckpointCheck()
    {
        isLinked = true;
        
        for (var i = 0; i < checkpoints.Count; i++)
        {
            _lineRenderer.SetPosition(i, checkpoints[i]);
            if (i == 0) continue;
            if (i == checkpoints.Count - 1) continue;

            if (Physics.Raycast(checkpoints[i], checkpoints[i + 1] - checkpoints[i], out _hitData,
                    Vector3.Distance(checkpoints[i], checkpoints[i + 1])) && i < checkpoints.Count - 2)
            {
                _lineRenderer.SetPosition(i + 1 , _hitData.point);
                _lineRenderer.positionCount = i + 2;
                isLinked = false;
                i = checkpoints.Count;
                
                print(_hitData.transform.gameObject);
            }
        }
    }
}
