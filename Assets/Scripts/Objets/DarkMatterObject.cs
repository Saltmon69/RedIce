using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VHierarchy.Libs;

public class DarkMatterObject : MonoBehaviour
{
    public bool state = false;
    [SerializeField] Material darkMatterMaterial;
    [SerializeField] DarkMatterType darkMatterType;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DarkMatterBullet"))
        {
            if (state)
            {
                switch (darkMatterType)
                {
                    case DarkMatterType.Wall:
                        GetComponent<MeshRenderer>().material = darkMatterMaterial;
                        GetComponent<BoxCollider>().isTrigger = false;
                        state = false;
                        other.Destroy();
                        break;
                    case DarkMatterType.Platform:
                        GetComponent<MeshRenderer>().material = default;
                        GetComponent<BoxCollider>().isTrigger = false;
                        state = false;
                        other.Destroy();
                        break;
                }
            }
            else if (!state)
            {
                switch (darkMatterType)
                {
                    case DarkMatterType.Wall:
                        GetComponent<MeshRenderer>().material = default;
                        GetComponent<BoxCollider>().isTrigger = true;
                        state = true;
                        other.Destroy();
                        break;
                    case DarkMatterType.Platform:
                        GetComponent<MeshRenderer>().material = darkMatterMaterial;
                        GetComponent<BoxCollider>().isTrigger = true;
                        state = false;
                        other.Destroy();
                        break;
                }
            }
        }
    }
}

public enum DarkMatterType
{
    Wall, Platform
}
