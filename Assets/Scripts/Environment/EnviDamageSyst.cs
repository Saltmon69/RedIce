using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviDamageSyst : MonoBehaviour
{
    [SerializeField] private ZoneType zoneType;
    [SerializeField] private float damagePerCooldown;
    [SerializeField] private float cooldown;

    private float timer = 0f;
    private PlayerManager playerManager;
    bool isPlayerInZone;
    
    
    
    private void Start()
    {
        playerManager = PlayerManager.instance;
    }

    private void FixedUpdate()
    {
        if (isPlayerInZone)
        {
            if (timer <= 0)
            {
                playerManager.TakeDamage(damagePerCooldown, zoneType);
                timer = cooldown;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
        }
    }
}

public enum ZoneType
{
    Hot, Cold, Pressure, Toxic, Radiation, LowOxygen
}
