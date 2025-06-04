using Unity.Behavior;
using UnityEngine;

[RequireComponent(typeof(BehaviorGraphAgent))]
public class UnitAI : MonoBehaviour
{
    private BehaviorGraphAgent agent;
    private PilotDataHolder pilotData;

    void Awake()
    {
        agent = GetComponent<BehaviorGraphAgent>();
        pilotData = GetComponent<PilotDataHolder>();
    }

    public void StartMoveBehaviour(Vector3 position)
    {
        agent.Graph = pilotData.Pilot.MoveBehaviour;
        agent.BlackboardReference.SetVariableValue("Position", position);
        agent.BlackboardReference.SetVariableValue("UnitMovement", GetComponent<UnitMovement>());

        /*
        behaviorGraphAgent.BlackboardReference.SetVariableValue("DestinationSetter", aiDestinationSetter);
        behaviorGraphAgent.BlackboardReference.SetVariableValue("Position", position);
        */
    }

    public void StartAttackBehaviour(GameObject target)
    {
        agent.Graph = pilotData.Pilot.AttackBehaviour;
        agent.BlackboardReference.SetVariableValue("TargetCharacter", target.GetComponent<UnitManager>());
        agent.BlackboardReference.SetVariableValue("TargetTracker", GetComponent<TargetTracker>());
        agent.BlackboardReference.SetVariableValue("UnitMovement", GetComponent<UnitMovement>());
        agent.BlackboardReference.SetVariableValue("Target", target);
    }

    public void Stop()
    {
        agent.Graph = null;
    }
}
