using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Variables")]
    public float moveSpeed;
    [SerializeField] private float goldAmount;
    public int enemiesDefeated; // Used for Game Over Menu

    [Header("Health")]
    public float maxHealth;
    public float currentHealth;
    [SerializeField] private bool playerIsAlive;
    [SerializeField] public HealthbarScript healthbar;
    public bool canTakeDamage;

    [Header("References")]
    [SerializeField] private BoxCollider2D playerHitbox;
    [SerializeField] private TextMeshProUGUI goldAmountText;
    [SerializeField] private GameObject weaponHolder;

    [Header("Game Over References")]
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject timerObject;
    [SerializeField] private TextMeshProUGUI enemiesDefeatedText;
    [SerializeField] private TextMeshProUGUI goldEarnedText;
    [SerializeField] private TextMeshProUGUI levelReachedText;

    [Header("Sound Effects")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathSFX;
    [SerializeField] private AudioClip playerHitSFX;
    [SerializeField] private AudioClip collectSFX;
    [SerializeField] private AudioClip coinCollectSFX;
    [SerializeField] private AudioClip turkeySFX;
    private bool sfxCooldownBool = true;

    private Vector2 movementInput;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private LevelingManager levelingManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        levelingManager = GetComponent<LevelingManager>();

        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (playerIsAlive)
        {
            Move();
            FlipSprite();
        }
    }

    private void Move()
    {
        if (movementInput.magnitude > 1)
        {
            movementInput.Normalize();
        }

        rb.velocity = movementInput * moveSpeed * weaponHolder.GetComponent<WeaponManager>().wingsMultiplier;
    }

    private void FlipSprite()
    {
        if (movementInput.x < 0) // Moving left
        {
            spriteRenderer.flipX = true;
        }
        else if (movementInput.x > 0) // Moving right
        {
            spriteRenderer.flipX = false;
        }
    }

    // Healthbar methods
    public void TakeDamage(float damage)
    {
        StartCoroutine(sfxCooldown());

        currentHealth -= damage * weaponHolder.GetComponent<WeaponManager>().armorMultiplier;
        healthbar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            playerIsAlive = false;
            PlayerDeath();
        }
    }

    public void HealPlayer(float heal)
    {
        currentHealth += heal;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthbar.SetHealth(currentHealth);
    }

    void PlayerDeath()
    {
        audioSource.PlayOneShot(deathSFX);

        playerHitbox.enabled = false; // Stops the enemy from attacking the player
        rb.velocity = Vector2.zero; // Stops the player from moving

        Time.timeScale = 0f;
        gameOverMenu.SetActive(true);

        timerObject.GetComponent<Timer>().stopTimer = true;
        timerObject.GetComponent<Timer>().onPlayerDeath();

        enemiesDefeatedText.text = "Enemies Defeated: " + enemiesDefeated;
        goldEarnedText.text = "Gold Earned: " + goldAmount;

        levelReachedText.text = "Level Reached: " + levelingManager.level;
    }

    // Pickup for Turkey / Gold
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Turkey"))
        {
            audioSource.PlayOneShot(turkeySFX);
            Destroy(other.gameObject);
            HealPlayer(30);
        }
        else if (other.CompareTag("GoldBag"))
        {
            audioSource.PlayOneShot(coinCollectSFX);
            Destroy(other.gameObject);
            goldAmount += 10;
            goldAmountText.text = "" + goldAmount;
        }
        else if (other.CompareTag("BlueGem"))
        {
            audioSource.PlayOneShot(collectSFX);
            Destroy(other.gameObject);
            levelingManager.totalXP += weaponHolder.GetComponent<WeaponManager>().xpMultiplier * 10;
            levelingManager.UpdateXP();
            levelingManager.CheckWhenGainingXP();
        }
        else if (other.CompareTag("GreenGem"))
        {
            audioSource.PlayOneShot(collectSFX);
            Destroy(other.gameObject);
            levelingManager.totalXP += weaponHolder.GetComponent<WeaponManager>().xpMultiplier * 50;
            levelingManager.UpdateXP();
            levelingManager.CheckWhenGainingXP();
        }
        else if (other.CompareTag("RedGem"))
        {
            audioSource.PlayOneShot(collectSFX);
            Destroy(other.gameObject);
            levelingManager.totalXP += weaponHolder.GetComponent<WeaponManager>().xpMultiplier * 100;
            levelingManager.UpdateXP();
            levelingManager.CheckWhenGainingXP();
        }
    }

    // Game Over Menu Method
    public void GameOver()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }

    public void setMaxHealth()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    public IEnumerator sfxCooldown()
    {
        if(sfxCooldownBool)
        {
            sfxCooldownBool = false;
            audioSource.PlayOneShot(playerHitSFX);
            yield return new WaitForSeconds(0.2f);
            sfxCooldownBool = true;
        }
    }
}