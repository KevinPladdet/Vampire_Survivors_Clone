using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    private Transform playerTransform;
    private Rigidbody2D rb;
    private GameObject player;
    
    [Header("Enemy Variables")]
    [SerializeField] private float enemySpeed;
    private float originalEnemySpeed;
    
    // Attacking
    [SerializeField] private float enemyDamage;
    private bool isAttackingPlayer = false;
    [SerializeField] private float enemyHealth;

    // Taking Damage
    public bool onHitCooldown;

    [Header("Flash Hit")]
    [SerializeField] private Material hitMaterial;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;

    [Header("Pickup References")]
    [SerializeField] private GameObject turkeyPrefab;
    [SerializeField] private GameObject goldbagPrefab;
    [SerializeField] private GameObject pickupHolder;
    [SerializeField] private GameObject blueGemPrefab;
    [SerializeField] private GameObject greenGemPrefab;
    [SerializeField] private GameObject redGemPrefab;

    [Header("Sound Effects")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip enemyHitSFX;
    private bool sfxCooldownBool = true;

    [Header("Other References")]
    [SerializeField] private GameObject timerObject;
    [SerializeField] private GameObject enemyHolder;

    private float pickUpRandomizer;
    private float gemRandomizer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        pickupHolder = GameObject.Find("PickupHolder");
        timerObject = GameObject.Find("Timer");
        enemyHolder = GameObject.Find("EnemyHolder");
        audioSource = GameObject.FindGameObjectWithTag("AudioSourceSFX").GetComponent<AudioSource>();

        enemyHolder.GetComponent<EnemySpawner>().aliveEnemies += 1;

        originalEnemySpeed = enemySpeed;

        // Flash Hit
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;

        pickUpRandomizer = Random.Range(1f, 150f);
        gemRandomizer = Random.Range(1f, 150f);

        // Enemy Stats Multiplier
        enemyDamage *= timerObject.GetComponent<Timer>().enemyStatsMultiplier;
        enemyHealth *= timerObject.GetComponent<Timer>().enemyStatsMultiplier;
    }

    private void Update()
    {
        MoveTowardsPlayer();

        if (isAttackingPlayer && player.GetComponent<PlayerMovement>().canTakeDamage)
        {
            player.GetComponent<PlayerMovement>().TakeDamage(enemyDamage);
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        rb.velocity = direction * enemySpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            isAttackingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            isAttackingPlayer = false;
        }
    }

    // Is called to deal damage to the enemy
    public void TakeDamage(float damage)
    {
        StartCoroutine(sfxCooldown()); // enemyHitSFX
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            EnemyDeath();
        }
        else
        {
            StartCoroutine(ChangeMaterialForHit());
        }
    }

    // Slows the enemy down for 0.1s
    public IEnumerator SlowdownEnemy(float speed)
    {
        enemySpeed -= speed;
        yield return new WaitForSeconds(0.2f);
        enemySpeed = originalEnemySpeed;
    }

    // Makes the enemy fully white for 0.1s upon getting hit
    private IEnumerator ChangeMaterialForHit()
    {
        spriteRenderer.material = hitMaterial;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = originalMaterial;
    }

    // Hit cooldown for the enemy
    public IEnumerator HitCooldown(float enemyHitCooldown)
    {
        onHitCooldown = true;
        yield return new WaitForSeconds(enemyHitCooldown);
        onHitCooldown = false;
    }

    // Is called when the enemy dies
    void EnemyDeath()
    {
        player.GetComponent<PlayerMovement>().enemiesDefeated += 1;

        if (pickUpRandomizer >= 1 && gemRandomizer <= 4)
        {
            Instantiate(turkeyPrefab, transform.position, Quaternion.identity, pickupHolder.transform);
        }
        else if (pickUpRandomizer >= 146 && gemRandomizer <= 150)
        {
            Instantiate(goldbagPrefab, transform.position, Quaternion.identity, pickupHolder.transform);
        }
        else // If a turkey or goldbag did not drop, it will drop a gem
        {
            if (gemRandomizer >= 1 && gemRandomizer <= 10)
            {
                Instantiate(greenGemPrefab, transform.position, Quaternion.identity, pickupHolder.transform);
            }
            else if (gemRandomizer == 100)
            {
                Instantiate(redGemPrefab, transform.position, Quaternion.identity, pickupHolder.transform);
            }
            else
            {
                Instantiate(blueGemPrefab, transform.position, Quaternion.identity, pickupHolder.transform);
            }  
        }

        enemyHolder.GetComponent<EnemySpawner>().aliveEnemies -= 1;
        Destroy(gameObject);
    }

    public IEnumerator sfxCooldown()
    {
        if (sfxCooldownBool)
        {
            sfxCooldownBool = false;
            audioSource.PlayOneShot(enemyHitSFX);
            yield return new WaitForSeconds(0.1f);
            sfxCooldownBool = true;
        }
    }
}
