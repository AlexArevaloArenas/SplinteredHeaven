using Unity.Behavior;
using Unity.VisualScripting;
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

        agent.Graph = Instantiate(pilotData.Pilot.MoveBehaviour);
        agent.BlackboardReference.SetVariableValue("Position", position);
        agent.BlackboardReference.SetVariableValue("UnitMovement", GetComponent<UnitMovement>());
        agent.BlackboardReference.SetVariableValue("TargetTracker", GetComponent<TargetTracker>());
        /*
        behaviorGraphAgent.BlackboardReference.SetVariableValue("DestinationSetter", aiDestinationSetter);
        behaviorGraphAgent.BlackboardReference.SetVariableValue("Position", position);
        */
    }

    public void StartAttackBehaviour(GameObject target)
    {
        agent.Graph = Instantiate(pilotData.Pilot.AttackBehaviour);
        agent.BlackboardReference.SetVariableValue("TargetCharacter", target.GetComponent<UnitManager>());
        agent.BlackboardReference.SetVariableValue("TargetTracker", GetComponent<TargetTracker>());
        agent.BlackboardReference.SetVariableValue("UnitMovement", GetComponent<UnitMovement>());
        agent.BlackboardReference.SetVariableValue("Target", target);
    }

    public void Stop()
    {
        if (agent.Graph != null)
        {
            agent.Restart();
            agent.Update();
        }
        
        agent.Graph = null;
        
        
    }
}
