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
        

        if (SelfCharacter.Value.unit.obj.transform.GetComponent<TargetTracker>().IsInRange(TargetCharacter.Value))
        {
            return Status.Success;
        }
        else
        {
            return Status.Failure;
        }

        /*
        // Check if the target is in range
        foreach (var part in SelfCharacter.Value.unit.Parts)
        {
            foreach(var weapon in part.Weapons)
            {
                if (TargetCharacter.Value == null || SelfCharacter.Value == null) return Status.Failure;
                else if (weapon.IsInRange(SelfCharacter.Value.transform.position, TargetCharacter.Value.transform.position))
                {
                    //Debug.Log($"Target {TargetCharacter.Value.name} is in range for weapon {weapon.name}");
                    return Status.Success;
                }
            }
        }
        return Status.Failure;
        */
    }

    protected override void OnEnd()
    {
    }
}

