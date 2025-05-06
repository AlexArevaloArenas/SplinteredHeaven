using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WeaponModuleInstance : ModuleInstance
{
    public new WeaponData Data { get; private set; }

    public WeaponModuleInstance(WeaponData data, Unit owner, UnitPart part) : base(data, owner, part)
    {
        Initialize(data, owner, part);
    }

    public override void Initialize(ModuleData baseData, Unit _owner, UnitPart _part)
    {
        Data = (WeaponData)baseData;
        Owner = _owner;
        AttachedPart = _part;
        Debug.Log(Data.range);
    }

    public override void Activate(UnitManager owner, UnitManager target, UnitPart targetPart = null)
    {
        Debug.Log("Attacking with weapon module");
        if (!IsAvailable()) return;

        if (Vector3.Distance(owner.transform.position, target.transform.position) > Data.range) return;

        base.Activate(owner, target, targetPart); // Calls ApplyEffects
    }

    public bool IsInRange(Vector3 origin, Vector3 target)
    {
        Debug.Log($"Checking range: {Vector3.Distance(origin, target)} <= {Data.range}");
        return Vector3.Distance(origin, target) <= Data.range;
    }
}
