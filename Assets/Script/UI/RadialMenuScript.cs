using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuScript : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] PlayerInteraction playerInteraction;
    
    [SerializeField] GameObject pingPrefab;
    RaycastHit itemHit;
    
    
    public void OnPingPressed()
    {
        
        playerInteraction.RaycastMaker(100f);    
        
        if(itemHit.collider == null || itemHit.collider.CompareTag("Obstacle") || itemHit.collider.CompareTag("Ground") || itemHit.collider.CompareTag("Player") || itemHit.collider.CompareTag("Minerai"))
        {
            playerManager.data.ping = Instantiate(pingPrefab, itemHit.point, Quaternion.identity);
            
            if (itemHit.collider.CompareTag("Minerai"))
            {
                playerManager.data.itemPinged = itemHit.collider.gameObject;
            }
            else
            {
                playerManager.data.itemPinged = null;
            }
        }
        
        playerManager.data.order = Order.GoOnPing;
        playerInteraction.radialMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        
        playerManager.NotifyObservers();
    }
    
    public void OnFollowPressed()
    {
        playerManager.data.order = Order.Follow;
        playerInteraction.radialMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        
        playerManager.NotifyObservers();
    }
}
