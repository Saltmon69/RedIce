using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStart : MonoBehaviour
{

    public InventoryManager inventoryManager;

    public List<ItemClass> itemClassList;

    void Start()
    {
        for(var i = 0; i < itemClassList.Count; i++)
        {
            inventoryManager.itemToGive = itemClassList[i];
            inventoryManager.numberOfItemsToGive = 100;
            
            inventoryManager.DevCheat();
        }

        //StartCoroutine(time());
    }

    public IEnumerator time()
    {
        while (true)
        {
            Debug.Log(Time.timeScale);
            yield return new WaitForSecondsRealtime(0.1f);
        }

    }
}
