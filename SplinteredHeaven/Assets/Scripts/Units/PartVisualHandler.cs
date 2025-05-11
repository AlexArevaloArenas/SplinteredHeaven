using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartVisualHandler : MonoBehaviour
{
    public PartType partType; // Manually set in prefab or via script
    public string partId; // e.g., "LeftArm"

    private Dictionary<PartType, Transform> partHolders;
    private Dictionary<int, Transform> moduleSlots;

    public List<Transform> moduleHolders; // Optional fallback or preview list
    public UnitPart linkedPart;
    public Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        SetUpHolders();
    }


    public void Initialize(UnitPart part)
    {
        linkedPart = part;
        linkedPart.OnDamageTaken += ShowDamageFeedback;
        linkedPart.OnDestroyed += HandleDestruction;
        LocateModuleHolders();
        CreateModules();
    }

    public void SetUpHolders()
    {
        partHolders = new Dictionary<PartType, Transform>();
        partHolders.Clear();

        // Discover holders for child parts and modules
        foreach (Transform child in transform.GetComponentsInChildren<Transform>(true))
        {
            string name = child.name.ToLower();

            // Part holders (other body parts to be attached)
            if (name.EndsWith("holder_head"))
                partHolders[PartType.Head] = child;
            else if (name.EndsWith("holder_rightarm"))
                partHolders[PartType.ArmRight] = child;
            else if (name.EndsWith("holder_leftarm"))
                partHolders[PartType.ArmLeft] = child;
            else if (name.EndsWith("holder_chest"))
                partHolders[PartType.Body] = child;

            /*
            // Module slots based on GameObject name (e.g., Slot_0, Slot_1...)
            if (name.StartsWith("slot_"))
            {
                string[] split = name.Split('_');
                if (split.Length > 1 && int.TryParse(split[1], out int index))
                {
                    if (!moduleSlots.ContainsKey(index))
                        moduleSlots[index] = child;
                }
            }
            */
        }
    }

    public Transform GetHolderForPart(PartType type)
    {
        return partHolders.TryGetValue(type, out var t) ? t : null;
    }

    private class ModuleHolderInfo
    {
        public Transform holder;
        public ModulePositionType positionType;
        public ModuleWeightType weightType;
    }

    private List<ModuleHolderInfo> parsedModuleHolders = new();

    private void LocateModuleHolders()
    {
        parsedModuleHolders.Clear();

        foreach (Transform child in transform.GetComponentsInChildren<Transform>(true))
        {
            string name = child.name.ToLower();

            if (!name.StartsWith("moduleholder_") || name.EndsWith("end")) continue;

            string[] parts = name.Split('_');
            if (parts.Length < 3) continue;

            if (!System.Enum.TryParse(parts[1], true, out ModulePositionType posType)) continue;
            if (!System.Enum.TryParse(parts[2], true, out ModuleWeightType weightType)) continue;

            parsedModuleHolders.Add(new ModuleHolderInfo
            {
                holder = child,
                positionType = posType,
                weightType = weightType
            });
        }
    }

    private void CreateModules()
    {
        for (int i = 0; i < linkedPart.Modules.Count; i++)
        {
            ModuleInstance module = linkedPart.Modules[i];
            if (module == null || module.Data == null) continue;

            // Find a compatible holder
            var match = parsedModuleHolders.FirstOrDefault(h =>
                h.positionType == module.Data.positionType &&
                (h.weightType == module.Data.weightType || h.weightType == ModuleWeightType.Heavy && module.Data.weightType == ModuleWeightType.Light));

            if (match == null)
            {
                Debug.LogWarning($"[PartVisualHandler] No compatible slot for module '{module.Data.name}' on part '{partId}'.");
                continue;
            }

            if (module.Data.visualPrefab != null)
            {
                GameObject vis = Instantiate(module.Data.visualPrefab);
                //vis.transform.localRotation = Quaternion.identity;
                vis.transform.SetParent(match.holder, true); // Set parent and keep local position/rotation
                vis.transform.localPosition = Vector3.zero;

                if (vis.TryGetComponent(out ModuleVisualHandler visualHandler))
                    visualHandler.Initialize(module);
            }
        }
    }

    /*
    private void CreateModules()
    {
        for (int i = 0; i < linkedPart.Modules.Count; i++)
        {
            ModuleInstance module = linkedPart.Modules[i];
            if (module == null || module.Data == null) continue;

            if (moduleSlots.TryGetValue(i, out Transform slot))
            { 
                if (module.Data.visualPrefab != null)
                {
                    GameObject vis = Instantiate(module.Data.visualPrefab, slot);
                    vis.transform.localPosition = Vector3.zero;
                    vis.transform.localRotation = Quaternion.identity;

                    if (vis.TryGetComponent(out ModuleVisualHandler visualHandler))
                        visualHandler.Initialize(module);
                }
            }
            else
            {
                Debug.LogWarning($"[PartVisualHandler] No slot found for module index {i} on part {partId}.");
            }
        }
    }
    */

    private void Update()
    {
        if (partType == PartType.Legs)
        {

        }
    }
    void ShowDamageFeedback(float damage)
    {
        // Detach or destroy child parts
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out PartVisualHandler childHandler))
            {
                Destroy(child.gameObject);
            }
        }

        // play explosion, detach part, etc
    }

    void HandleDestruction()
    {
        // play explosion, detach part, etc
    }
}
