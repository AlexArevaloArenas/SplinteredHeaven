using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    private UnitManager unitM;
    private TargetTracker targetTracker;
    private List<WeaponModuleInstance> weapons = new List<WeaponModuleInstance>();

    [SerializeField] private float attackInterval = 1.0f;
    private float attackCooldown = 1f;
    private List<AimingController> aimingControllers;

    void Start()
    {
        unitM = GetComponent<UnitManager>();
        targetTracker = GetComponent<TargetTracker>();
        aimingControllers = new List<AimingController>(GetComponentsInChildren<AimingController>());

        // Gather all WeaponModules from UnitParts
        foreach (UnitPart part in unitM.unit.Parts)
        {
            if (part == null) continue; // Skip null parts
            if (part.Weapons == null || part.Weapons.Count == 0) continue; // Skip parts without weapons
            weapons.AddRange(part.Weapons);
        }

        WeaponModuleInstance weaponWithMoreRange = null;
        float maxRange = 0;
        foreach (var weapon in weapons)
        {
            if (weapon.GetRange() > maxRange)
            {
                weaponWithMoreRange = weapon;
            }
        }
        targetTracker.weaponWithMoreRange = weaponWithMoreRange;
    }

    void Update()
    {
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
            return;
        }

        if (targetTracker.CurrentTargetUnit == null)
        {
            //Debug.Log("No valid target found, skipping attack.");
            foreach (var aimer in aimingControllers)
            {
                aimer.SetTarget(null);
            }
            StopAim(GetAvaliableWeapons());

            return;
        }
        else
        {
            foreach (var aimer in aimingControllers)
            {
                aimer.SetTarget(targetTracker.CurrentTargetUnit.transform);
                TryAim(GetAvaliableWeapons());
            }

            TryAttack(GetAvaliableWeapons());
        }

        
        attackCooldown = attackInterval;
    }

    private WeaponModuleInstance[] GetAvaliableWeapons()
    {

        List<WeaponModuleInstance> avaliableWeapons = new List<WeaponModuleInstance>();
        foreach (var weapon in weapons)
        {
            if (weapon.IsAvailable())
            {
                avaliableWeapons.Add(weapon);
            }
        }

        return avaliableWeapons.ToArray();
    }

    private void TryAttack(WeaponModuleInstance[] avaliableWeapons)
    {

        if (!targetTracker.HasValidTarget()) return;

        var targetUnit = targetTracker.CurrentTargetUnit;
        var targetPart = targetTracker.CurrentTargetPart;

        if (targetUnit == null || targetPart == null)
        {
            Debug.LogWarning("Target unit or part is null, skipping attack.");
            return;
        }

        foreach (var weapon in avaliableWeapons)
        {

            Debug.Log($"Checking weapon: {weapon.Data.name}");
            WeaponData weaponData = weapon.Data as WeaponData;
            if (weaponData.animatedAttack)
            {
                weapon.AttachedPart.transform.GetComponent<PartVisualHandler>().SetCurrentAttackInfo(new AttackInfo(unitM, targetUnit, weapon, targetPart));
                //weapon.Activate(unitM, targetUnit, targetPart);

                weapon.AttachedPart.transform.GetComponent<PartAnimationManager>().SetAnimationTrigger("Shoot");
            }
            else
            {
                weapon.Activate(unitM, targetUnit, targetPart);
            }

            

        }

    }

    public void TryAim(WeaponModuleInstance[] avaliableWeapons)
    {
        foreach (var weapon in avaliableWeapons)
        {
            weapon.AttachedPart.transform.GetComponent<PartAnimationManager>().SetAnimationBool("Aiming", true);


        }

    }
    public void StopAim(WeaponModuleInstance[] avaliableWeapons)
    {
        foreach (var weapon in avaliableWeapons)
        {
            weapon.AttachedPart.transform.GetComponent<PartAnimationManager>().SetAnimationBool("Aiming", false);


        }

    }

}

public class AttackInfo
{
    public UnitManager Attacker { get; private set; }
    public UnitManager Target { get; private set; }
    public WeaponModuleInstance Weapon { get; private set; }
    public UnitPart TargetPart { get; private set; }
    public AttackInfo(UnitManager attacker, UnitManager target, WeaponModuleInstance weapon, UnitPart targetPart)
    {
        Attacker = attacker;
        Target = target;
        Weapon = weapon;
        TargetPart = targetPart;
    }
}
