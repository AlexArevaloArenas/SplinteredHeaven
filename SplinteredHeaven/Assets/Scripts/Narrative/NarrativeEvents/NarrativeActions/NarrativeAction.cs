using UnityEngine;

public abstract class NarrativeAction : ScriptableObject
{
    public abstract void Execute(NarrativeContext context);
}
