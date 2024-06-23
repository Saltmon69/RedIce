using System;
using UnityEngine;

public class MiningMode : MonoBehaviour
{
    private PlayerInteraction _playerInteraction;
    private Ray _ray;
    private Camera mainCamera;
    private RaycastHit _hitData;
    private LayerMask _layerMask;

    public GameObject textUI;
    
    public void Awake()
    {
        _playerInteraction = GameObject.FindWithTag("Player").GetComponent<PlayerInteraction>();
        
        _layerMask = LayerMask.GetMask("Default");

        mainCamera = Camera.main;
    }

    public void Update()
    {
        textUI.SetActive(false);
        _ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(_ray, out _hitData, _playerInteraction.interactionRange, _layerMask))
        {
            if(_hitData.transform.CompareTag("Minerai") || _hitData.transform.CompareTag("MineraiCrit")) textUI.SetActive(true);
        }
    }

    public void OnEnable()
    {
        _playerInteraction.isMiningModeActive = true;
    }

    public void OnDisable()
    {
        _playerInteraction.isMiningModeActive = false;
    }
}
