using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WeaponModuleInstance : ModuleInstance
{
    public override ModuleData Data => weaponData;

    private WeaponData weaponData;


    public WeaponModuleInstance(WeaponData data, Unit owner, UnitPart part) : base(data, owner, part)
    {
        weaponData = data;
        Owner = owner;
        AttachedPart = part;
        slotInt = -1;
        //Initialize(data, owner, part);
    }

    public WeaponModuleInstance(WeaponData data, Unit owner, UnitPart part, RuntimeModuleData runtime) : base(data, owner, part, runtime)
    {
        weaponData = data;
        Owner = owner;
        AttachedPart = part;
        slotInt = runtime.slotIndex;
        //Initialize(data, owner, part);
    }

    public override void Initialize(ModuleData baseData, Unit _owner, UnitPart _part)
    {
        weaponData = (WeaponData)baseData;
        Owner = _owner;
        AttachedPart = _part;
        //Debug.Log(Data.range);
    }
    
    public override void Activate(UnitManager owner, UnitManager target, UnitPart targetPart = null)
    {
        if (target == null)
        {
            return;
        }

        Debug.Log("Attacking with weapon module");
        if (!IsAvailable()) return;

        if (Vector3.Distance(owner.transform.position, target.transform.position) > weaponData.range) return;

        base.Activate(owner, target, targetPart); // Calls ApplyEffects
    }

    public bool IsInRange(Vector3 origin, Vector3 target)
    {
        Debug.Log($"Checking range: {Vector3.Distance(origin, target)} <= {weaponData.range}");
        return Vector3.Distance(origin, target) <= weaponData.range;
    }

    public float GetRange()
    {
        return weaponData.range;
    }
}
