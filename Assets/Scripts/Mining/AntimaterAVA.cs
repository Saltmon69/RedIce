using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AntimaterAVA : MonoBehaviour
{
    public GameObject avaGlasses;

    public GameObject antimaterOrbPrefab;

    public InventoryUpgrade inventoryUpgrade;
    
    private Vector3 _size;

    private void Awake()
    {
        _size = this.gameObject.transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Minerai") && other.GetComponent<MineraiClass>().isFilledWithAntimater)
        {
            Instantiate(antimaterOrbPrefab, other.transform);
            return;
        }
        
        if(other.CompareTag("DarkMatterObject") && 
           inventoryUpgrade.upgradeItemInInventory.Contains(Resources.Load<ItemClass>("SO/Upgrades/AntimaterPlayerBootsModule")) && 
           other.GetComponent<DarkMatterObject>().darkMatterType == DarkMatterType.Platform &&
           other.GetComponent<DarkMatterObject>().darkMatterState == DarkMatterState.Normal)
        {
            other.isTrigger = false;
        }
    }

    public void Update()
    {
        this.gameObject.transform.localScale = avaGlasses.activeInHierarchy ? _size : Vector3.zero;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Minerai") && other.GetComponent<MineraiClass>().isFilledWithAntimater)
        {
            Destroy(other.transform.GetChild(other.transform.childCount - 1).gameObject);
        }
    }
}
