
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Narrative/Actions/Prepare new events")]
public class PrepareNewEventsAction : NarrativeAction
{
    public List<NarrativeEventData> newEvents;

    public override void Execute(NarrativeContext context)
    {
        if (newEvents!=null)
        {
            context.narrativeEventManager.LoadNewEvents(newEvents);

        }
    }
}
