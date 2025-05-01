using Pathfinding;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToAction", story: "Move Agent To [Position] Using [Destination]", category: "Action", id: "bdec92014c3ae02a6926d7818e91d0a1")]
public partial class MoveToAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector3> Position;
    [SerializeReference] public BlackboardVariable<AIDestinationSetter> Destination;

    protected override Status OnStart()
    {
        if (Position != null)
        {
            Destination.Value.target.position = Position.Value;
            return Status.Success;
        }
        else
        {
            return Status.Failure;
        }
        //return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

