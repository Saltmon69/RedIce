using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineCollider : MonoBehaviour
{
    //se script mis sur chaque object possible d'être placé par le biais du blueprint afin de facilement voir si l'objet es plaçable ou non par des scripts externe
    public bool canBePlaced;
    public bool isActive;

    private float _baseWaitingTime;
    private float _waitingTime;
    
    private HighlightComponent _highlightComponent;
    private Collider _thisCollider;
    public List<Collider> _childColliderList;
    public List<GameObject> _collisionList;

    private void Awake()
    {
        _collisionList = new List<GameObject>();
        _childColliderList = new List<Collider>();
        
        _highlightComponent = this.gameObject.GetComponent<HighlightComponent>();
        _thisCollider = this.gameObject.GetComponent<Collider>();

        for(var i = 1; i < this.transform.childCount; i++)
        {
            try
            {
                _childColliderList.Add(this.transform.GetChild(i).GetComponent<Collider>());
                Physics.IgnoreCollision(_thisCollider, _childColliderList[i - 1]);
            }catch(MissingComponentException){}
        }

        _baseWaitingTime = 0.05f;
    }

    private void OnEnable()
    {
        canBePlaced = false;
        isActive = true;
        _highlightComponent.Highlight();
        _waitingTime = _baseWaitingTime;
        this.gameObject.GetComponent<Collider>().enabled = false;
        StartCoroutine(TimeDelayCount());
    }

    private IEnumerator TimeDelayCount()
    {
        while(_waitingTime > 0)
        {
            _waitingTime -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        this.gameObject.GetComponent<Collider>().enabled = true;
        _highlightComponent.Blueprint();
        canBePlaced = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isActive && _waitingTime <= 0)
        {
            canBePlaced = false;
            _collisionList.Add(collision.gameObject);
            _highlightComponent.Highlight();
            IsTrigger(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(isActive && _waitingTime <= 0)
        {
            _collisionList.Remove(other.gameObject);

            if(_collisionList.Count < 1)
            {
                canBePlaced = true;
                _highlightComponent.Blueprint();
                IsTrigger(false);
            } 
        }
    }

    public void IsTrigger(bool enable)
    {
        if(isActive && _waitingTime <= 0)
        {
            _thisCollider.isTrigger = enable;
            
            for(var i = 0; i < _childColliderList.Count; i++)
            {
                _childColliderList[i].isTrigger = enable;
            }
        }
    }

    public void OnDisable()
    {
        isActive = false;
        _highlightComponent.BaseMaterial();
    }
}