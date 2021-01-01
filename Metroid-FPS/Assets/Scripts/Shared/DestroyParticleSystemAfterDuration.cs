using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleSystemAfterDuration : MonoBehaviour
{
    [SerializeField] private ParticleSystem myParticleSystem;

    private void Awake()
    {
        Destroy(gameObject, myParticleSystem.main.duration);
    }
}
