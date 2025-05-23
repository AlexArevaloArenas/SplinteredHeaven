using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetRegistry", menuName = "Game/Asset Registry")]
public class RuntimeAssetRegistry : ScriptableObject
{
    [Header("Runtime Asset Registry")]
    [Tooltip("All Unit Parts in the game.")]
    public List<UnitData> allUnits;
    public List<UnitPartData> allParts;
    public List<ModuleData> allModules;

    [Header("Runtime Asset Registry Lookup")]
    [Tooltip("Lookup for Unit Parts.")]
    public Dictionary<string, UnitData> unitLookup;
    private Dictionary<string, UnitPartData> partLookup;
    private Dictionary<string, ModuleData> moduleLookup;

    private void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        unitLookup = new Dictionary<string, UnitData>();
        foreach (var unit in allUnits)
        {
            if (unit != null && !string.IsNullOrEmpty(unit.id))
                unitLookup[unit.id] = unit;
        }

        partLookup = new Dictionary<string, UnitPartData>();
        foreach (var part in allParts)
        {
            if (part != null && !string.IsNullOrEmpty(part.id))
                partLookup[part.id] = part;
        }

        moduleLookup = new Dictionary<string, ModuleData>();
        foreach (var mod in allModules)
        {
            if (mod != null && !string.IsNullOrEmpty(mod.id))
                moduleLookup[mod.id] = mod;
        }
    }

    public UnitData GetUnit(string id)
    {
        return unitLookup.TryGetValue(id, out var unit) ? unit : null;
    }
    public UnitPartData GetPart(string id)
    {
        return partLookup.TryGetValue(id, out var part) ? part : null;
    }

    public ModuleData GetModule(string id)
    {
        return moduleLookup.TryGetValue(id, out var module) ? module : null;
    }

}