using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private bool destroyObjectOnDeath;
    [SerializeField] private GameObject deathParticleEffect;

    private int health;

    public UnityEvent onDeath;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
            KillDamageable();
    }

    private void KillDamageable()
    {
        GameObject.Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
        onDeath?.Invoke();

        if (destroyObjectOnDeath)
            Destroy(gameObject);
    }

}
