using System.Collections;
using Cinemachine;
using UnityEngine;

public class TirolienneMachine : MonoBehaviour
{
    private GameObject _player;
    private LineRenderer _lineRenderer;
    public LayerMask layerMask;
    public bool isPlaced;
    public float poleDetectionRadius;
    public float speed;
    public float distanceToStop;
    private bool isInCinematic;
    public float cableAltitude;

    public Collider[] firstPoleColliderHit;
    public Collider[] secondPoleColliderHit;
    
    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").gameObject;
        _lineRenderer = this.gameObject.transform.GetChild(0).GetComponent<LineRenderer>();
        isPlaced = false;
    }

    public void Update()
    {
        if(!isPlaced)
        {
            this.gameObject.transform.GetChild(1).position = _player.transform.position + _player.transform.forward + _player.transform.right * 2;
            _lineRenderer.SetPosition(0, this.gameObject.transform.GetChild(1).position + Vector3.up * cableAltitude);
            _lineRenderer.SetPosition(1, this.gameObject.transform.GetChild(2).position + Vector3.up * cableAltitude);
        }
        else
        {
            firstPoleColliderHit = Physics.OverlapSphere(this.gameObject.transform.GetChild(1).position, poleDetectionRadius, layerMask);
            secondPoleColliderHit = Physics.OverlapSphere(this.gameObject.transform.GetChild(2).position, poleDetectionRadius, layerMask);
            
            if (Input.GetKeyDown(KeyCode.E) && !isInCinematic)
            {
                if(firstPoleColliderHit.Length > 0) UseTirolienne(firstPoleColliderHit ,this.gameObject.transform.GetChild(1), this.gameObject.transform.GetChild(2));
                if(secondPoleColliderHit.Length > 0) UseTirolienne(secondPoleColliderHit ,this.gameObject.transform.GetChild(2), this.gameObject.transform.GetChild(1));
            }
        }
    }

    private void UseTirolienne(Collider[] colliders, Transform startPole, Transform endPole)
    {
        for (var i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].CompareTag("Player"))
            {
                _player.transform.position = startPole.transform.position + Vector3.left * 2 + Vector3.up;
                StartCoroutine(PlayerDisplacement(endPole));
            }
        }
    }

    private IEnumerator PlayerDisplacement(Transform endPole)
    {
        isInCinematic = true;
        _player.GetComponent<InputManager>().enabled = false;

        while(true)
        {
            _player.transform.position = Vector3.MoveTowards(_player.transform.position, endPole.position + Vector3.left * 2, speed / 10);

            Debug.Log(Vector3.Distance(_player.transform.position, endPole.position));
            
            if(Vector3.Distance(_player.transform.position, endPole.position) <= distanceToStop)
            {
                Debug.Log("we're in");
                isInCinematic = false;
                _player.GetComponent<InputManager>().enabled = true;
                StopAllCoroutines();
                yield return null;
            }
            
            yield return new WaitForSeconds(0.01f);
        }
    }
}