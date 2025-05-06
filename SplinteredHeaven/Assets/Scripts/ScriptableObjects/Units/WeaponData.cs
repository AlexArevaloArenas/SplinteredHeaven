using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class WeaponData : ModuleData
{
    public float damage;
    public float range;
    public DamageType damageType;
    public WeaponType weaponType;

    public override void ApplyEffects(UnitManager user, UnitManager target, UnitPart part)
    {
        if (part == null || part.currentHealth <= 0) return;

        Debug.Log($"[Weapon] {user.name} hits {target.name} at {part.name} for {damage} damage");
        part.TakeDamage(damage);
    }
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