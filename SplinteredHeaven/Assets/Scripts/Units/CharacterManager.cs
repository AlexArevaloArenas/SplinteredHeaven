using Pathfinding;
using Unity.Behavior;
using UnityEngine;

[RequireComponent(typeof(AIDestinationSetter))]
[RequireComponent(typeof(BehaviorGraphAgent))]
public class CharacterManager : UnitManager
{
    private AIDestinationSetter aiDestinationSetter;
    private BehaviorGraphAgent behaviorGraphAgent;
    public GameObject target;
    public PilotData pilot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start(); // Call the base class Start method if needed
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
        aiDestinationSetter.target = target.transform; // Set the target to the new GameObject
    }

    public override void Initialize()
    {
        unit = new Character(unitData as CharacterData, hpBar);
    }

    public void Stop()
    {
        aiDestinationSetter.target = null; // Clear the target to stop the AI from moving
        behaviorGraphAgent.Graph = null; // Clear the behavior graph
    }

    public void Attack(UnitManager unit)
    {

    }

    //Behaviour Trees IA Orders
    public void StartMoveBehaviour(Vector3 position)
    {
        //aiDestinationSetter.target.position = position; // Clear the target to stop the AI from moving
        behaviorGraphAgent.Graph = pilot.MoveBehaviour; // Clear the behavior graph
        behaviorGraphAgent.BlackboardReference.SetVariableValue("DestinationSetter", aiDestinationSetter);
        behaviorGraphAgent.BlackboardReference.SetVariableValue("Position", position);
        
        
    }

    public void StartAttackBehaviour(UnitManager target)
    {

    }

    private void OnEnable()
    {
        target = new GameObject(gameObject.name + " Target");
        target.transform.position = transform.position; // Set the target to the same position as the character

    }

    private void OnDisable()
    {
        target = null; // Set the target to null when the object is disabled
        Destroy(target);  
    }

}
