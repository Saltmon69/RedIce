using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseTeleporter : MonoBehaviour
{
    public GameObject player;
    private CharacterController _characterController;
    public GameObject teleport;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").gameObject;
        _characterController = player.GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject == player);
        if(collision.gameObject == player)
        {
            _characterController.enabled = false;
            player.transform.position = teleport.transform.position;
            _characterController.enabled = true;
        }
    }
}
