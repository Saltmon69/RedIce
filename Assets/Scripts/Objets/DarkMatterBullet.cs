using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkMatterBullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody rb;
    


    void Start()
    {
        rb.velocity = transform.forward * speed;
    }

    private void Update()
    {
        Destroy(gameObject, 5f);
    }
}
