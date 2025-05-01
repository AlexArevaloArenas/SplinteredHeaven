using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class WeaponData : ScriptableObject
{
    public float AttackRange;
    public float AttackSpeed;
    public float AttackDamage;
    public DamageType damageType;
    public WeaponType weaponType;
}

public enum DamageType
{
    Ballistic,
    Energy,
    Explosive
}
public enum WeaponType
{
    Melee,
    Range
}