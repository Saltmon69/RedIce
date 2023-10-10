using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    private void Update()
    {
        Physics.Raycast(playerCamera.transform.position, transform.forward, out RaycastHit hit, 2f);
    }

    public void OnInteractionPressed()
    {
        print("J'ai appuyé sur E");
    }
}
