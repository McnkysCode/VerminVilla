using UnityEngine;

[CreateAssetMenu(fileName = "weaponData", menuName = "Create Weapon Data", order = 1)]

public class WeaponData : ScriptableObject
{
    public int Damage;
    public int WeaponSpeed;
    public int DamageOverTime;
    public int WeaponState;
}