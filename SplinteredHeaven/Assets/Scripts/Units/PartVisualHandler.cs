using System.Collections.Generic;
using UnityEngine;

public class PartVisualHandler : MonoBehaviour
{
    public string partId; // e.g., "LeftArm"
    public UnitPart linkedPart;

    public List<Transform> moduleHolders;

    public void Initialize(UnitPart part)
    {
        linkedPart = part;
        linkedPart.OnDamageTaken += ShowDamageFeedback;
        linkedPart.OnDestroyed += HandleDestruction;
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
