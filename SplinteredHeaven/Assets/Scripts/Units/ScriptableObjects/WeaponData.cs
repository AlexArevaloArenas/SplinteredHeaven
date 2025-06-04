using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class WeaponData : ModuleData
{
    public override ModuleInstance CreateInstance(Unit owner, UnitPart part) => new WeaponModuleInstance(this, owner, part);
    public override ModuleInstance CreateInstanceFromRuntime(Unit owner, UnitPart part, RuntimeModuleData runtimeModule) => new WeaponModuleInstance(this, owner, part, runtimeModule);
    public float damage;
    public float range;
    public DamageType damageType;
    public WeaponType weaponType;

    public override void ApplyEffects(UnitManager user, UnitManager target, UnitPart part, Transform attackPoint)
    {
        if (part == null || part.currentHealth <= 0){
            foreach(var p in target.unit.Parts)
            {
                if(p == null) continue; // Skip null parts
                if (p.currentHealth > 0)
                {
                    part = p;
                    break;
                }
            }
        }

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