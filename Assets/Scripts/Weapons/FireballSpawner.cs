using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float spawnInterval;
    [SerializeField] float searchRadius;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SpawnFireballs());
    }

    private IEnumerator SpawnFireballs()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            
            // Finds closestEnemy before spawning a fireball
            EnemyScript closestEnemy = FindClosestEnemy();

            if (closestEnemy != null)
            {
                GameObject fireball = Instantiate(fireballPrefab, player.position, Quaternion.identity);

                // Sets the closestEnemy.transform as the targetEnemy of the fireball
                fireball.GetComponent<FireballWeapon>().SetTarget(closestEnemy.transform);
            }
        }
    }

    // Find the closest enemy to the player
    private EnemyScript FindClosestEnemy()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        EnemyScript closestEnemy = null;

        EnemyScript[] allEnemies = GameObject.FindObjectsOfType<EnemyScript>();

        foreach (EnemyScript currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - player.position).sqrMagnitude;
            if (distanceToEnemy <= searchRadius * searchRadius)
            {
                if (distanceToEnemy < distanceToClosestEnemy)
                {
                    distanceToClosestEnemy = distanceToEnemy;
                    closestEnemy = currentEnemy;
                }
            }
        }

        return closestEnemy;
    }
}