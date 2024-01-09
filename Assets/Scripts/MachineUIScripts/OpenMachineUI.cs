using UnityEngine;

public class OpenMachineUI : MonoBehaviour
{
    public Camera mainCamera;
    private Ray _ray;
    public float distance;
    private LayerMask _layerMask;
    private RaycastHit _hitData;
    
    public MachineUIDisplay thisDisplay;
    public GameObject thisPlayerInventory;

    public ComputerUIDisplay thisComputerDisplay;
    
    public PlayerModeSelect modeSelection;

    private bool _isUIUp;

    public DeactivatePlayerInput playerInput;

    public void Awake()
    {
        _layerMask = LayerMask.GetMask("Machine");

        mainCamera = Camera.main;
    }

    void Update()
    {
        _ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        //si une machine est séléctionné sa prend l'ui lié à cette machine et la crée
        if(Physics.Raycast(_ray, out _hitData, Mathf.Infinity , _layerMask))
        {
            if(Input.GetKeyDown(KeyCode.Mouse0) && !_isUIUp)
            {
                if(_hitData.transform.CompareTag("Untagged"))
                {
                    thisDisplay = _hitData.transform.GetComponent<MachineUIDisplay>();
                    thisDisplay.playerInventory = thisPlayerInventory;
                    thisDisplay.ActivateUIDisplay();
                }

                if (_hitData.transform.CompareTag("Computer"))
                {
                    thisComputerDisplay = _hitData.transform.GetComponent<ComputerUIDisplay>();
                    thisComputerDisplay.playerInventory = thisPlayerInventory;
                    thisComputerDisplay.ActivateUIDisplay();
                }
                
                if (_hitData.transform.CompareTag("External"))
                {
                    
                }

                thisPlayerInventory.transform.parent.gameObject.SetActive(false);
                _isUIUp = true;
                modeSelection.canPlayerSwitchMode = false;

                playerInput.Deactivate();
                
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        //désactive l ui avec echape
        if(Input.GetKeyUp(KeyCode.Escape) && (thisDisplay != null || thisComputerDisplay != null))
        {
            if(thisDisplay != null) thisDisplay.DeactivateUIDisplay();
            if(thisComputerDisplay != null) thisComputerDisplay.DeactivateUIDisplay();

            thisPlayerInventory.transform.parent.gameObject.SetActive(true);
            
            thisComputerDisplay = null;
            thisDisplay = null;
            _isUIUp = false;
            modeSelection.canPlayerSwitchMode = true;
            
            playerInput.Activate();
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
