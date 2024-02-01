using UnityEngine;

public class OpenMachineUI : MonoBehaviour
{
    public Camera mainCamera;
    private Ray _ray;
    public float distance;
    private LayerMask _layerMask;
    private RaycastHit _hitData;
    private GameObject _gameObjetHit;
    
    public MachineUIDisplay thisDisplay;
    public GameObject thisPlayerInventory;

    public ComputerUIDisplay thisComputerDisplay;
    public ChestUIDisplay thisChestUIDisplay;
    
    public PlayerModeSelect modeSelection;

    private bool _hasHitMachine;
    private bool _isUIUp;

    private PlayerMenuing _playerMenuing;

    public void Awake()
    {
        _layerMask = LayerMask.GetMask("Machine");

        mainCamera = Camera.main;

        _playerMenuing = GameObject.FindWithTag("Player").GetComponent<PlayerMenuing>();
    }

    public void Update()
    {
        _ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        //si une machine est séléctionné sa prend l'ui lié à cette machine et la crée
        if(Physics.Raycast(_ray, out _hitData, distance, _layerMask))
        {
            if(Input.GetKeyDown(KeyCode.Mouse0) && !_isUIUp)
            {
                _gameObjetHit = _hitData.transform.gameObject;
                _hasHitMachine = false;

                if(_gameObjetHit.transform.CompareTag("Input") || _gameObjetHit.transform.CompareTag("Output"))
                {
                    _gameObjetHit = _gameObjetHit.transform.parent.gameObject;
                }

                if(_gameObjetHit.transform.CompareTag("Untagged"))
                {
                    thisDisplay = _gameObjetHit.transform.GetComponent<MachineUIDisplay>();
                    thisDisplay.playerInventory = thisPlayerInventory;
                    thisDisplay.ActivateUIDisplay();
                    _hasHitMachine = true;
                }

                if(_gameObjetHit.transform.CompareTag("Computer"))
                {
                    thisComputerDisplay = _gameObjetHit.transform.GetComponent<ComputerUIDisplay>();
                    thisComputerDisplay.playerInventory = thisPlayerInventory;
                    thisComputerDisplay.ActivateUIDisplay();
                    _hasHitMachine = true;
                }
                
                if(_gameObjetHit.transform.CompareTag("Chest"))
                {
                    thisChestUIDisplay = _gameObjetHit.transform.GetComponent<ChestUIDisplay>();
                    thisChestUIDisplay.playerInventory = thisPlayerInventory;
                    thisChestUIDisplay.ActivateUIDisplay();
                    _hasHitMachine = true;
                }

                if(_gameObjetHit.transform.CompareTag("Ground"))
                {
                    _gameObjetHit.SetActive(false);
                    _gameObjetHit.transform.parent.GetChild(1).gameObject.SetActive(true);
                    
                    for(var i = 0; i < _gameObjetHit.GetComponent<MachineCost>().buildingMaterialList.Count; i++)
                    {
                        DestroyImmediate(_gameObjetHit.transform.parent.GetChild(_gameObjetHit.transform.parent.childCount - 1).gameObject);
                    }
                }


                if(_hasHitMachine)
                {
                    _isUIUp = true;
                    _playerMenuing.inMenu = true;
                    modeSelection.canPlayerSwitchMode = false;
                }
            }
            
            if(_hasHitMachine) Time.timeScale = 1;
        }

        //désactive l ui avec echape
        if(Input.GetKeyDown(KeyCode.Escape) && _isUIUp)
        {
            if(thisDisplay != null)
            {
                thisDisplay.DeactivateUIDisplay();
                thisDisplay = null;
            }

            if(thisComputerDisplay != null)
            {
                thisComputerDisplay.DeactivateUIDisplay();
                thisComputerDisplay = null;
            }

            if(thisChestUIDisplay != null)
            {
                thisChestUIDisplay.DeactivateUIDisplay();
                thisChestUIDisplay = null;
            }
        
            _isUIUp = false;

            modeSelection.canPlayerSwitchMode = true;
            _playerMenuing.OnQuitPressed();
        }
    }
}
