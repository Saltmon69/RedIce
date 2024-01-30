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
    //exemple si un object simple doit voir son materiaux changer cela ne pose pas de probleme
    //par-contre si l'on doit changer un objet qui a plusieurs enfant et que l'on veut sa globaliter changer de matériaux cela pose un nouveaux problème
    private void Awake()
    {
        _meshRenderersList.Clear();
        _baseMaterialsList.Clear();

        if(this.gameObject.transform.childCount > 0)
        {
            for(var i = 0; i < this.gameObject.transform.childCount; i++)
            {
                _meshRenderersList.Add(this.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>());
            }
        
            for(var i = 0; i < _meshRenderersList.Count; i++)
            {
                _baseMaterialsList.Add(_meshRenderersList[i].material);
            }
        }
        else
        {
            _meshRenderersList.Add(this.gameObject.GetComponent<MeshRenderer>());
            _baseMaterialsList.Add(_meshRenderersList[0].material);
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
        for(var i = 0; i < _meshRenderersList.Count; i++)
        {
            if(_meshRenderersList[i].material.name == _baseMaterialsList[i].name)
            {
                _meshRenderersList[i].material = highlightMaterial;
            }
        }
    }

    public void Outline()
    {
        for(var i = 0; i < _meshRenderersList.Count; i++)
        {
            if(_meshRenderersList[i].material.name == _baseMaterialsList[i].name)
            {
                _meshRenderersList[i].material = outlineMaterial;
            }
        }
    }

    public void Blueprint()
    {
        for(var i = 0; i < _meshRenderersList.Count; i++)
        {
            if(_meshRenderersList[i].material.name == _baseMaterialsList[i].name)
            {
                _meshRenderersList[i].material = blueprintMaterial;
            }
        }
    }
}
