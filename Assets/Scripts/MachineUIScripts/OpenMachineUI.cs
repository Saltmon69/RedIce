using System.Collections;
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
    public GeneratorUIDisplay thisGeneratorUIDisplay;
    
    public PlayerModeSelect modeSelection;
    
    private bool _isUIUp;

    private PlayerMenuing _playerMenuing;

    public bool hasInteractedWithComputer;
    public int numberOfMachineInteracted;

    public GameObject textUI;

    public Material dissolveMaterial;
    public float dissolveTimer;

    public void Awake()
    {
        _layerMask = LayerMask.GetMask("Machine");

        mainCamera = Camera.main;

        _playerMenuing = GameObject.FindWithTag("Player").GetComponent<PlayerMenuing>();
        
        dissolveMaterial.SetFloat("_Slider", 1);
    }

    private void HasHitMachine()
    {
        _isUIUp = true;
        _playerMenuing.InMenu();
        _playerMenuing.inMenu = true;
        Time.timeScale = 1;
        modeSelection.canPlayerSwitchMode = false;
    }
    
    public void Update()
    {
        textUI.SetActive(false);
        if(_isUIUp) return;
     
        _ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        //si une machine est séléctionné sa prend l'ui lié à cette machine et la crée
        if(Physics.Raycast(_ray, out _hitData, distance, _layerMask))
        {
            if(_hitData.transform.CompareTag("Input") || _hitData.transform.CompareTag("Output"))
            {
                _gameObjetHit = _hitData.transform.parent.gameObject;
            }
            else
            {
                _gameObjetHit = _hitData.transform.gameObject;
            }
            
            switch(_gameObjetHit.transform.tag)
            {
                case "Untagged":
                    textUI.SetActive(true);
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        thisDisplay = _gameObjetHit.transform.GetComponent<MachineUIDisplay>();
                        thisDisplay.playerInventory = thisPlayerInventory;
                        thisDisplay.ActivateUIDisplay();
                        numberOfMachineInteracted++;
                        HasHitMachine();
                    }
                    break;

                case "Computer":
                    textUI.SetActive(true);
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        thisComputerDisplay = _gameObjetHit.transform.GetComponent<ComputerUIDisplay>();
                        thisComputerDisplay.playerInventory = thisPlayerInventory;
                        thisComputerDisplay.ActivateUIDisplay();
                        hasInteractedWithComputer = true;
                        HasHitMachine();
                    }
                    break;

                case "Chest":
                    textUI.SetActive(true);
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        thisChestUIDisplay = _gameObjetHit.transform.GetComponent<ChestUIDisplay>();
                        thisChestUIDisplay.playerInventory = thisPlayerInventory;
                        thisChestUIDisplay.ActivateUIDisplay();
                        HasHitMachine();
                    }
                    break;

                case "Generator":
                    textUI.SetActive(true);
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        thisGeneratorUIDisplay = _gameObjetHit.transform.GetComponent<GeneratorUIDisplay>();
                        thisGeneratorUIDisplay.playerInventory = thisPlayerInventory;
                        thisGeneratorUIDisplay.ActivateUIDisplay();
                        HasHitMachine();
                    }
                    break;

                case "Ground":
                    textUI.SetActive(true);
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        if(_gameObjetHit.GetComponent<BaseRepair>().isBuildable)
                        {
                            _gameObjetHit.GetComponent<BaseRepair>().isBuilt = true;
                            _gameObjetHit.SetActive(false);
                            _gameObjetHit.transform.parent.GetChild(1).gameObject.SetActive(true);
                    
                            for(var i = 0; i < _gameObjetHit.GetComponent<MachineCost>().buildingMaterialList.Count; i++)
                            {
                                DestroyImmediate(_gameObjetHit.transform.parent.GetChild(_gameObjetHit.transform.parent.childCount - 1).gameObject);
                            }

                            StartCoroutine(Dissolve());
                        }
                    }
                    break;

                case "Tirolienne":
                    textUI.SetActive(true);
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        _gameObjetHit.transform.parent.GetComponent<TirolienneMachine>().UseTirolienne(_gameObjetHit);
                    }
                    break;
            }
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
            
            if(thisGeneratorUIDisplay != null)
            {
                thisGeneratorUIDisplay.DeactivateUIDisplay();
                thisGeneratorUIDisplay = null;
            }
        
            _isUIUp = false;
            modeSelection.canPlayerSwitchMode = true;
            _playerMenuing.inMenu = false;
            _playerMenuing.OutMenu();
        }
    }

    private IEnumerator Dissolve()
    {
        dissolveTimer = 1;
        while (dissolveTimer >= -1)
        {
            dissolveTimer -= 0.01f;
            dissolveMaterial.SetFloat("_Slider", dissolveTimer);
            
            yield return new WaitForSeconds(0.01f);
        }
    }
}
