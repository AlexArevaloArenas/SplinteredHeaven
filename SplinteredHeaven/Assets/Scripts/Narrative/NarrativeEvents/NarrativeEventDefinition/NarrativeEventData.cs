using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNarrativeEvent", menuName = "Narrative/NarrativeEvent")]
public class NarrativeEventData : ScriptableObject
{
    public string eventID;
    public NarrativeCondition triggerCondition;
    public List<NarrativeAction> actions;
    public List<NarrativeEventData> chainedEvents; // Next events
    public bool removeAfterExecution = false; // If true, this event will be removed after execution
}
