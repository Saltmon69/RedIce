using System.Collections.Generic;
using UnityEngine;

public class MachineCollider : MonoBehaviour
{
    //se script mis sur chaque object possible d'être placé par le biais du blueprint afin de facilement voir si l'objet es plaçable ou non par des scripts externe
    public bool isActive;
    public bool canBePlaced;
    
    private HighlightComponent _highlightComponent;

    public List<GameObject> _collisionList;

    private void Awake()
    {
        _collisionList = new List<GameObject>();
        
        _highlightComponent = this.gameObject.GetComponent<HighlightComponent>();

        for (var i = 0; i < this.transform.childCount; i++)
        {
            try
            {
                Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), this.transform.GetChild(i).GetComponent<Collider>());
            }catch(MissingComponentException){}
        }
    }

    private void OnEnable()
    {
        canBePlaced = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isActive)
        {            
            canBePlaced = false;
            _collisionList.Add(collision.gameObject);
        }
    }

    private void OnCollisionStay()
    {
        if (isActive)
        {
            _highlightComponent.Highlight();
        }
    }

    private void OnCollisionExit(Collision collision)
    {      
        if (isActive)
        {
            _collisionList.Remove(collision.gameObject);

            if (_collisionList.Count < 1)
            {
                canBePlaced = true;
                _highlightComponent.Blueprint();
            }
        }
    }
}