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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DarkMatterObject"))
        {
            DarkMatterObject dmo = other.GetComponent<DarkMatterObject>();
            
            if (dmo.darkMatterState == DarkMatterState.DarkMatter)
            {
                dmo.darkMatterState = DarkMatterState.Normal;
                Destroy(gameObject, 0.1f);
            }
            else if (dmo.darkMatterState == DarkMatterState.Normal)
            {
                dmo.darkMatterState = DarkMatterState.DarkMatter;
                Destroy(gameObject, 0.1f);
            }
        }
        else
        {
            Destroy(gameObject, 5f);
        }
       
    }
}
