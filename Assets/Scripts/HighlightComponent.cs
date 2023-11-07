using System.Collections.Generic;
using UnityEngine;

public class HighlightComponent : MonoBehaviour
{
    public List<MeshRenderer> _meshRenderers;

    public List<Material> _baseMaterials;

    public Material highlightMaterial;
    public Material outlineMaterial;
    public Material blueprintMaterial;
    
    private void Awake()
    {
        _meshRenderers.Clear();
        _baseMaterials.Clear();
        
        if (this.gameObject.transform.childCount == 0)
        {
            _meshRenderers.Add(this.gameObject.GetComponent<MeshRenderer>());
            _baseMaterials.Add(_meshRenderers[0].material);
        }
        
        if (this.gameObject.transform.childCount > 0)
        {
            for (var i = 0; i < this.gameObject.transform.childCount; i++)
            {
                _meshRenderers.Add(this.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>());
            }
        
            for (var i = 0; i < _meshRenderers.Count; i++)
            {
                _baseMaterials.Add(_meshRenderers[i].material);
            }
        }
    }

    public void BaseMaterial()
    {
        for (var i = 0; i < _meshRenderers.Count; i++)
        {
            _meshRenderers[i].material = _baseMaterials[i];
        }
        
        for (var i = 1; i < this.gameObject.transform.childCount; i++)
        {
            this.gameObject.transform.GetChild(i).GetComponent<Collider>().enabled = true;
        }
    }

    public void Highlight()
    {
        for (var i = 0; i < _meshRenderers.Count; i++)
        {
            _meshRenderers[i].material = highlightMaterial;
        }
    }

    public void Outline()
    {
        for (var i = 0; i < _meshRenderers.Count; i++)
        {
            _meshRenderers[i].material = outlineMaterial;
        }
    }

    public void Blueprint()
    {
        for (var i = 0; i < _meshRenderers.Count; i++)
        {
            _meshRenderers[i].material = blueprintMaterial;
        }
        
        for (var i = 1; i < this.gameObject.transform.childCount; i++)
        {
            this.gameObject.transform.GetChild(i).GetComponent<Collider>().enabled = false;
        }
    }
}
