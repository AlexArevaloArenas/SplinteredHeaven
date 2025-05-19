using UnityEngine;

[CreateAssetMenu(menuName = "Missions/Conditions/KillTarget")]
public class KillTargetCondition : ObjectiveCondition
{
    public string targetID;

    private ObjectiveInstance currentInstance;
    private ObjectiveContext context;

    public override void Initialize(ObjectiveContext context, ObjectiveInstance instance)
    {
        this.context = context;
        this.currentInstance = instance;
        MissionEvents.OnEnemyKilled += OnEnemyKilled;
    }

    public override void Dispose(ObjectiveContext context)
    {
        MissionEvents.OnEnemyKilled -= OnEnemyKilled;
    }

    private void OnEnemyKilled(string id)
    {
        if (id == targetID)
        {
            currentInstance.MarkComplete();
        }
    }
}
