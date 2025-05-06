using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckRange", story: "[SelfCharacter] checks if [TargetCharacter] is in Range", category: "Action", id: "c821cc74cea9d752b6d2469129b0ad46")]
public partial class CheckRangeAction : Action
{
    [SerializeReference] public BlackboardVariable<UnitManager> SelfCharacter;
    [SerializeReference] public BlackboardVariable<UnitManager> TargetCharacter;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        // Check if the target is in range

        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

