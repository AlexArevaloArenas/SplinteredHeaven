using Ink.Parsed;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Missions/Conditions/KillTarget")]
public class KillTargetCondition : ObjectiveCondition
{
    public List<string> targetID;
    private int targetCount =0;
    private ObjectiveInstance currentInstance;
    private ObjectiveContext context;

    public override void Initialize(ObjectiveContext context, ObjectiveInstance instance)
    {
        targetCount = targetID.Count;
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
        Debug.Log($"Enemy killed with ID: {id}");
        if (targetID.Contains(id))
        {
            targetCount--;
        }
        if (targetCount== 0)
        {
            currentInstance.MarkComplete();
            Debug.Log("All targets killed, objective complete.");
        }
    }
}
