using UnityEngine;

public abstract class NarrativeEventAction : ScriptableObject
{
    public abstract void Execute(NarrativeContext context);
}
