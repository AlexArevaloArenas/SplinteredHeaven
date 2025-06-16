using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ClearTargetAction", story: "Clear [TargetTracker] target", category: "Action", id: "d7befe025dfe96396baa38cd80088387")]
public partial class ClearTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<TargetTracker> TargetTracker;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        TargetTracker.Value.ClearTarget();
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

