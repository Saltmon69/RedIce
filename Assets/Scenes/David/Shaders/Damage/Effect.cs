using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public Material screenDamageMat;
    private Coroutine screenDamageTask

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
            ScreenDamageEffect(Random.Range(0.1f, 1));
    }
    void ScreenDamageEffect(float intensity)
    {
            if(screenDamageTask != null)
                StopCoroutine(screenDamageTask);
            screenDamageTask = StartCoroutine(screenDamage(intensity));
    }
    private IEnumerator screenDamage(float intensity)
            //camera shake
            var velocity   
    {
            var targetRadius = Remap(intensity, 0, 1, 0.4f, -0.15f);
            var curRadius = 1, //No damage
            for(float t = 0; curRadius != targetRadius; t += Time.deltatime)
            {
                curRadius = Mathf.Lerp(1, targetRadius, t);
                screenDamageMat.SetFloat("_Vignette_radius", curRadius);
                yield return null;
            }
            for(float t = 0; curRadius < 1; t += Time.deltaTime)
            curRadius = Mathf.Lerp(targetRadius, 1, t):
                }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
