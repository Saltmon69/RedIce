using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineraiSpawner : MonoBehaviour
{
    public GameObject minerai;
    public GameObject activeMinerai;
    private float timer;
    public float spawnDelay = 3f;

    
    private void Awake()
    {
        activeMinerai = Instantiate(minerai, transform.position, transform.rotation, gameObject.transform);
        
    }

    private void Update()
    {
        if (activeMinerai == null)
        {
            if (timer < spawnDelay)
            {
                timer += Time.deltaTime;
            }
            else
            {
                activeMinerai = Instantiate(minerai, transform.position, transform.rotation, gameObject.transform);
                timer = 0;
            }
        }
    }
    
}
