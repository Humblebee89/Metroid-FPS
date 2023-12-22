using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private bool destroyObjectOnDeath;
    [SerializeField] private GameObject objectToDestroy;
    [SerializeField] private GameObject deathParticleEffect;

    private int health;
    private bool dead;

    public UnityEvent onDeath;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if(health > 0)
            health -= amount;

        if (health <= 0)
            KillDamageable();
    }

    private void KillDamageable()
    {
        if (dead == false)
        {
            GameObject.Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
            onDeath?.Invoke();

            dead = true;

            if (destroyObjectOnDeath)
                Destroy(objectToDestroy);
        }
    }   

}
