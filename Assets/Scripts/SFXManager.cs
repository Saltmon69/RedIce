using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    
    public AudioSource InstantiatedSFXObject;
    
    [SerializeField]private AudioSource SFXObject;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        if (instance == null)
        {
            instance = this;
        }
    }
    
    public void PlaySFX(AudioClip audioClip, Transform transform, float volume, bool isLoop)
    {
        AudioSource audioSource = Instantiate(SFXObject, transform.position, Quaternion.identity);
       
        InstantiatedSFXObject = audioSource;
        
        audioSource.loop = isLoop;
        
        audioSource.clip = audioClip;
        
        audioSource.volume = volume;
        
        audioSource.Play();

        if (!isLoop)
        {
            float audioLength = audioClip.length;
            
            Destroy(audioSource.gameObject, audioLength);
            
        }
    }
}
