using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour
{

    public RectTransform initPosition;
    public RectTransform outOfScreenPosition;

    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject creditsMenu;

    private void Start()
    {
        initPosition = mainMenu.GetComponent<RectTransform>();
        outOfScreenPosition = optionsMenu.GetComponent<RectTransform>();
        
    }

    public void Play()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void Options()
    {
        
    }

    public void Credits()
    {
        
    }

    public void QuitMenu()
    {
        Application.Quit();
    }
}
