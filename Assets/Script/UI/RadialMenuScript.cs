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
    
    private GameObject ping;
    
    
    public void OnPingPressed()
    {
        playerMenuing.inMenu = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        itemHit = playerInteraction.RaycastMaker(100f);    
        
        if(itemHit.collider == null || itemHit.collider.CompareTag("Obstacle") || itemHit.collider.CompareTag("Ground") || itemHit.collider.CompareTag("Player") || itemHit.collider.CompareTag("Minerai"))
        {
            
            if (playerManager.data.ping != null)
            {
                Destroy(playerManager.data.ping);
                ping = Instantiate(pingPrefab, itemHit.point, Quaternion.identity);
                
            }
            else
            {
                ping = Instantiate(pingPrefab, itemHit.point, Quaternion.identity);
            }
            
            if (itemHit.collider != null)
            {
                if (itemHit.collider.CompareTag("Minerai"))
                {
                    playerManager.data.itemPinged = itemHit.collider.gameObject;
                }
                else
                {
                    playerManager.data.itemPinged = null;
                }
            }
        }
        
        playerManager.data.ping = ping;
        playerManager.data.order = Order.GoOnPing;
        playerManager.NotifyObservers();
        playerInteraction.radialMenu.SetActive(false);
        
       
        
        
    }
    
    public void OnFollowPressed()
    {
        playerManager.data.order = Order.Follow;
        playerInteraction.radialMenu.SetActive(false);
        playerMenuing.inMenu = false;
        
        playerManager.NotifyObservers();
    }
}
