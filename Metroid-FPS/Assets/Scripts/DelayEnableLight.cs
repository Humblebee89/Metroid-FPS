using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayEnableLight : MonoBehaviour
{
    [SerializeField] private Light lightSource;
    [SerializeField] private float delay = 0.1f;

    void Start()
    {
        StartCoroutine(DelayEnable());
    }

    private IEnumerator DelayEnable()
    {
        yield return new WaitForSeconds(delay);
        lightSource.enabled = true;
    }
}
