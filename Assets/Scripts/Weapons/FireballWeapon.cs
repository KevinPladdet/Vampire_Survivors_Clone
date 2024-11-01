using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballWeapon : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    private GameObject weaponHolder;
    [SerializeField] private float fireballSpeed;

    private Transform targetEnemy;  // The fireball will go towards the targetEnemy

    void Start()
    {
        weaponHolder = GameObject.Find("WeaponHolder");
        StartCoroutine(DeleteFireball());
    }

    void Update()
    {
        if (targetEnemy != null)
        {
            // Move towards the targetEnemy
            Vector2 direction = (targetEnemy.position - transform.position).normalized;
            transform.Translate(direction * (fireballSpeed * weaponHolder.GetComponent<WeaponManager>().projectileSpeedMultiplier) * Time.deltaTime, Space.World);

            // Rotates the fireball correctly depending on the direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            // If there is no targetEnemy, the fireball will be destroyed
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform enemy)
    {
        targetEnemy = enemy;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyScript enemy = other.GetComponent<EnemyScript>();

            enemy.StartCoroutine(enemy.HitCooldown(0.5f));
            enemy.TakeDamage(weapon.weaponDamage * weaponHolder.GetComponent<WeaponManager>().spinachMultiplier);
            enemy.StartCoroutine(enemy.SlowdownEnemy(1f));

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("TilemapFront"))
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DeleteFireball()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
}