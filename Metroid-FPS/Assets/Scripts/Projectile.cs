using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 1.0f;
    [SerializeField] private float destroyTimer = 4.0f;
    [SerializeField] private int damage = 10;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private bool hitEffectAlignWithProjectile;
    [SerializeField] private GameObject scorchMark;
    [SerializeField] private float hitEffectOffset = 0.1f;
    [SerializeField] private bool randomZRotation;
    [SerializeField] private LayerMask rayCastLayerMask;
    [SerializeField] private float triggerRaycastRange = 10f;
    [SerializeField] private GameObject[] gameObjectsToDestroyOnCollision;
    [SerializeField] private ParticleSystem[] particleSystemsToDestroyDelayed;

    private Rigidbody projectileRigidbody;
    private Collider playerCollider;
    private float longestParticleLifetime = 0f;
    private bool hasCollided;
    private Quaternion projectileHitNormalAveraged;

    private void Awake()
    {
        projectileRigidbody = GetComponent<Rigidbody>();
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
        FindLongestParticleLifetime();
        Invoke("DestroyAfterParticleEffects", destroyTimer);

        if(randomZRotation)
            transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
    }

    private void FindLongestParticleLifetime()
    {
        if (particleSystemsToDestroyDelayed == null)
            return;

        for(int i=0; i < particleSystemsToDestroyDelayed.Length; i++)
        {
            if (particleSystemsToDestroyDelayed[i].main.startLifetimeMultiplier > longestParticleLifetime)
                longestParticleLifetime = particleSystemsToDestroyDelayed[i].main.startLifetimeMultiplier;
        }
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

    private void DestroyAfterParticleEffects()
    {
        if (particleSystemsToDestroyDelayed == null)
            Destroy(gameObject);
        else
        {
            //Stop the pojectile before killing the particle effects
            projectileSpeed = 0f;

            if (gameObjectsToDestroyOnCollision != null)
            {
                for (int i = 0; i < gameObjectsToDestroyOnCollision.Length; i++)
                {
                    print(gameObjectsToDestroyOnCollision[i].gameObject.name);
                    Destroy(gameObjectsToDestroyOnCollision[i]);
                }
            }

            Destroy(gameObject, longestParticleLifetime);
        }
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
        projectileHitNormalAveraged = transform.rotation * Quaternion.Euler(0, 180, 0);

        if(hitEffectAlignWithProjectile)
            Instantiate(hitEffectPrefab, contactPoint, projectileHitNormalAveraged);
        else
            Instantiate(hitEffectPrefab, contactPoint, hitNormal);

        if (collision.gameObject.tag != "Enemy")
        {
            //TODO Add random rotation to scorch effect
            Instantiate(scorchMark, contactPoint, hitNormal);
        }

        CancelInvoke("DestroyAfterParticleEffects");
        DestroyAfterParticleEffects();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (hasCollided)
            return;

        hasCollided = true;

        if(collider.gameObject.tag == "IgnoreWeaponCollision")
            return;

        if (collider.gameObject.TryGetComponent<Damageable>(out Damageable damageable))
            damageable.TakeDamage(damage);

        RaycastHit hitInfo;
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, triggerRaycastRange, rayCastLayerMask))
        {
            Quaternion hitNormal = Quaternion.FromToRotation(Vector3.forward, hitInfo.normal);
            Vector3 contactPoint = hitInfo.point;
            Instantiate(hitEffectPrefab, contactPoint, hitNormal);

            if (collider.gameObject.tag != "Enemy")
            {
                //TODO Add random rotation to scorch effect
                Instantiate(scorchMark, contactPoint, hitNormal);
            }
        }

     

        CancelInvoke("DestroyAfterParticleEffects");
        DestroyAfterParticleEffects();
    }
}
