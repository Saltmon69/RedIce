using System;
using UnityEngine;

public class MachineCollider : MonoBehaviour
{
    public bool canBePlaced;
    private GameObject _hologram;
    private GameObject _machine;

    private MeshRenderer _hologramMeshRenderer;
    
    public Material canBePlacedMaterial;
    public Material cannotBePlacedMaterial;
    
    private void Awake()
    {
        _hologram = this.gameObject.transform.GetChild(0).gameObject;
        _machine = this.gameObject.transform.GetChild(1).gameObject;
        _hologramMeshRenderer = _hologram.GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        canBePlaced = true;

    }

    public void ActivateHologram()
    {
        _hologram.SetActive(true);
        _machine.SetActive(false);
    }

    public void DeactivateHologram()
    {
        _hologram.SetActive(false);
        _machine.SetActive(true);
    }
    
    private void OnCollisionStay()
    {
        canBePlaced = false;
        _hologramMeshRenderer.material = cannotBePlacedMaterial;
    }

    private void OnCollisionExit()
    {
        canBePlaced = true;
        _hologramMeshRenderer.material = canBePlacedMaterial;
    }
    
}