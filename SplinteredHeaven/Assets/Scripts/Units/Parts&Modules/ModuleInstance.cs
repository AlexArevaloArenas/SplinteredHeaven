using UnityEngine;

public class ModuleInstance
{
    public virtual ModuleData Data { get; protected set; }
    public Unit Owner { get; protected set; }
    public UnitPart AttachedPart { get; protected set; }
    public int slotInt;
    public Transform origin;
    protected float cooldownRemaining =0;
    public bool disabled = false;

    public ModuleInstance(ModuleData data, Unit owner, UnitPart part)
    {
        Initialize(data, owner, part);
        slotInt = -1;
    }

    public ModuleInstance(ModuleData data, Unit owner, UnitPart part, RuntimeModuleData moduleRuntime)
    {
        Initialize(data, owner, part);
        slotInt = moduleRuntime.slotIndex;
    }
    public virtual void Initialize(ModuleData data, Unit owner, UnitPart part)
    {
        Data = data;
        Owner = owner;
        AttachedPart = part;
        if (part.transform != null)
        {
            origin = part.transform;
        }
        
    }

    public virtual void Tick(float deltaTime)
    {
        if (cooldownRemaining > 0f)
            cooldownRemaining -= deltaTime;
    }

    public bool IsAvailable() => cooldownRemaining <= 0f && AttachedPart.currentHealth > 0 && !disabled;

    public void Disable()
    {
        disabled = true;
    }

    public virtual void Activate(UnitManager owner, UnitManager target, UnitPart targetPart = null)
    {
        if (!IsAvailable()) return;

        Data.ApplyEffects(owner, target, targetPart, origin);
        cooldownRemaining = Data.cooldown;
    }
}
