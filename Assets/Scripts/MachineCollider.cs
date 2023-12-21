using System;
using System.Collections.Generic;
using UnityEngine;

public class MachineCollider : MonoBehaviour
{
    //se script mis sur chaque object possible d'être placé par le biais du blueprint afin de facilement voir si l'objet es plaçable ou non par des scripts externe
    public bool canBePlaced;
    public bool isActive;
    
    private HighlightComponent _highlightComponent;
    private CapsuleCollider _thisCollider;
    public List<BoxCollider> _childColliderList;
    public List<GameObject> _collisionList;

    private void Awake()
    {
        _collisionList = new List<GameObject>();
        _childColliderList = new List<BoxCollider>();
        
        _highlightComponent = this.gameObject.GetComponent<HighlightComponent>();
        _thisCollider = this.gameObject.GetComponent<CapsuleCollider>();

        for (var i = 1; i < this.transform.childCount; i++)
        {
            try
            {
                _childColliderList.Add(this.transform.GetChild(i).GetComponent<BoxCollider>());
                Physics.IgnoreCollision(_thisCollider, _childColliderList[i - 1]);
            }catch(MissingComponentException){}
        }
    }

    private void OnEnable()
    {
        canBePlaced = true;
        isActive = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isActive)
        {
            canBePlaced = false;
            _collisionList.Add(collision.gameObject);
            _highlightComponent.Highlight();
            IsTrigger(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isActive)
        {
            _collisionList.Remove(other.gameObject);

            if (_collisionList.Count < 1)
            {
                canBePlaced = true;
                _highlightComponent.Blueprint();
                IsTrigger(false);
            } 
        }
    }

    public void IsTrigger(bool enable)
    {
        if (isActive)
        {
            _thisCollider.isTrigger = enable;
            
            for (var i = 0; i < _childColliderList.Count; i++)
            {
                _childColliderList[i].isTrigger = enable;
            }
        }
    }

    public void OnDisable()
    {
        isActive = false;
    }
}