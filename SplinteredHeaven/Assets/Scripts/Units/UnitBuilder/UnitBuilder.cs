using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class UnitRuntimeData
{
    public int version = 1;
    public string unitDataID;
    public string unitNameOverride;
    public List<PartRuntimeData> parts = new();
}

[System.Serializable]
public class PartRuntimeData
{
    public string partID;
    public float currentHealth;
    public List<RuntimeModuleData> modules = new();
}

[System.Serializable]
public class RuntimeModuleData
{
    public string moduleID;   // maps to ScriptableObject
    public int slotIndex;     // optional: which module slot this was in
}

public static class MechaBuilder
{
    public static string defaultDirectory = "Assets/Resources/UnitJSON/";
    public static UnitRuntimeData CreateRuntimeDataFromUnit(Unit unit)
    {
        var runtime = new UnitRuntimeData
        {
            unitDataID = unit.unitData?.id ?? "",
            unitNameOverride = unit.name,
            parts = new List<PartRuntimeData>()
        };

        foreach (var part in unit.Parts)
        {
            var runtimePart = new PartRuntimeData
            {
                partID = part.data.id,
                currentHealth = part.currentHealth,
                modules = new List<RuntimeModuleData>()
            };

            for (int i = 0; i < part.Modules.Count; i++)
            {
                var module = part.Modules[i];
                if (module?.Data == null) continue;

                runtimePart.modules.Add(new RuntimeModuleData
                {
                    moduleID = module.Data.id,
                    slotIndex = i
                });
            }

            runtime.parts.Add(runtimePart);
        }

        return runtime;
    }

    public static Unit CreateUnitFromRuntimeData(UnitRuntimeData data, RuntimeAssetRegistry registry, GameObject owner)
    {
        UnitData unitData = registry.allUnits.FirstOrDefault(d => d.id == data.unitDataID);


        if (unitData == null)
        {
            Debug.LogError($"UnitData with ID '{data.unitDataID}' not found in registry.");
            return null;
        }

        Unit unit = new Unit(data, unitData, registry);
        return unit;
    }

    public static void SaveToJson(UnitRuntimeData data, string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName + ".json");
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log($"Mecha saved to {path}");
    }

    public static void SaveMecha(Unit unit)
    {
        UnitRuntimeData unitRuntimeData = CreateRuntimeDataFromUnit(unit);
        string fileName = unit.unitData.id;
        SaveToJson(unitRuntimeData, fileName);
    }

    public static UnitRuntimeData LoadFromJson(string fileName)
    {
        //string path = Path.Combine(Application.persistentDataPath, fileName + ".json");
        string path = Path.Combine(defaultDirectory, fileName + ".json");
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Save file not found: {path}");
            return null;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<UnitRuntimeData>(json);
    }

    public static string[] ListSavedMechas()
    {
        //string dir = Application.persistentDataPath;
        string dir = defaultDirectory;
        if (!Directory.Exists(dir)) return Array.Empty<string>();

        return Directory.GetFiles(dir, "*.json")
            .Select(Path.GetFileNameWithoutExtension)
            .ToArray();
    }

    public static void CreateMechasFromUnitData(UnitData[] unitsData, RuntimeAssetRegistry registry)
    {
        foreach (UnitData unitData in unitsData)
        {
            Unit unitInstance = new Unit(unitData, null);
            var runtimeData = CreateRuntimeDataFromUnit(unitInstance);
            string fileName = unitData.name;
            SaveToJson(runtimeData, fileName);
            Debug.Log($"Mecha created and saved as {fileName}");
        }
    }

    public static void SaveMechaFromUnitData(UnitData unitData, RuntimeAssetRegistry registry)
    {
        Unit unitInstance = new Unit(unitData, null);
        var runtimeData = CreateRuntimeDataFromUnit(unitInstance);
        string fileName = unitData.name;
        SaveToJson(runtimeData, fileName);
        Debug.Log($"Mecha created and saved as {fileName}");
    }
}
