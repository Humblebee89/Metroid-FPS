using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private float spawnDelay = 3.0f;

    private GameObject spawnedEnemy;

    private void Start()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (!spawnedEnemy)
        {
            spawnedEnemy = GameObject.Instantiate(enemy, transform.position, transform.rotation);
            //This feels hacky. Find out why Ienumerators don't work
            spawnedEnemy.GetComponent<Damageable>().onDeath.AddListener(startCoroutine);
        }
        else
            print("Spawned enemy is " + spawnedEnemy);
    }

    private void startCoroutine()
    {
        StartCoroutine("SpawnEnemyDelayed");
    }

    private IEnumerator SpawnEnemyDelayed()
    {
        yield return new WaitForSeconds(spawnDelay);

        SpawnEnemy();
    }    

}
