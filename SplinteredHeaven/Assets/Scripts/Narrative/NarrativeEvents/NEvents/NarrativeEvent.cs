using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNarrativeEvent", menuName = "Narrative/NarrativeEvent")]
public class NarrativeEvent : ScriptableObject
{
    public string eventName;
    public List<NarrativeEventAction> actions;

    public virtual void Execute(NarrativeContext context)
    {
        foreach (var action in actions)
        {
            action.Execute(context);
        }
    }
}
