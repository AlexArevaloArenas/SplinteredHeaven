using System;

public class NarrativeEventInstance
{
    public NarrativeEventData definition;
    public bool triggered = false;
    private NarrativeContext context;

    public NarrativeEventInstance(NarrativeEventData def, NarrativeContext ctx)
    {
        definition = def;
        context = ctx;
        def.triggerCondition?.Initialize(this, context);
    }

    public void MarkTriggered()
    {
        if (triggered) return;
        triggered = true;

        // Execute all actions
        foreach (var action in definition.actions)
            action.Execute(context);

        // Chain next events
        context.narrativeEventManager.LoadNewEvents(definition.chainedEvents);


        if (definition.removeAfterExecution)
        {
            context.narrativeEventManager.activeInstances.Remove(definition.eventID);
            context.narrativeEventManager.allEvents.Remove(definition);
        }
    }
}
