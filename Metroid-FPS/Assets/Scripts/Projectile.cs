using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 1.0f;
    [SerializeField] private float destroyTimer = 4.0f;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private GameObject scorchMark;
    [SerializeField] private float hitEffectOffset = 0.1f;

    private Rigidbody projectileRigidbody;
    private Collider playerCollider;

    private void Awake()
    {
        projectileRigidbody = GetComponent<Rigidbody>();
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
        Destroy(gameObject, destroyTimer);
    }

    private void Start()
    {
         Physics.IgnoreCollision(transform.GetComponent<Collider>(), playerCollider);
    }

    private void FixedUpdate()
    {
        projectileRigidbody.MovePosition(transform.position + transform.forward * projectileSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Quaternion hitNormal = Quaternion.FromToRotation(Vector3.forward, contact.normal);
        Vector3 contactPoint = contact.point + contact.normal * hitEffectOffset;
        Instantiate(hitEffectPrefab, contactPoint, hitNormal);
        //TODO Add random rotation to scorch effect
        Instantiate(scorchMark, contactPoint, hitNormal);
        Destroy(gameObject);
    }
}
