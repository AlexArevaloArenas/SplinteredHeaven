using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Stop", story: "Sets [UnitMovement] to stop", category: "Action", id: "0210fa972c3e1503bf07e55c436e7689")]
public partial class StopAction : Action
{
    [SerializeReference] public BlackboardVariable<UnitMovement> UnitMovement;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (UnitMovement == null || UnitMovement.Value == null)
        {
            Debug.LogError("UnitMovement is not set or is null.");
            return Status.Failure;
        }
        UnitMovement.Value.Stop();
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

