using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FollowTarget", story: "[UnitMovement] follows [Target]", category: "Action", id: "26b37427e35af56768c1527ec68df373")]
public partial class FollowTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<UnitMovement> UnitMovement;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnStart()
    {
        //UnitMovement.Value.FollowTarget(Target.Value);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Target.Value == null)
        {
            return Status.Failure;
        }
        else
        {
            UnitMovement.Value.SetTarget(Target);
            return Status.Success;
        }

    }

    protected override void OnEnd()
    {
    }
}

