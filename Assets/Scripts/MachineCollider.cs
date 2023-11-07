using UnityEngine;

public class MachineCollider : MonoBehaviour
{
    public bool isActive;
    public bool canBePlaced;
    
    private HighlightComponent _highlightComponent;
    
    private void Awake()
    {
        _highlightComponent = this.gameObject.GetComponent<HighlightComponent>();
    }

    private void OnEnable()
    {
        canBePlaced = true;
    }

    private void OnCollisionStay()
    {
        if (isActive)
        {
            canBePlaced = false;
            _highlightComponent.Highlight();
        }
    }

    private void OnCollisionExit()
    {      
        if (isActive)
        {
            canBePlaced = true;
            _highlightComponent.Blueprint();
        }
    }
    
}