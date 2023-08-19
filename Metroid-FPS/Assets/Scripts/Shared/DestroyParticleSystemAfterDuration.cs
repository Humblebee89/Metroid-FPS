using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleSystemAfterDuration : MonoBehaviour
{
    [SerializeField] private ParticleSystem myParticleSystem;
    [SerializeField] private GameObject specificObjectToDestroy;

    private void Awake()
    {
        if(specificObjectToDestroy == null)
            Destroy(gameObject, myParticleSystem.main.duration);
        else
            Destroy(specificObjectToDestroy, myParticleSystem.main.duration);
    }
}
