using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackAction", story: "[SelfCharacter] attacks with all each available weapons to the [TargetCharacter]", category: "Action", id: "4f2500ff2faad088227e29957730d0d6")]
public partial class AttackAction : Action
{
    [SerializeReference] public BlackboardVariable<CharacterManager> SelfCharacter;
    [SerializeReference] public BlackboardVariable<UnitManager> TargetCharacter;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

