using Unity.Behavior;
using UnityEngine;

[CreateAssetMenu(fileName = "PilotData", menuName = "Scriptable Objects/PilotData")]
public class PilotData : ScriptableObject
{
    public BehaviorGraph AttackBehaviour;
    public BehaviorGraph MoveBehaviour;
}
