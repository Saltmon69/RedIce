using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HighlightComponent : MonoBehaviour
{
    public List<MeshRenderer> _meshRenderersList;
    public List<Material> _baseMaterialsList;

    public Material highlightMaterial;
    public Material outlineMaterial;
    public Material blueprintMaterial;

    //cette fonction attribu tous les MeshRenderer qui concerne cet object
    private void Awake()
    {
        _meshRenderersList.Clear();
        _baseMaterialsList.Clear();
        
        if(this.gameObject.transform.childCount > 0)
        {
            _meshRenderersList.AddRange(this.gameObject.transform.GetComponentsInChildren<MeshRenderer>());

            for(var i = 0; i < _meshRenderersList.Count; i++)
            {
                _baseMaterialsList.Add(_meshRenderersList[i].material);
            }
        }
    }

    //toutes les fonction si dessous sont un simple changement de matériaux qui permet aux scripts externe de pouvoir facilement changer un ou des matériaux
    public void BaseMaterial()
    {
        for(var i = 1; i <= _meshRenderersList.Count; i++)
        {
            if(_meshRenderersList[^i].material.name == _meshRenderersList[0].material.name)
            {
                _meshRenderersList[^i].material = _baseMaterialsList[^i];
            }
        }
    }

    public void Highlight()
    {
        Debug.Log("we in");
        for(var i = 0; i < _meshRenderersList.Count; i++)
        {
            _meshRenderersList[i].material = highlightMaterial;
        }
    }

    public void Outline()
    {
        for(var i = 0; i < _meshRenderersList.Count; i++)
        {
            _meshRenderersList[i].material = outlineMaterial;
        }
    }

    public void Blueprint()
    {
        for(var i = 0; i < _meshRenderersList.Count; i++)
        {
            _meshRenderersList[i].material = blueprintMaterial;
        }
    }
}
