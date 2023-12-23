using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalController : MonoBehaviour
{
    [SerializeField] float fadeDelay = 1.0f;
    [SerializeField] float fadeTime = 3.0f;
    [SerializeField] float emissionFadeTime = 1.5f;
    [SerializeField] private float emissionBrightness = 1f;

    private DecalProjector decalProjector;
    private float startingOpacity;


    private void Awake()
    {
        decalProjector = GetComponent<DecalProjector>();
        decalProjector.material = new Material(decalProjector.material);
    }

    private void Start()
    {
        startingOpacity = decalProjector.fadeFactor;
    }

    private void OnEnable()
    {
        StartCoroutine(FadeOut());
        StartCoroutine(EmissionFade());
    }

    private IEnumerator EmissionFade()
    {
        float emissionOpacity = emissionBrightness;
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeTime)
        {
            decalProjector.material.SetFloat("_Brightness",  Mathf.Lerp(emissionOpacity, 0.0f, (elapsedTime / emissionFadeTime)));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(fadeDelay);

        float currentOpacity = startingOpacity;
        float elapsedTime = 0.0f;

        while(elapsedTime < fadeTime)
        { 
            decalProjector.fadeFactor = Mathf.Lerp(currentOpacity, 0.0f, (elapsedTime / fadeTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if(elapsedTime >= fadeTime)
        {
            Destroy(gameObject);
        }
    }
}
