using System;
using UnityEngine;

public class MachineCollider : MonoBehaviour
{
    public bool canBePlaced;

    private void Awake()
    {
        canBePlaced = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        canBePlaced = false;
    }

    private void OnCollisionExit(Collision other)
    {
        canBePlaced = true;
    }
}