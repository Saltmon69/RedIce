using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMenuing : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject inventory;
    [SerializeField] GameObject map;
    public void OnMainMenuPressed()
    {
        if (inventory.activeSelf)
        {
            inventory.SetActive(false);
            
        }
        else if (map.activeSelf)
        {
            map.SetActive(false);
            
        }
        else
        {
            Debug.Log("Main Menu");
            mainMenu.SetActive(true);
            
        }
    }
    
    public void OnInventoryPressed()
    {
        Debug.Log("Inventory");
        inventory.SetActive(true);
    }
    
    public void OnMapPressed()
    {
        Debug.Log("Map");
        map.SetActive(true);

    }
}
