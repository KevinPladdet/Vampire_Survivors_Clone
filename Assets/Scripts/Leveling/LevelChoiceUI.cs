using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelChoiceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private Image itemImage;

    private Weapon weaponData;
    private WeaponManager weaponManager;

    [Header("Other References")]
    [SerializeField] private GameObject gameManager;

    public void Initialize(Weapon weapon, WeaponManager manager)
    {
        weaponData = weapon;
        weaponManager = manager;
        itemNameText.text = weapon.weaponName;
        itemDescriptionText.text = weapon.weaponDescription;
        itemImage.sprite = weapon.weaponImage;
    }

    public void OnSelect()
    {
        Time.timeScale = 1f;
        GameManager.instance.canPauseGame = true;

        if (weaponData.isWeapon)
        {
            GameObject weaponPrefab = weaponManager.GetWeaponPrefab(weaponData.weaponName);
            if (weaponPrefab != null)
            {
                weaponManager.SelectWeapon(weaponPrefab);
            }
        }
        else
        {
            weaponManager.SelectPassive(weaponData.weaponName);
        }
        
        weaponManager.chooseMenu.SetActive(false);
    }
}