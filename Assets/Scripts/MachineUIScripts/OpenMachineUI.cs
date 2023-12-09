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
        if (Physics.Raycast(_ray, out _hitData, Mathf.Infinity , _layerMask))
        {
            if(Input.GetKeyDown(KeyCode.Mouse0) && _hitData.transform.CompareTag("Untagged") && !_isUIUp)
            {
                thisDisplay = _hitData.transform.GetComponent<MachineUIDisplay>();
                thisDisplay.playerInventory = thisPlayerInventory;
                thisDisplay.ActivateUIDisplay();
                thisPlayerInventory.transform.parent.gameObject.SetActive(false);
                _isUIUp = true;
                modeSelection.canPlayerSwitchMode = false;

                playerInput.Deactivate();
                
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        //désactive l ui avec echape
        if(Input.GetKeyUp(KeyCode.Escape) && thisDisplay != null)
        {
            thisDisplay.DeactivateUIDisplay();
            thisPlayerInventory.transform.parent.gameObject.SetActive(true);
            thisDisplay = null;
            _isUIUp = false;
            modeSelection.canPlayerSwitchMode = true;
            
            playerInput.Activate();
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
