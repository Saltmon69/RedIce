using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMachineUI : MonoBehaviour
{
    public Camera mainCamera;
    public Ray ray;
    public float distance;
    private LayerMask layerMask;
    private RaycastHit _hitData;
    public MachineUIDisplay thisDisplay;

    private bool isUIUp;

    public void Awake()
    {
        layerMask = LayerMask.GetMask("Machine");

        mainCamera = Camera.main;
    }

    void Update()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        //si une machine est sélectionné sa prend l ui liée a cette machine et la crée
        if (Physics.Raycast(ray, out _hitData, Mathf.Infinity , layerMask))
        {
            if(Input.GetKeyDown(KeyCode.Mouse0) && _hitData.transform.CompareTag("Untagged") && !isUIUp)
            {
                thisDisplay = _hitData.transform.GetComponent<MachineUIDisplay>();
                thisDisplay.ActivateUIDisplay();
                isUIUp = true;
            }
        }

        //désactive l ui avec echape
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            thisDisplay.DeactivateUIDisplay();
            isUIUp = false;
        }
    }
}
