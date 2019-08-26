using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponScript : ScriptableObject
{
    public enum Wep_Type {Firearm, Melee}

    public Wep_Type type; 
    public string Weapon_Name;
    public GameObject Weapon_Model;
}

