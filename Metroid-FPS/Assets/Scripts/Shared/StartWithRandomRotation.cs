using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWithRandomRotation : MonoBehaviour
{
    [SerializeField] private bool randomXRotation;
    [SerializeField] private bool randomYRotation;
    [SerializeField] private bool randomZRotation;
    private void Awake()
    {
        if (randomXRotation)
            transform.Rotate(Random.Range(0.0f, 360.0f),transform.rotation.y, transform.rotation.z);
        if (randomYRotation)
            transform.Rotate(transform.rotation.x, Random.Range(0.0f, 360.0f), transform.rotation.z);
        if (randomZRotation)
            transform.Rotate(transform.rotation.x, transform.rotation.y, Random.Range(0.0f, 360.0f));
    }
}
