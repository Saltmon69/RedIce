using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePower : MonoBehaviour
{

    private GameObject powerUI;
    private GameObject _thisPowerUI;
    private LayerMask _layerMask;
    public int maxPower;
    public int currentPowerUsage;
    public bool isPlayerOn;
    private Collider[] _colliders;
    private List<Collider> _colliderHitList;
    private Text _powerTextUI;
    private int _numberOfFlashes;
    private ComputerUIDisplay _computerUIDisplay;
    private GeneratorUIDisplay _generatorUIDisplay;

    public void Awake()
    {
        powerUI = Resources.Load<GameObject>("ComputerMachine/UIBase");
        _layerMask = LayerMask.GetMask("Default");
        _colliderHitList = new List<Collider>();
        try
        {
            _computerUIDisplay = GameObject.FindWithTag("Computer").GetComponent<ComputerUIDisplay>();
        }catch(NullReferenceException){}

        _generatorUIDisplay = this.gameObject.transform.parent.GetChild(this.gameObject.transform.parent.childCount - 1).gameObject.GetComponent<GeneratorUIDisplay>();
    }

    public void Update()
    {
        if(_computerUIDisplay == null)
        {
            try
            {
                _computerUIDisplay = GameObject.FindWithTag("Computer").GetComponent<ComputerUIDisplay>();
            }
            catch(NullReferenceException)
            {
                return;
            }
        }
        
        maxPower = _computerUIDisplay.maxPower;
        currentPowerUsage = _computerUIDisplay.currentPowerUsage;
        
        _colliders = Physics.OverlapBox(this.transform.position + Vector3.up * 2, this.transform.lossyScale / 2, Quaternion.identity, _layerMask, QueryTriggerInteraction.Ignore);
        _colliderHitList.Clear();
        _colliderHitList.AddRange(_colliders);

        isPlayerOn = false;
        for(var i = 0; i < _colliderHitList.Count; i++)
        {
            if(_colliderHitList[i].transform.CompareTag("Player"))
            {
                isPlayerOn = true;
                
                if(_thisPowerUI == null)
                {
                    _thisPowerUI = Instantiate(powerUI);
                    _powerTextUI = _thisPowerUI.transform.GetChild(0).GetChild(0).GetComponent<Text>();
                    _generatorUIDisplay.powerUI = _powerTextUI.gameObject;
                }
            }
        }

        if(!isPlayerOn && _thisPowerUI != null)
        {
            Destroy(_thisPowerUI);
            _thisPowerUI = null;
            isPlayerOn = false;
        }

        if (_thisPowerUI != null)
        {
            _powerTextUI.text = currentPowerUsage + " / " + maxPower;
        }
    }

    public void Flash()
    {
        _numberOfFlashes = 5;
        StopAllCoroutines();
        StartCoroutine(Flashes());
    }

    private IEnumerator Flashes()
    {
        while(_numberOfFlashes >= 0)
        {
            _powerTextUI.color = _numberOfFlashes % 2 == 1 ? Color.red : Color.white;
            
            _numberOfFlashes--;
            
            yield return new WaitForSeconds(0.1f);
        }
    }
}
