using Ink.Parsed;
using UnityEngine;

public class NarrativeEventTrigger : ScriptableObject
{
    public NarrativeEvent narrativeEvent;
    public bool isTriggered = false;

    public virtual void SetUp()
    {
        // Initialize the trigger if needed
        isTriggered = false; // Reset the trigger state
    }

    public virtual void Close()
    {
        // Clean up if needed
        isTriggered = false; // Reset the trigger state
    }

    // Should be called regularly or hooked into game events
    public void CheckTrigger(NarrativeContext context)
    {
        if (isTriggered)
        {
            // If the event has already been triggered, do nothing
            return;
        }
        isTriggered = true; // Reset the trigger state
    }

}
