using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineraiSpawner : MonoBehaviour
{
    public GameObject minerai;
    private GameObject activeMinerai;
    public float spawnTime = 3f;

    
    private void Awake()
    {
        ClassicSpawn();
    }

    private void Update()
    {
        if (activeMinerai == null)
        {
            StartCoroutine(SpawnMinerai());
        }
    }

    public IEnumerator SpawnMinerai()
    {
        yield return new WaitForSeconds(spawnTime);
        activeMinerai = Instantiate(minerai, transform.position, transform.rotation);
    }
    
    public void ClassicSpawn()
    {
        activeMinerai = Instantiate(minerai, transform.position, transform.rotation);
    }
}
