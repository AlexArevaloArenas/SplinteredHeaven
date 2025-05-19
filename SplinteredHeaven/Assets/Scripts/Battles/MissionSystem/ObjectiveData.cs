using UnityEngine;

[CreateAssetMenu(menuName = "Missions/Objective")]
public abstract class ObjectiveData : ScriptableObject
{
    public string objectiveID;
    public string description;
    public ObjectiveCondition condition;
}