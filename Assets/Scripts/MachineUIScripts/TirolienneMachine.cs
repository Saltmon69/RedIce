using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TirolienneMachine : MonoBehaviour
{
    private GameObject _player;
    private LineRenderer _lineRenderer;
    public bool isPlaced;
    public float poleDetectionRadius;
    public float speed;
    public float distanceToStop;
    private bool isInCinematic;
    public float cableAltitude;
    public int steps;
    public int currentSteps;

    private Vector3 _endPosition;

    private List<GameObject> _poleList;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").gameObject;
        _lineRenderer = this.gameObject.transform.GetChild(0).GetComponent<LineRenderer>();
        
        _poleList = new List<GameObject>();
        for(var i = 1; i < this.gameObject.transform.childCount; i++)
        {
            _poleList.Add(this.gameObject.transform.GetChild(i).gameObject);
        }
        
        isPlaced = false;
    }

    public void Update()
    {
        if(!isPlaced)
        {
            _poleList[0].transform.position = _player.transform.position + _player.transform.forward + _player.transform.right * 2 + Vector3.down * 0.65f;
            _lineRenderer.SetPosition(0, _poleList[0].transform.position + Vector3.up * cableAltitude);
            _lineRenderer.SetPosition(1, _poleList[1].transform.position + Vector3.up * cableAltitude);
        }
    }

    public void UseTirolienne(GameObject pole)
    {
        if (Vector2.Distance(pole.transform.position, _player.transform.position) <= poleDetectionRadius && !isInCinematic)
        {
            currentSteps = steps;
            StartCoroutine(PlayerDisplacement(_poleList[^(_poleList.IndexOf(pole)+1)].transform));
        }
    }

    private IEnumerator PlayerDisplacement(Transform endPole)
    {
        isInCinematic = true;
        _player.GetComponent<PlayerMovement>().enabled = false;
        _endPosition = endPole.position + Vector3.up * (cableAltitude - 2);
        
        while(true)
        {
            if(currentSteps > 0) _player.transform.position = Vector3.MoveTowards(_player.transform.position, _player.transform.position + Vector3.up * cableAltitude, 0.5f);
            currentSteps--;
            _player.transform.position = Vector3.MoveTowards(_player.transform.position, _endPosition, (speed - currentSteps * 0.01f) / 10);

            Debug.Log(Vector3.Distance(_player.transform.position, _endPosition));
            
            if(Vector3.Distance(_player.transform.position, _endPosition) <= distanceToStop)
            {
                _player.GetComponent<PlayerMovement>().enabled = true;
                isInCinematic = false;
                StopAllCoroutines();
                yield return null;
            }
            
            yield return new WaitForSeconds(0.01f);
        }
    }
}