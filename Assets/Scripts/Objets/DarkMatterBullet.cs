using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkMatterBullet : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody rb;
    public bool isVisible;
    public float timeToDestroy = 5f;
    public float timer = 0f;
    private float speedDisplay;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward;
        speedDisplay = rb.velocity.magnitude;
    }
    private void Update()
    {
        rb.velocity = transform.forward * speed * (Time.time/10);
        speedDisplay = rb.velocity.magnitude;
        Debug.Log(speedDisplay);
        
        if (isVisible == false)
        {
            timer += Time.deltaTime;
            if (timer >= timeToDestroy)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            timer = 0;
        }
        
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
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }

    private void OnBecameVisible()
    {
        isVisible = true;
    }
}
