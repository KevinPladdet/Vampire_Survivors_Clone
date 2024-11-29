using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelingManager : MonoBehaviour
{
    public float totalXP;
    [SerializeField] private float xpRequirement;
    public float level;

    [Header("References")]
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TextMeshProUGUI levelAmountText;

    [Header("Level Up Menu")]
    [SerializeField] private GameObject chooseMenu;
    [SerializeField] private GameObject levelChoicePrefab;
    [SerializeField] private Transform choiceContainer;
    private bool onlyWeapons;

    [Header("Sound Effects")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip levelUpSFX;

    [Header("Other References")]
    [SerializeField] private GameObject gameManager;

    private void Start()
    {
        onlyWeapons = true;
        audioSource.PlayOneShot(levelUpSFX);
        level += 1f;
        levelAmountText.text = "Lv " + level;
        ShowLevelUpChoices();
        onlyWeapons = false;
    }

    private void LevelUp()
    {
        audioSource.PlayOneShot(levelUpSFX);
        totalXP -= xpRequirement; // Removes the XP it costed to level up (to "reset" the XP)
        xpRequirement *= 1.5f; // Increase xpRequirement by 50%
        xpSlider.maxValue = xpRequirement; // Set XP Slider maxValue to the new increased xpRequirement
        xpSlider.value = totalXP; // Update XP Slider with totalXP value
        level += 1f;
        levelAmountText.text = "Lv " + level;
        StartCoroutine(WaitForLevelUp()); // Checks if you can level up again
        ShowLevelUpChoices(); // Enables chooseMenu and spawns 3 levelChoices
    }

    private void ShowLevelUpChoices()
    {
        gameManager.GetComponent<GameManager>().canPauseGame = false;
        Time.timeScale = 0f;
        chooseMenu.SetActive(true);

        // Clear previous levelChoices
        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }

        List<Weapon> availableOptions = weaponManager.GetRandomOptions(3, onlyWeapons);
        float yOffset = -200f;

        for (int i = 0; i < availableOptions.Count; i++)
        {
            GameObject choice = Instantiate(levelChoicePrefab, choiceContainer);
            LevelChoiceUI choiceUI = choice.GetComponent<LevelChoiceUI>();
            // Fills in the itemName, itemDescription and itemImage + passes weaponManager
            choiceUI.Initialize(availableOptions[i], weaponManager);

            // Change Y position to make them spawn under eachother with an offset of 200
            RectTransform rectTransform = choice.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, i * yOffset);
        }
    }

    public void CheckWhenGainingXP()
    {
        if (totalXP >= xpRequirement)
        {
            LevelUp();
        }
    }

    public void UpdateXP()
    {
        xpSlider.value = totalXP;
    }

    private IEnumerator WaitForLevelUp()
    {
        yield return new WaitForSeconds(0.1f);
        CheckWhenGainingXP();
    }
}