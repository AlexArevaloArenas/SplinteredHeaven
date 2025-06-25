using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackAction", story: "We assign unit [TargetTracker] the [target] unit and part", category: "Action", id: "76063be58901d8463c0683720567408a")]
public partial class AttackAction : Action
{
    [SerializeReference] public BlackboardVariable<TargetTracker> TargetTracker;
    [SerializeReference] public BlackboardVariable<UnitManager> Target;

    protected override Status OnStart()
    {
        if (TargetTracker == null)
        {
            Debug.LogError("TargetTracker is null");
            return Status.Failure;
        }
        else if (Target == null)
        {
            Debug.LogError("Target is null");
            return Status.Failure;
        }
        else
        {
            TargetTracker.Value.SetTarget(Target.Value, null);
            //Debug.Log($"Target set to {Target.Value.name}");

        }
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

