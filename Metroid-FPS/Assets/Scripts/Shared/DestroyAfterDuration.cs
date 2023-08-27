using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDuration : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    private void Awake()
    {
        Destroy(gameObject, duration);
    }
}
