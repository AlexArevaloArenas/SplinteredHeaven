using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToAction", story: "[UnitMovements] moves unit to [position]", category: "Action", id: "ff152cee4f58fcb372bb0df998918c43")]
public partial class MoveToAction : Action
{
    [SerializeReference] public BlackboardVariable<UnitMovement> UnitMovements;
    [SerializeReference] public BlackboardVariable<Vector3> Position;

    protected override Status OnStart()
    {
        if (Position != null)
        {
            bool reach = UnitMovements.Value.MoveTo(Position.Value);

            if (reach)
            {
                return Status.Success;
            }
            else
            {
                return Status.Failure;
            }
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

