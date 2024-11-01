using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject batEnemyPrefab;
    [SerializeField] private GameObject enemyHolder;

    [SerializeField] private float spawnRadius; // How far away the enemy should spawn

    [SerializeField] private float maxEnemies; // How many enemies can be spawned at once (while still alive)
    public float aliveEnemies; // Amount of how many enemies are alive right now

    [Header("Other References")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject timerObject;

    private void Start()
    {
        StartCoroutine(KeepSpawningEnemies());
    }

    private void SpawnEnemy()
    {
        Vector3 playerPosition = player.position;
        
        // Get random angle and random distance for spawn position
        float randomAngle = Random.Range(0f, 2f * Mathf.PI); // Random angle between 0 and 360 degrees
        float randomDistance = Random.Range(spawnRadius, spawnRadius +5); // Random distance
        
        // Calculates distance with polar coordinates (this means how far it will be from the center)
        Vector3 spawnPosition = new Vector3(playerPosition.x + Mathf.Cos(randomAngle) * randomDistance, 
        playerPosition.y + Mathf.Sin(randomAngle) * randomDistance, 0f);
        
        Instantiate(batEnemyPrefab, spawnPosition, Quaternion.identity, enemyHolder.transform);
    }

    private IEnumerator KeepSpawningEnemies()
    {
        while (true)  // Keeps the coroutine infinitely running
        {
            if (Time.timeScale == 1f && aliveEnemies < maxEnemies) // Only spawns enemies when Time.timeScale = 0f
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(timerObject.GetComponent<Timer>().spawningFrequency);
        }
    }
}