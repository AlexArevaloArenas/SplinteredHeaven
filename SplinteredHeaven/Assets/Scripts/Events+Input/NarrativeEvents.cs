using System;

public static class NarrativeEvents
{
    public static Action<string> OnEnemyKilled;
    public static Action<string> OnItemCollected;
    public static Action<ObjectiveInstance> OnObjectiveCompleted;
    public static Action<ObjectiveInstance> OnObjectiveFailed;
}