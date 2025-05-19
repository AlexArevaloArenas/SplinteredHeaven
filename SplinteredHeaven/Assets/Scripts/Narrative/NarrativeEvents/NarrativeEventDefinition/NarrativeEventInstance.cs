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
        foreach (var next in definition.chainedEvents)
            context.narrativeEventManager.TriggerEvent(next);
    }
}
