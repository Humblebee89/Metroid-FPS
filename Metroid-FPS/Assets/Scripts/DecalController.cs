using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalController : MonoBehaviour
{
    [SerializeField] float fadeDelay = 1.0f;
    [SerializeField] float fadeTime = 3.0f;

    private DecalProjector decalProjector;
    private float startingOpacity;

    private void Awake()
    {
        decalProjector = GetComponent<DecalProjector>();
    }

    private void Start()
    {
        startingOpacity = decalProjector.fadeFactor;
    }

    private void OnEnable()
    {
        StartCoroutine(FadeOut());
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
