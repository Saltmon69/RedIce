using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlueprintMode : MonoBehaviour
{
    /*
     * faire un code qui peut etre mis sur tous les état et est modulable pour faire TOUS les changements dans l inspecteur
     * x bouton = reviens au parent et active son script
     * y bouton = avancer sur x enfant et desactive se script
     *
     * 2 listes : liste des bouton pour avancer, et liste des enfant en corrélation (bouton 1 = go a e enfant 1 etc)
     * une derniere liste pour les components
     *
     * le script différenciel est appart. Donc la fonction principale du module et uniquement sa fonction se trouve dans un autre script rattaché a l objet
     */

    public List<KeyCode> keyCodeGoForwardButtons;
    public List<GameObject> forwardModes;
    
    public KeyCode keyCodeGoBackButton;

    private List<MonoBehaviour> attachedScripts;
    private BlueprintMode _parentBlueprint;
    
    void Awake()
    {
        attachedScripts = this.gameObject.GetComponents<MonoBehaviour>().ToList();
        _parentBlueprint = this.gameObject.transform.parent.GetComponent<BlueprintMode>();
    }

    public void AllComponentsOff()
    {
        for (var i = 0; i < attachedScripts.Count; i++)
        {
            attachedScripts[i].enabled = false;
        }
    }
    
    public void AllComponentsOn()
    {
        for (var i = 0; i < attachedScripts.Count; i++)
        {
            attachedScripts[i].enabled = true;
        }
    }

    void Update()
    {
        GoBack();
        GoForward();
    }

    public void GoBack()
    {
        if (Input.GetKeyDown(keyCodeGoBackButton))
        {
            _parentBlueprint.AllComponentsOn();
            this.gameObject.SetActive(false);
        }
    }

    public void GoForward()
    {
        for (var i = 0; i < forwardModes.Count; i++)
        {
            if (Input.GetKeyDown(keyCodeGoForwardButtons[i]))
            {
                forwardModes[i].SetActive(true);
                AllComponentsOff();
            }
        }
    }
}
