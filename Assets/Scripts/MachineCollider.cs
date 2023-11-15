using UnityEngine;

public class MachineCollider : MonoBehaviour
{
    //se script mis sur chaque object possible d'être placé par le biais du blueprint afin de facilement voir si l'objet es plaçable ou non par des scripts externe
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