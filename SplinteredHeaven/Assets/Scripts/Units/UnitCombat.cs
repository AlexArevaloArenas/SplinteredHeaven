using System.Collections.Generic;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    private UnitManager unitM;
    private TargetTracker targetTracker;
    private List<WeaponModuleInstance> weapons = new List<WeaponModuleInstance>();

    [SerializeField] private float attackInterval = 1.0f;
    private float attackCooldown = 0f;

    void Start()
    {
        unitM = GetComponent<UnitManager>();
        targetTracker = GetComponent<TargetTracker>();

        // Gather all WeaponModules from UnitParts
        foreach (UnitPart part in unitM.unit.Parts)
        {
            //Debug.Log($"Part: {part.name}");
            foreach (ModuleInstance module in part.Modules)
            {
                //Debug.Log($"Module: {module.Data.name}");
                if (module is WeaponModuleInstance weapon)
                {
                    //Debug.Log($"Module: {weapon.Data.name}");
                    weapons.Add(weapon);
                }
                    
            }
        }
    }

    void Update()
    {
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
            return;
        }

        TryAttack();
        attackCooldown = attackInterval;
    }

    private void TryAttack()
    {
        if (!targetTracker.HasValidTarget()) return;
        Debug.Log("Trying to attack...");

        var targetUnit = targetTracker.CurrentTargetUnit;
        var targetPart = targetTracker.CurrentTargetPart;

        foreach (var weapon in weapons)
        {
            Debug.Log($"Checking weapon: {weapon.Data.name}");
            if (weapon.IsInRange(transform.position, targetPart.transform.position))
            {
                Debug.Log("Attacking...");
                weapon.Activate(unitM, targetUnit, targetPart);
            }
        }
    }
}
