using UnityEngine;

public class Weapon
{
    private bool Ready;
    private float cooldown;
    private float AttackRange;
    private float AttackSpeed;
    private float AttackDamage;
    private DamageType damageType;
    private WeaponType weaponType;

    public bool IsReady => Ready;
    public float GetAttackRange => AttackRange;
    public float GetAttackSpeed => AttackSpeed;
    public float GetAttackDamage => AttackDamage;
    public DamageType GetDamageType => damageType;
    public WeaponType GetWeaponType => weaponType;

    public Weapon(WeaponData weaponData)
    {
        AttackRange = weaponData.AttackRange;
        AttackSpeed = weaponData.AttackSpeed;
        AttackDamage = weaponData.AttackDamage;
        damageType = weaponData.damageType;
        weaponType = weaponData.weaponType;
        Ready = true;
    }
}
