using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerModeSelect : MonoBehaviour
{
    public int modeSelected;
    public int oldSelectedMod;
    
    public float wait;
    public float time;
    public CanvasGroup canvasGroupMode; 
        
    public GameObject modeSelectedUI;

    public List<GameObject> modeList;

    public bool canPlayerSwitchMode;
    public bool couldPlayerSwitchMode;
    
    private PlayerMenuing _playerMenuing;

    private void Awake()
    {
        //ajuste le nombre de mode et les assignes
        modeList.Clear();
        
        for(var i = 0; i < this.gameObject.transform.childCount; i++)
        {
           modeList.Add(this.gameObject.transform.GetChild(i).gameObject); 
        }
        
        _playerMenuing = GameObject.FindWithTag("Player").GetComponent<PlayerMenuing>();
    }

    private void Update()
    {
        //permet grâce a notre molette de choisir un des modes
        modeSelected = (modeSelected + modeList.Count + (int)Input.mouseScrollDelta.y) % modeList.Count;

        if(modeSelected != oldSelectedMod && canPlayerSwitchMode)
        {
            ModeSelectionOutput();
            
            StopAllCoroutines();
            StartCoroutine(UICanvasAlpha(wait));
            
            oldSelectedMod = modeSelected;

            for (var i = 0; i < modeSelectedUI.transform.childCount; i++)
            {
                modeSelectedUI.transform.GetChild(i).gameObject.SetActive(false);
            }
            
            modeSelectedUI.transform.GetChild(modeSelected).gameObject.SetActive(true);;
        }
        else
        {
            modeSelected = oldSelectedMod;
        }
        
        if(couldPlayerSwitchMode != canPlayerSwitchMode)
        {
            if(canPlayerSwitchMode)
            {
                _playerMenuing.enabled = true;
            }
            else
            {
                _playerMenuing.enabled = false;
            }
            couldPlayerSwitchMode = canPlayerSwitchMode;
        }

    }

    //active ou désactive les object qui compose les mécanique de chaque modes
    private void ModeSelectionOutput()
    {
        for (var i = 0; i < modeList.Count; i++)
        {
            modeList[i].SetActive(i == modeSelected);
        }
    }
    
    //permet de faire des fondus avec l'ui de sélection des modes
    private IEnumerator UICanvasAlpha(float thisTime)
    {
        time = thisTime;
        canvasGroupMode.alpha = 1;
        
        while (time > 0)
        {
            time -= 0.01f;
            canvasGroupMode.alpha = (time + 0.15f)/(wait - 1);
            
            yield return new WaitForSeconds(0.01f);
        }
    }
}
