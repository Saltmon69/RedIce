using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingBehaviour : MonoBehaviour
{
    private void Start()
    {
        Canvas canvas = gameObject.GetComponentInChildren<Canvas>();
        canvas.worldCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        gameObject.transform.LookAt(GameObject.Find("Player").transform.position);
    }
}
