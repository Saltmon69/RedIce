using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerRegeneration : MonoBehaviour
{
    private PlayerManager _playerManager;
    
    public GameObject player;
    private CharacterController _characterController;
    private GameObject _teleportGameObject;
    private Collider _thisCollider;

    public CanvasGroup thisCanvasGroup;

    public bool isDead;
    
    private void Awake()
    {
        player = GameObject.FindWithTag("Player").gameObject;
        _teleportGameObject = this.gameObject.transform.GetChild(0).gameObject;
        _thisCollider = this.gameObject.GetComponent<Collider>();
        _playerManager = player.GetComponent<PlayerManager>();
        _characterController = player.GetComponent<CharacterController>();
    }
    
    public void Update()
    {
        if(_playerManager.oxygen <= 0)
        {
            _playerManager.playerHealth--;
            _playerManager.oxygen = 1;
        }
        
        if(_playerManager.radiation <= 0)
        {
            _playerManager.playerHealth--;
            _playerManager.radiation = 1;
        }
        
        if(_playerManager.pressure <= 0)
        {
            _playerManager.playerHealth--;
            _playerManager.pressure = 1;
        }

        if(_playerManager.playerHealth <= 0 && !isDead)
        {
            isDead = true;
            Debug.Log("you died");
            StartCoroutine(DeathTransition());
        }
    }

    private IEnumerator DeathTransition()
    {
        while(true)
        {
            if(isDead) thisCanvasGroup.alpha += 0.01f;

            if(thisCanvasGroup.alpha >= 1)
            {
                Debug.Log("in a coma");
                _characterController.enabled = false;
                player.transform.position = _teleportGameObject.transform.position;
                player.transform.rotation = _teleportGameObject.transform.rotation;
                _characterController.enabled = true;
                isDead = false;
            }

            if(!isDead)
            {
                Debug.Log("waking up");
                thisCanvasGroup.alpha -= 0.02f;
                if(thisCanvasGroup.alpha <= 0)
                {
                    Debug.Log("woke up");
                    StopAllCoroutines();
                    yield return null;
                }
            }

            yield return new WaitForSeconds(0.02f);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject != player) Physics.IgnoreCollision(_thisCollider, collision);
    }

    private void OnTriggerStay(Collider collision)
    {
        if(collision.gameObject == player)
        {
            if(_playerManager.oxygen < _playerManager.maxOxygen) _playerManager.oxygen++;
            if(_playerManager.pressure < _playerManager.maxPressure)_playerManager.pressure++;
            if(_playerManager.radiation < _playerManager.maxRadiation)_playerManager.radiation++;
            if(_playerManager.playerHealth < _playerManager.playerMaxHealth)_playerManager.playerHealth++;
            
            isDead = false;
        }
    }
}
