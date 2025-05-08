using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class UnitVisualManager : MonoBehaviour
{
    private UnitData unitData;
    public UnitData UnitData
    {
        get => unitData;
        set
        {
            unitData = value;
            Unit u = GetComponent<UnitManager>().unit = new Unit(unitData, gameObject);
            LoadPartsFromData(u);
        }
    }
    public Transform root; // Parent holder for visuals
    private List<GameObject> spawnedParts = new();
    private UnitManager unit;

    private void Start()
    {
        unit = GetComponent<UnitManager>();
    }

    public void ClearParts()
    {
        if(spawnedParts.Count > 0)
        {
            foreach (var part in spawnedParts)
            {
                if (part != null) DestroyImmediate(part);
            }
            spawnedParts.Clear();
        }
        else
        {
            foreach (Transform child in root)
            {
                DestroyImmediate(child.gameObject);
            }
        }
            
    }
    /*
    public void LoadPartsFromData(Unit unit)
    {
        foreach (var part in unit.Parts)
        {
            GameObject prefab = part.data.visualPrefab;
            if (prefab)
            {
                GameObject partGO = (GameObject)PrefabUtility.InstantiatePrefab(prefab, root);
                spawnedParts.Add(partGO);

                // Optional: connect back to visual handlers
                PartVisualHandler handler = partGO.GetComponent<PartVisualHandler>();
                if (handler != null)
                    handler.Initialize(part);
            }
        }
    }
    */
    public void LoadPartsFromData(Unit unit)
    {
        ClearParts();
        Dictionary<PartType, UnitPart> partsByType = unit.Parts.ToDictionary(p => p.data.partType);

        if (!partsByType.TryGetValue(PartType.Legs, out UnitPart legs))
        {
            Debug.LogError("Legs are required to build unit base.");
            return;
        }

        GameObject legsGO = SpawnPart(legs, root);
        PartVisualHandler legsHandler = legsGO.GetComponent<PartVisualHandler>();

        // Step 2: Body
        if (!partsByType.TryGetValue(PartType.Body, out UnitPart body))
        {
            Debug.LogError("Body is required to continue building.");
            return;
        }

        Transform bodyHolder = legsHandler.GetHolderForPart(PartType.Body);
        GameObject bodyGO = SpawnPart(body, bodyHolder);
        PartVisualHandler bodyHandler = bodyGO.GetComponent<PartVisualHandler>();

        // Step 3: Arms and Head
        foreach (PartType type in new[] { PartType.Head, PartType.ArmLeft, PartType.ArmRight })
        {
            if (!partsByType.TryGetValue(type, out UnitPart part))
                continue;

            Transform holder = bodyHandler.GetHolderForPart(type);
            if (holder == null)
            {
                Debug.LogWarning($"No holder for {type} on body prefab.");
                continue;
            }

            SpawnPart(part, holder);
        }
    }

    private GameObject SpawnPart(UnitPart part, Transform parent)
    {
        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(part.data.visualPrefab);
        
        go.transform.SetParent(parent, true);
        go.transform.localPosition = Vector3.zero;
        PartVisualHandler handler = go.GetComponent<PartVisualHandler>();
        handler?.Initialize(part);
        spawnedParts.Add(go);
        return go;
    }
}
