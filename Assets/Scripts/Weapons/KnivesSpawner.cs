using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnivesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject knifePrefab;
    [SerializeField] private float spawnInterval;

    private Vector2 lastMoveDirection; // Tracks the last movement direction

    private void Start()
    {
        StartCoroutine(SpawnKnives());
    }

    private void Update()
    {
        // Track player movement directions
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (moveInput != Vector2.zero) // Updates direction only when moving
        {
            lastMoveDirection = moveInput;
        }
    }

    private IEnumerator SpawnKnives()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Spawn knife in the last moving direction
            if (lastMoveDirection != Vector2.zero)
            {
                GameObject knife = Instantiate(knifePrefab, transform.position, Quaternion.identity);
                knife.GetComponent<KnivesWeapon>().SetDirection(lastMoveDirection); // Set the direction
            }
        }
    }
}