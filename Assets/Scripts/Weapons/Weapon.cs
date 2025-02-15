using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public string weaponDescription;
    public Sprite weaponImage;

    public float weaponDamage;
    
    public bool isWeapon;
}