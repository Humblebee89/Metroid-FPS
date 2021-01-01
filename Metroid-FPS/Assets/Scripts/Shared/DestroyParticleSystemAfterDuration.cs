using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleSystemAfterDuration : MonoBehaviour
{
    private ParticleSystem myParticleSystem;

    private void Awake()
    {
        myParticleSystem = GetComponent<ParticleSystem>();

        Destroy(gameObject, myParticleSystem.main.duration);
    }
}
