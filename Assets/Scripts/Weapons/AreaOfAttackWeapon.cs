using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfAttackWeapon : MonoBehaviour
{

    // Scriptable Object for AoE weapons (like Garlic and Holy Water)
    public Weapon weapon;

    private SpriteRenderer spriteRenderer;

    private GameObject weaponHolder;

    // Opacity changing
    [SerializeField] private float opacitySpeed;
    [SerializeField] private float minOpacity;
    [SerializeField] private float maxOpacity;

    // Attacking
    private HashSet<EnemyScript> enemiesInArea = new HashSet<EnemyScript>(); // Tracks enemies inside the area / AoE

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        weaponHolder = GameObject.Find("WeaponHolder");

        if (gameObject.name == "HolyWaterCircle(Clone)")
        {
            StartCoroutine(DeleteHolyWater());
        }
    }

    void Update()
    {
        // Loops between opacity values for the sprite renderer
        float opacity = Mathf.PingPong(Time.time * opacitySpeed, maxOpacity - minOpacity) + minOpacity;
        Color color = spriteRenderer.color;
        // Sets the alpha value to opacity float
        color.a = opacity;
        // Applies the color to the sprite renderer
        spriteRenderer.color = color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyScript enemy = other.GetComponent<EnemyScript>();

            // Adds enemy to the HashSet if it is not already being damaged
            if (!enemiesInArea.Contains(enemy))
            {
                enemiesInArea.Add(enemy);
                StartCoroutine(DamageEnemyOverTime(enemy));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyScript enemy = other.GetComponent<EnemyScript>();

            // Removes enemy from the HashSet when it left the area / AoE
            if (enemiesInArea.Contains(enemy))
            {
                enemiesInArea.Remove(enemy);
            }
        }
    }

    // Deals damage every 1.5 seconds while enemy is inside the area
    private IEnumerator DamageEnemyOverTime(EnemyScript enemy)
    {
        while (enemiesInArea.Contains(enemy))
        {
            if (!enemy.onHitCooldown)
            {
                enemy.StartCoroutine(enemy.HitCooldown(0.5f)); // Enemy hit cooldown
                enemy.TakeDamage(weapon.weaponDamage * weaponHolder.GetComponent<WeaponManager>().spinachMultiplier);
                enemy.StartCoroutine(enemy.SlowdownEnemy(1f)); // Slowdown enemySpeed for 0.1 second
            }
            yield return new WaitForSeconds(0.5f); // Wait before dealing damage again
        }
    }

    private IEnumerator DeleteHolyWater()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}