using Ink.Parsed;
using UnityEngine;

public abstract class NarrativeCondition : ScriptableObject
{
    public abstract void Initialize(NarrativeEventInstance instance, NarrativeContext context);
    public abstract void Dispose();

}
