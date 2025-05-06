using UnityEngine;

[CreateAssetMenu(fileName = "NewModule", menuName = "Module/Module Data")]
public class ModuleData : ScriptableObject
{
    public string moduleName;
    public GameObject visualPrefab;
    public float cooldown;

    // Generalized effect interface — override in subclass
    public virtual void ApplyEffects(UnitManager user, UnitManager target, UnitPart targetPart)
    {
        // Default: no-op (or log for debug)
        Debug.Log($"Module {moduleName} activated but has no effect.");
    }
}
