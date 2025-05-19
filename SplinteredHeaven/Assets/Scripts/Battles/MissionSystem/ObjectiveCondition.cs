using UnityEngine;

public abstract class ObjectiveCondition : ScriptableObject
{
    public abstract void Initialize(ObjectiveContext context, ObjectiveInstance instance);
    public abstract void Dispose(ObjectiveContext context);
}
