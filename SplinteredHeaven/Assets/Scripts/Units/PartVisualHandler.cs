using FOVMapping;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartVisualHandler : MonoBehaviour
{
    public PartType partType; // Manually set in prefab or via script
    public string partId; // e.g., "LeftArm"

    private Dictionary<PartType, Transform> partHolders;

    //Current Attack Info
    public AttackInfo currentAttackInfo; // Optional, can be set by UnitCombat or other systems

    //private Dictionary<int, Transform> moduleSlots;

    public List<ModuleSlot> moduleSlots; // Optional fallback or preview list
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
        GetComponent<DamageReceiver>().Initialize(part); // Initialize damage receiver with the part
        linkedPart.OnDamageTaken += ShowDamageFeedback;
        linkedPart.OnDestroyed += HandleDestruction;
        linkedPart.transform = transform;
        linkedPart.partObject = gameObject;
        LocateModuleHolders();
        CreateModules();

        if (TryGetComponent(out FOVAgent fov))
        {
            fov.Init();
        }
        else
        {
            if (linkedPart.data.partType == PartType.Head)
            {
                Debug.LogWarning($"[PartVisualHandler] FOVAgent component not found on head part");
            }
            
        }
        
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
        public ModuleSlot slot;
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
            ModuleSlot slot;
            if (child.TryGetComponent(out ModuleSlot moduleSlot))
            {
                slot = moduleSlot;
            }
            else
            {
                if (!name.StartsWith("moduleholder_") || name.EndsWith("end")) continue;

                string[] parts = name.Split('_');
                if (parts.Length < 3) continue;

                if (!System.Enum.TryParse(parts[1], true, out ModulePositionType posType)) continue;
                if (!System.Enum.TryParse(parts[2], true, out ModuleWeightType weightType)) continue;
                slot = child.GetComponent<ModuleSlot>();
            }

            slot.SetUp(linkedPart);

            linkedPart.slots.Add(slot);

            ModuleHolderInfo holderInfo = new ModuleHolderInfo
            {
                holder = child,
                slot = slot,
                positionType = slot.modulePositionType,
                weightType = slot.moduleWeightType
            };
            parsedModuleHolders.Add(holderInfo);
        }
    }

    private void CreateModules()
    {
        for (int i = 0; i < linkedPart.Modules.Count; i++)
        {
            ModuleInstance module = linkedPart.Modules[i];
            if (module == null || module.Data == null) continue;

            ModuleHolderInfo match;
            if (module.slotInt == -1)
            {
                match = parsedModuleHolders.FirstOrDefault(h =>
                    h.positionType == module.Data.positionType &&
                    (h.weightType == module.Data.weightType || h.weightType == ModuleWeightType.Heavy && module.Data.weightType == ModuleWeightType.Light));
            }
            else
            {
                 match = parsedModuleHolders.FirstOrDefault(h => h.slot.slot == module.slotInt);
            }


            if (match == null)
            {
                Debug.LogWarning($"[PartVisualHandler] No compatible slot for module '{module.Data.name}' on part '{partId}'.");
                continue;
            }

            if (module.Data.visualPrefab != null)
            {
                module.slotInt = match.slot.slot;
                GameObject vis = Instantiate(module.Data.visualPrefab);
                
                vis.transform.SetParent(match.holder, true); // Set parent and keep local position/rotation
                vis.transform.localPosition = Vector3.zero;

                if(module.Data.positionType == ModulePositionType.Basic)
                    vis.transform.localRotation = Quaternion.Euler(0, 0, 0);
                else
                    vis.transform.localRotation = Quaternion.identity;

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
        /*
        // Detach or destroy child parts
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out PartVisualHandler childHandler))
            {
                Destroy(child.gameObject);
            }
        }
        */
        // play explosion, detach part, etc
    }

    void HandleDestruction()
    {
        switch (linkedPart.data.partType)
        {
            case PartType.Legs:
                // Handle leg destruction logic
                foreach(Transform t in GetComponentInChildren<Transform>(true))
                {
                    if (t.TryGetComponent(out BoxCollider collider))
                    {
                        collider.enabled = false; // Disable colliders to prevent further interactions
                    }
                }
                break;
            case PartType.Body:
                // Handle torso destruction logic
                foreach (Transform t in GetComponentInChildren<Transform>(true))
                {
                    if (t.TryGetComponent(out BoxCollider collider))
                    {
                        collider.enabled = false; // Disable colliders to prevent further interactions
                    }
                }
                break;
            case PartType.ArmLeft:
                // Handle arm destruction logic
                // play explosion, detach part, etc
                gameObject.SetActive(false);
                Instantiate(linkedPart.data.destructionPrefab, transform.position, transform.rotation); // Optionally instantiate a destroyed visual prefab
                break;
            case PartType.ArmRight:
                // Handle arm destruction logic
                // play explosion, detach part, etc
                gameObject.SetActive(false);
                Instantiate(linkedPart.data.destructionPrefab, transform.position, transform.rotation); // Optionally instantiate a destroyed visual prefab
                break;
            case PartType.Head:
                // Handle head destruction logic
                // play explosion, detach part, etc
                gameObject.SetActive(false);
                Instantiate(linkedPart.data.destructionPrefab, transform.position, transform.rotation); // Optionally instantiate a destroyed visual prefab
                break;
            default:
                break;
        }
        
    }

    public void SetCurrentAttackInfo(AttackInfo attackInfo)
    {
        currentAttackInfo = attackInfo;
    }

    public void StartModuleAttack()
    {
        linkedPart.Weapons.Where(n => n == currentAttackInfo.Weapon).ToList().ForEach(w => w.Activate(currentAttackInfo.Attacker, currentAttackInfo.Target, currentAttackInfo.TargetPart));

        // Reset current attack info after use
        currentAttackInfo = null;
    }

}
