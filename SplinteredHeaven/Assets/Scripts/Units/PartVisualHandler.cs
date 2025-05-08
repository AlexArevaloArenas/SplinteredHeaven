using System.Collections.Generic;
using UnityEngine;

public class PartVisualHandler : MonoBehaviour
{
    public PartType partType; // Manually set in prefab or via script
    private Dictionary<PartType, Transform> partHolders;
    private UnitPart linkedPart;
    private Dictionary<int, Transform> moduleSlots;
    public string partId; // e.g., "LeftArm"

    public List<Transform> moduleHolders;
    void Awake()
    {
        
    }


    public void Initialize(UnitPart part)
    {
        partHolders = new Dictionary<PartType, Transform>();
        partHolders.Clear();

        foreach (Transform child in transform.GetComponentsInChildren<Transform>(true))
        {
            string name = child.name.ToLower();
            if (name.EndsWith("holder_head"))
                partHolders[PartType.Head] = child;
            else if (name.EndsWith("holder_rightarm"))
                partHolders[PartType.ArmRight] = child;
            else if (name.EndsWith("holder_leftarm"))
                partHolders[PartType.ArmLeft] = child;
            else if (name.EndsWith("holder_chest"))
                partHolders[PartType.Body] = child;
        }

        linkedPart = part;
        linkedPart.OnDamageTaken += ShowDamageFeedback;
        linkedPart.OnDestroyed += HandleDestruction;
    }
    public Transform GetHolderForPart(PartType type)
    {
        return partHolders.TryGetValue(type, out var t) ? t : null;
    }

    private void Update()
    {
        GetComponent<Animator>().SetFloat("Speed", linkedPart.owner.obj.GetComponent<CharacterController>().velocity.magnitude);
    }
    void ShowDamageFeedback(float damage)
    {
        // flash red, spawn sparks, etc
    }

    void HandleDestruction()
    {
        // play explosion, detach part, etc
    }
}
