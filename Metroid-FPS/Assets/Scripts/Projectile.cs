using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 1.0f;

    private Rigidbody projectileRigidbody;

    private void Awake()
    {
        projectileRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        projectileRigidbody.MovePosition(transform.position + transform.forward * projectileSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
