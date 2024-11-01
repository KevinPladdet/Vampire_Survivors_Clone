using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnivesWeapon : MonoBehaviour
{

    [SerializeField] private Weapon weapon;
    private GameObject weaponHolder;
    private Vector2 moveDirection;
    [SerializeField] private float knifeSpeed;

    void Start()
    {
        weaponHolder = GameObject.Find("WeaponHolder");
        StartCoroutine(DeleteThrowingKnife());
    }

    void Update()
    {
        // Move the knife forwards based on the set direction
        transform.Translate(Vector3.right * (knifeSpeed * weaponHolder.GetComponent<WeaponManager>().projectileSpeedMultiplier) * Time.deltaTime);
    }

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
        
        // Get angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Set knife Z rotation based on angle
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
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

    private IEnumerator DeleteThrowingKnife()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
