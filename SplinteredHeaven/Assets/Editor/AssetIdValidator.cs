#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class AssetIdValidator : EditorWindow
{
    [MenuItem("Tools/Validate Asset IDs")]
    public static void ValidateAll()
    {
        var units = AssetDatabase.FindAssets("t:UnitData")
            .Select(guid => AssetDatabase.LoadAssetAtPath<UnitData>(AssetDatabase.GUIDToAssetPath(guid)))
            .ToList();

        var parts = AssetDatabase.FindAssets("t:UnitPartData")
            .Select(guid => AssetDatabase.LoadAssetAtPath<UnitPartData>(AssetDatabase.GUIDToAssetPath(guid)))
            .ToList();

        var modules = AssetDatabase.FindAssets("t:ModuleData")
            .Select(guid => AssetDatabase.LoadAssetAtPath<ModuleData>(AssetDatabase.GUIDToAssetPath(guid)))
            .ToList();

        var duplicatedUnits = units.GroupBy(p => p.id).Where(g => g.Count() > 1 && !string.IsNullOrEmpty(g.Key));
        var duplicateParts = parts.GroupBy(p => p.id).Where(g => g.Count() > 1 && !string.IsNullOrEmpty(g.Key));
        var duplicateModules = modules.GroupBy(m => m.id).Where(g => g.Count() > 1 && !string.IsNullOrEmpty(g.Key));

        foreach (var group in duplicatedUnits)
            Debug.LogError($"Duplicate Unit ID: {group.Key}");

        foreach (var group in duplicateParts)
            Debug.LogError($"Duplicate Part ID: {group.Key}");

        foreach (var group in duplicateModules)
            Debug.LogError($"Duplicate Module ID: {group.Key}");

        var emptyUnits = units.Where(u => string.IsNullOrEmpty(u.id));
        var emptyParts = parts.Where(p => string.IsNullOrEmpty(p.id));
        var emptyModules = modules.Where(m => string.IsNullOrEmpty(m.id));

        foreach (var unit in emptyUnits)
            Debug.LogWarning($"Missing Unit ID: {unit.name}");
        foreach (var part in emptyParts)
            Debug.LogWarning($"Missing Part ID: {part.name}");

        foreach (var mod in emptyModules)
            Debug.LogWarning($"Missing Module ID: {mod.name}");

        Debug.Log("Validation complete.");
    }

    [MenuItem("Tools/Populate Asset Registry")]
    public static void PopulateRegistry()
    {
        string[] registryGuids = AssetDatabase.FindAssets("t:RuntimeAssetRegistry");
        if (registryGuids.Length == 0)
        {
            Debug.LogError("No RuntimeAssetRegistry found.");
            return;
        }

        string registryPath = AssetDatabase.GUIDToAssetPath(registryGuids[0]);
        var registry = AssetDatabase.LoadAssetAtPath<RuntimeAssetRegistry>(registryPath);

        registry.allUnits = AssetDatabase.FindAssets("t:UnitData")
            .Select(guid => AssetDatabase.LoadAssetAtPath<UnitData>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(u => u != null).ToList();

        registry.allParts = AssetDatabase.FindAssets("t:UnitPartData")
            .Select(guid => AssetDatabase.LoadAssetAtPath<UnitPartData>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(p => p != null).ToList();

        registry.allModules = AssetDatabase.FindAssets("t:ModuleData")
            .Select(guid => AssetDatabase.LoadAssetAtPath<ModuleData>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(m => m != null).ToList();

        EditorUtility.SetDirty(registry);
        AssetDatabase.SaveAssets();
        Debug.Log("Asset Registry populated successfully.");
    }
}
#endif