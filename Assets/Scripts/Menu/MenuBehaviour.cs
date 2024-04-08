using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour
{

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
