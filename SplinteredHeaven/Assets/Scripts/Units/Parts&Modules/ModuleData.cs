using UnityEngine;

[CreateAssetMenu(fileName = "NewModule", menuName = "Module/Module Data")]
public abstract class ModuleData : ScriptableObject
{
    public virtual ModuleInstance CreateInstance(Unit owner, UnitPart part) => new ModuleInstance(this, owner, part);
    public virtual ModuleInstance CreateInstanceFromRuntime(Unit owner, UnitPart part, RuntimeModuleData runtimeModule) => new ModuleInstance(this, owner, part, runtimeModule);

    public string id;
    public Sprite icon;
    public GameObject visualPrefab;
    public float cooldown;

    public ModulePositionType positionType;
    public ModuleWeightType weightType;
    public ModuleEffectType effectType;

    // Generalized effect interface — override in subclass
    public virtual void ApplyEffects(UnitManager user, UnitManager target, UnitPart targetPart, Transform origin)
    {
        // Default: no-op (or log for debug)
        Debug.Log($"Module {id} activated but has no effect.");
    }
}

public enum ModulePositionType { Basic, Hand }
public enum ModuleWeightType { Light, Heavy }

public enum ModuleEffectType { Self, Target }