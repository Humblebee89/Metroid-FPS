using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 1.0f;
    [SerializeField] private float destroyTimer = 4.0f;
    [SerializeField] private int damage = 10;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private GameObject scorchMark;
    [SerializeField] private float hitEffectOffset = 0.1f;
    [SerializeField] private bool randomZRotation;

    private Rigidbody projectileRigidbody;
    private Collider playerCollider;

    private void Awake()
    {
        projectileRigidbody = GetComponent<Rigidbody>();
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
        Destroy(gameObject, destroyTimer);

        if(randomZRotation)
            transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
    }

    private void Start()
    {
         Physics.IgnoreCollision(transform.GetComponent<Collider>(), playerCollider);
    }

    private void FixedUpdate()
    {
        if(projectileRigidbody != null)
            projectileRigidbody.MovePosition(transform.position + transform.forward * projectileSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "IgnoreWeaponCollision")
            return;

        if (collision.gameObject.TryGetComponent<Damageable>(out Damageable damageable))
            damageable.TakeDamage(damage);

        ContactPoint contact = collision.contacts[0];
        Quaternion hitNormal = Quaternion.FromToRotation(Vector3.forward, contact.normal);
        Vector3 contactPoint = contact.point + contact.normal * hitEffectOffset;
        Instantiate(hitEffectPrefab, contactPoint, hitNormal);

        if (collision.gameObject.tag != "Enemy")
        {
            //TODO Add random rotation to scorch effect
            Instantiate(scorchMark, contactPoint, hitNormal);
        }

        Destroy(gameObject);
    }
}
