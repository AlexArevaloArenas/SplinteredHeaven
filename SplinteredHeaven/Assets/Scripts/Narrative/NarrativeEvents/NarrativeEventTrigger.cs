using UnityEngine;

[CreateAssetMenu(fileName = "NarrativeEventTrigger", menuName = "Scriptable Objects/NarrativeEventTrigger")]
public abstract class NarrativeEventTrigger : ScriptableObject
{
    public NarrativeEvent narrativeEvent;

    // Should be called regularly or hooked into game events
    public abstract void CheckTrigger(NarrativeContext context);
}
