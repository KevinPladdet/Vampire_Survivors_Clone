using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyWaterSpawner : MonoBehaviour
{

    [SerializeField] private GameObject holyWaterPrefab;
    [SerializeField] private GameObject projectileHolder;

    [SerializeField] private float spawnRadius; // How far away the enemy should spawn

    [SerializeField] private Transform player;

    private void Start()
    {
        projectileHolder = GameObject.Find("ProjectileHolder");
        player = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(SpawnTimer());
    }

    private void SpawnHolyWater()
    {
        Vector3 playerPosition = player.position;

        // Get random angle and random distance for spawn position
        float randomAngle = Random.Range(0f, 2f * Mathf.PI); // Random angle between 0 and 360 degrees
        float randomDistance = Random.Range(spawnRadius, spawnRadius + 3); // Random distance

        // Calculates distance with polar coordinates (this means how far it will be from the center)
        Vector3 spawnPosition = new Vector3(playerPosition.x + Mathf.Cos(randomAngle) * randomDistance,
        playerPosition.y + Mathf.Sin(randomAngle) * randomDistance, 0f);

        Instantiate(holyWaterPrefab, spawnPosition, Quaternion.identity, projectileHolder.transform);
    }

    private IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(1);

        while (Time.timeScale == 1f)
        {
            SpawnHolyWater();
            yield return new WaitForSeconds(5);
        }
    }
}
