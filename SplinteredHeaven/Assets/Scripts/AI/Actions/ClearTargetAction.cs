using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ClearTargetAction", story: "Clear [TargetTracker]", category: "Action", id: "0d67606915b9cf803811c384297a6815")]
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

