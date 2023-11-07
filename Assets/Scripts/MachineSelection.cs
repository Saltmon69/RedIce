using System;
using UnityEngine;

public class MachineSelection : MonoBehaviour
{
        private Camera _mainCamera;
        private Ray _ray;
        private RaycastHit _hitData;
        private RaycastHit _oldHitData;
        
        public float distance;
        public LayerMask layerMask;
        
        private BlueprintMode _blueprintMode;
        private void Awake()
        {
                _mainCamera = Camera.main;
                _blueprintMode = this.gameObject.GetComponent<BlueprintMode>();
        }
        private void Update()
        {
                _blueprintMode.nextModesRequirements[0] = false;
                
                _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(_ray, out _hitData, distance, layerMask))
                {
                        try
                        {
                                if (_hitData.transform.gameObject != _oldHitData.transform.gameObject)
                                {
                                        _hitData.transform.SetSiblingIndex(_hitData.transform.parent.childCount - 1);
                                        _hitData.transform.GetComponent<HighlightComponent>().Outline();
                                        _oldHitData.transform.GetComponent<HighlightComponent>().BaseMaterial();
                                        _oldHitData = _hitData;
                                }
                                
                                _blueprintMode.nextModesRequirements[0] = true;
                        }
                        catch (NullReferenceException)
                        {
                                _oldHitData = _hitData;
                        }
                }
        }
}
