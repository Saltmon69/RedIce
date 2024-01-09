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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DarkMatterObject"))
        {
            if (other.GetComponent<DarkMatterObject>().state == false)
            {
                other.GetComponent<MeshRenderer>().enabled = true;
                other.GetComponent<Collider>().isTrigger = false;
            }
            else if (other.GetComponent<DarkMatterObject>().state == true)
            {
                other.GetComponent<MeshRenderer>().enabled = false;
                other.GetComponent<Collider>().isTrigger = true;
            }
                
        }
    }
}
