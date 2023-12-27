using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuScript : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] PlayerMenuing playerMenuing;
    
    [SerializeField] GameObject pingPrefab;
    RaycastHit itemHit;
    
    
    
    private Data data;
    
    
    
    public void OnPingPressed()
    {
        data = new Data();
        
        playerMenuing.inMenu = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        itemHit = playerInteraction.RaycastMaker(100f);    
        
        if(itemHit.collider == null || itemHit.collider.CompareTag("Obstacle") || itemHit.collider.CompareTag("Ground") || itemHit.collider.CompareTag("Player") || itemHit.collider.CompareTag("Minerai"))
        {
            
            if (playerManager.activePing != null)
            {
                Destroy(playerManager.activePing);
                data.ping = Instantiate(pingPrefab, itemHit.point, Quaternion.identity);
                
            }
            else
            {
                data.ping = Instantiate(pingPrefab, itemHit.point, Quaternion.identity);
            }
            
            if (itemHit.collider != null)
            {
                if (itemHit.collider.CompareTag("Minerai"))
                {
                    data.itemPinged = itemHit.collider.gameObject;
                }
                else
                {
                    data.itemPinged = null;
                }
            }
        }
        data.order = Order.GoOnPing;
        playerManager.NotifyObservers(data);
        Invoke("InvokeNotify", 0.2f);
        playerInteraction.radialMenu.SetActive(false);
        
    }
    
    public void OnFollowPressed()
    {
        data = new Data();
        
        data.ping = null;
        data.itemPinged = null;
        data.order = Order.Follow;
        playerManager.NotifyObservers(data);
        playerMenuing.inMenu = false;
        playerInteraction.radialMenu.SetActive(false);
        
    }
    
    private void InvokeNotify()
    {
        playerManager.NotifyObservers(data);
    }
}
