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
    public ChestUIDisplay thisChestUIDisplay;
    
    public PlayerModeSelect modeSelection;

    private bool _hasHitMachine;
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
                _hasHitMachine = false;
                
                if(_hitData.transform.CompareTag("Untagged"))
                {
                    thisDisplay = _hitData.transform.GetComponent<MachineUIDisplay>();
                    thisDisplay.playerInventory = thisPlayerInventory;
                    thisDisplay.ActivateUIDisplay();
                    _hasHitMachine = true;
                }

                if(_hitData.transform.CompareTag("Computer"))
                {
                    thisComputerDisplay = _hitData.transform.GetComponent<ComputerUIDisplay>();
                    thisComputerDisplay.playerInventory = thisPlayerInventory;
                    thisComputerDisplay.ActivateUIDisplay();
                    _hasHitMachine = true;
                }
                
                if(_hitData.transform.CompareTag("Chest"))
                {
                    thisChestUIDisplay = _hitData.transform.GetComponent<ChestUIDisplay>();
                    thisChestUIDisplay.playerInventory = thisPlayerInventory;
                    thisChestUIDisplay.ActivateUIDisplay();
                    _hasHitMachine = true;
                }

                if (_hasHitMachine)
                {
                    thisPlayerInventory.transform.parent.gameObject.SetActive(false);
                    _isUIUp = true;
                    modeSelection.canPlayerSwitchMode = false;

                    playerInput.Deactivate();
                
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true; 
                }
            }
        }

        //désactive l ui avec echape
        if(Input.GetKeyUp(KeyCode.Escape) && (thisDisplay != null || thisComputerDisplay != null || thisChestUIDisplay != null))
        {
            if(thisDisplay != null) thisDisplay.DeactivateUIDisplay();
            if(thisComputerDisplay != null) thisComputerDisplay.DeactivateUIDisplay();
            if(thisChestUIDisplay != null) thisChestUIDisplay.DeactivateUIDisplay();

            thisPlayerInventory.transform.parent.gameObject.SetActive(true);

            thisChestUIDisplay = null;
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
