using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerModeSelect : MonoBehaviour
{
    public int modeSelected;
    public int oldSelectedMod;
    
    public float wait;
    public float time;
    public CanvasGroup canvasGroupMode; 
        
    public Slider modeSelectedUI;

    public GameObject handsFreeModeManager;
    public GameObject miningModeManager;
    public GameObject blueprintModeManager;

    public bool canPlayerSwitchMode;
    public bool couldPlayerSwitchMode;

    public DeactivatePlayerInput playerInput;
    
    void Update()
    {
        //permet grâce a notre molette de choisir un des modes
        modeSelected = (modeSelected + 3 + (int)Input.mouseScrollDelta.y) % 3;
        
        //permet grâce a nos chiffres de choisir un des modes
        OnKeyboardInput();
        
        if(modeSelected != oldSelectedMod && canPlayerSwitchMode)
        {
            ModeSelectionOutput();
            
            StopAllCoroutines();
            StartCoroutine(UICanvasAlpha(wait));
            
            oldSelectedMod = modeSelected;
            modeSelectedUI.value = (float)modeSelected / 2;
        }
        else
        {
            modeSelected = oldSelectedMod;
        }

        if (couldPlayerSwitchMode != canPlayerSwitchMode)
        {
            if (canPlayerSwitchMode)
            {
                playerInput.Activate();
            }
            else
            {
                playerInput.SoftDeactivate();
            }
            couldPlayerSwitchMode = canPlayerSwitchMode;
        }

    }

    //active ou désactive les object qui compose les mécanique de chaque modes
    private void ModeSelectionOutput()
    {

        if(modeSelected == 0)
        {
            handsFreeModeManager.SetActive(true);
            miningModeManager.SetActive(false);
            blueprintModeManager.SetActive(false);
        }
        
        if(modeSelected == 1)
        {
            handsFreeModeManager.SetActive(false);
            miningModeManager.SetActive(true);
            blueprintModeManager.SetActive(false);
        }
        
        if(modeSelected == 2)
        {
            handsFreeModeManager.SetActive(false);
            miningModeManager.SetActive(false);
            blueprintModeManager.SetActive(true);
        }
    }
    
    //permet de faire des fondus avec l'ui de sélection des modes
    private IEnumerator UICanvasAlpha(float thisTime)
    {
        time = thisTime;
        canvasGroupMode.alpha = 1;
        
        while (wait > 0)
        {
            time -= 0.01f;
            canvasGroupMode.alpha = time/(wait - 1);
            
            yield return new WaitForSeconds(0.01f);
        }
    }
    
    private void OnKeyboardInput()
    {
        if(Input.GetKeyDown(KeyCode.Ampersand))
        {
            modeSelected = 0;
        }
        
        if(Input.GetKeyDown(KeyCode.Tilde))
        {
            modeSelected = 1;
        }
        
        if(Input.GetKeyDown(KeyCode.Hash))
        {
            modeSelected = 2;
        }
    }
}
