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
    [SerializeField] private float hitEffectColorDecayRate = 0.01f;

    private int health;
    private bool dead;
    private Material material;
    private float redBlendAmount;

    public UnityEvent onDeath;

    private void Start()
    {
        health = maxHealth;
        material = this.GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        UpdateMaterialColor();
    }

    public void TakeDamage(int amount)
    {
        if (health > 0)
        {
            health -= amount;
            redBlendAmount = 1f;
        }

        if (health <= 0)
            KillDamageable();
    }


    private void UpdateMaterialColor()
    {
        if (redBlendAmount <= 0f)
            return;

        redBlendAmount -= hitEffectColorDecayRate;
        material.SetFloat("_HitColorBlendAmount", redBlendAmount);
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
