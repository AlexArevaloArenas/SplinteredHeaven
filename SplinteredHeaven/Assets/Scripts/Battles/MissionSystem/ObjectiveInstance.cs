using UnityEngine;
using System;

public class ObjectiveInstance {
    public ObjectiveData definition;
    public bool isComplete { get; private set; }
    public bool isFailed { get; private set; }

    private ObjectiveContext context;

    public ObjectiveInstance(ObjectiveData def, ObjectiveContext context)
    {
        definition = def;
        this.context = context;
        isComplete = false;
        isFailed = false;
        definition.condition.Initialize(context, this);
    }

    public void MarkComplete()
    {
        if (isComplete) return;
        isComplete = true;
        definition.condition.Dispose(context);
        MissionEvents.OnObjectiveCompleted?.Invoke(this);
    }

    public void MarkFailed()
    {
        if (isFailed) return;
        isFailed = true;
        definition.condition.Dispose(context);
        MissionEvents.OnObjectiveFailed?.Invoke(this);
    }
}

public class ObjectiveContext
{
    public event Action<float> OnUpdate;

    public void Tick(float deltaTime)
    {
        OnUpdate?.Invoke(deltaTime);
    }
}