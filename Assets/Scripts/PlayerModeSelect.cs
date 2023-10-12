using UnityEngine;
using UnityEngine.UI;

public class PlayerModeSelect : MonoBehaviour
{
    public int modeSelected;
    public int oldSelectedMod;
    
    public float wait;
    public CanvasGroup canvasGroupMode; 
        
    public Slider modeSelectedUI;

    public GameObject handsFreeModeManager;
    public GameObject miningModeManager;
    public GameObject blueprintModeManager;
    void Update()
    {
        UICanvasAlpha();
        OnKeyboardInput();
        
        modeSelected = (modeSelected + 3 + (int)Input.mouseScrollDelta.y) % 3;
        modeSelectedUI.value = (float)modeSelected / 2;

        if (modeSelected != oldSelectedMod)
        {
            ModeSelectionOutput();
        }

        oldSelectedMod = modeSelected;
    }

    public void ModeSelectionOutput()
    {
        if (modeSelected == 0)
        {
            //add animation
            handsFreeModeManager.SetActive(true);
            miningModeManager.SetActive(false);
            blueprintModeManager.SetActive(false);
        }
        
        if (modeSelected == 1)
        {
            //add animation
            handsFreeModeManager.SetActive(false);
            miningModeManager.SetActive(true);
            blueprintModeManager.SetActive(false);

        }
        
        if (modeSelected == 2)
        {
            //add animation
            handsFreeModeManager.SetActive(false);
            miningModeManager.SetActive(false);
            blueprintModeManager.SetActive(true);

        }
    }
    
    public void UICanvasAlpha()
    {
        wait += Time.deltaTime;

        if (wait > 2)
        {
            canvasGroupMode.alpha -= Time.deltaTime;
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            wait = 0;
            canvasGroupMode.alpha = 1;
        }
    }
    
    public void OnKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Ampersand))
        {
            modeSelected = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.Tilde))
        {
            modeSelected = 1;
        }
        
        if (Input.GetKeyDown(KeyCode.Hash))
        {
            modeSelected = 2;
        }
    }
}
