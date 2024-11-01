using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WeaponManager : MonoBehaviour
{
    // Weapon prefabs that will be spawned under WeaponHolder
    [SerializeField] private List<GameObject> weapons = new List<GameObject>();

    [SerializeField] private List<Weapon> weaponsSO = new List<Weapon>(); // Scriptable objects for weapons
    [SerializeField] private List<Weapon> passivesSO = new List<Weapon>(); // Scriptable objects for passives

    // Passive Upgrades
    public float spinachMultiplier = 1f;
    public float wingsMultiplier = 1f;
    public float xpMultiplier = 1f;
    public float armorMultiplier = 1f;
    public float maxLifeMultiplier = 1f;
    public float projectileSpeedMultiplier = 1f;

    [SerializeField] private List<Image> weaponImages = new List<Image>();
    [SerializeField] private List<Image> passiveImages = new List<Image>();

    [Header("Other References")]
    [SerializeField] private GameObject weaponHolder;  // Holder where the weapons are spawned under
    [SerializeField] private GameObject player;
    public GameObject chooseMenu;

    // These lists are used to track which weapons / passives are already selected
    private List<GameObject> selectedWeapons = new List<GameObject>();
    private List<string> selectedPassives = new List<string>();

    public List<Weapon> GetRandomOptions(int count, bool onlyWeapons)
    {
        // Only chooses weapons if "onlyWeapons" is true, otherwise chooses both weapons and passives
        List<Weapon> allOptions = onlyWeapons ? new List<Weapon>(weaponsSO) : new List<Weapon>(weaponsSO.Concat(passivesSO));
        
        // Removes weapons/passives that have already been selected before
        // It's a very long line because it also filters out spacebars in the name to find the weapons
        allOptions = allOptions.Where(option => !selectedWeapons.Any(w => w.name.Replace(" ", "") == option.weaponName.Replace(" ", "")) && !selectedPassives.Contains(option.weaponName)).ToList();

        // If there are less than 3 items it will adjust availableCount
        int availableCount = Mathf.Min(count, allOptions.Count);
        List<Weapon> randomChoices = new List<Weapon>();

        // Randomly selects weapons/passives without duplicates
        while (randomChoices.Count < availableCount && allOptions.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, allOptions.Count);
            randomChoices.Add(allOptions[randomIndex]);
            allOptions.RemoveAt(randomIndex); // Prevents duplicates in choices
        }

        return randomChoices;
    }

    public void SelectWeapon(GameObject selectedWeapon)
    {
        if (selectedWeapons.Contains(selectedWeapon))
        {
            Debug.Log("This weapon is already selected.");
            return;
        }

        // Instantiate the weapon and add it to the weapon holder
        Instantiate(selectedWeapon, weaponHolder.transform);
        selectedWeapons.Add(selectedWeapon);

        // Update weapon image in the UI
        for (int i = 0; i < weaponImages.Count; i++)
        {
            if (!weaponImages[i].enabled)
            {
                weaponImages[i].sprite = GetWeaponSprite(selectedWeapon.name);
                weaponImages[i].enabled = true;
                break;
            }
        }
    }

    // Method to handle passive selection
    public void SelectPassive(string passiveName)
    {
        if (selectedPassives.Contains(passiveName))
        {
            Debug.Log("This passive is already selected.");
            return;
        }

        // Add passive to the list and increase the multiplier
        selectedPassives.Add(passiveName);
        ChangePassiveMultiplier(passiveName);

        // Update passive image in the UI
        for (int i = 0; i < passiveImages.Count; i++)
        {
            if (!passiveImages[i].enabled)
            {
                passiveImages[i].sprite = GetPassiveSprite(passiveName);
                passiveImages[i].enabled = true;
                break;
            }
        }
    }

    // Change the passive multiplier by 10% for the selected passive
    void ChangePassiveMultiplier(string passive)
    {
        switch (passive)
        {
            case "Spinach":
                spinachMultiplier += 0.1f;
                break;
            case "Wings":
                wingsMultiplier += 0.1f;
                break;
            case "XP Badge":
                xpMultiplier += 0.1f;
                break;
            case "Armor":
                armorMultiplier -= 0.1f;
                break;
            case "Max Life":
                maxLifeMultiplier += 0.1f;
                player.GetComponent<PlayerMovement>().maxHealth *= maxLifeMultiplier;
                player.GetComponent<PlayerMovement>().setMaxHealth();
                break;
            case "Book of Wisdom":
                projectileSpeedMultiplier += 0.1f;
                break;
        }
    }

    public GameObject GetWeaponPrefab(string weaponName)
    {
        string normalizedWeaponName = weaponName.Replace(" ", ""); // Removes spaces in weaponName

        foreach (GameObject weapon in weapons)
        {
            // Removes spaces in weapon and checks if its the same as the normalizedWeaponName
            if (weapon.name.Replace(" ", "") == normalizedWeaponName)
            {
                return weapon;
            }
        }

        Debug.LogWarning("Weapon prefab not found for: " + weaponName); // Not happy whenever this occurs >:(
        return null;
    }

    // Helper method to get the correct sprite for a selected weapon
    Sprite GetWeaponSprite(string weaponName)
    {
        string normalizedWeaponName = weaponName.Replace(" ", "");

        // Finds the weapon using LINQ and normalizes the name
        Weapon weapon = weaponsSO.FirstOrDefault(w => w.weaponName.Replace(" ", "") == normalizedWeaponName);
        return weapon != null ? weapon.weaponImage : null;
    }

    // Helper method to get the correct sprite for a selected passive
    Sprite GetPassiveSprite(string passiveName)
    {
        // Finds the weapon using LINQ and normalizes the name
        Weapon passive = passivesSO.FirstOrDefault(p => p.weaponName.Replace(" ", "") == passiveName.Replace(" ", ""));
        return passive != null ? passive.weaponImage : null;
    }
}