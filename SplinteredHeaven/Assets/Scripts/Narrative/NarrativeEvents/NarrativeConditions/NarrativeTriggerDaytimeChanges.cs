using UnityEngine;

[CreateAssetMenu(fileName = "NarrativeTriggerDaytimeChanges", menuName = "Narrative/Trigger/NarrativeTriggerDaytimeChanges")]
public class NarrativeTriggerDaytimeChanges : NarrativeCondition
{
    public DayMoments targetMoment; // The moment of the day that triggers the event
    private NarrativeEventInstance instance;
    public bool waitOneCycle = false; // If true, waits for one full cycle before triggering

    private bool wait = false; // Internal flag to track if we are waiting for one cycle

    public override void Initialize(NarrativeEventInstance instance, NarrativeContext context)
    {
        this.instance = instance;
        TimeManager.Instance.OnDayTimeChanged += OnDayTimeChanged;
        wait = waitOneCycle;


        // Optional: If already at target time, trigger instantly
        if (TimeManager.Instance.currentMoment == targetMoment)
        {
            if (waitOneCycle)
            {
                // If waiting for one cycle, we do not trigger immediately
                wait = false; // Reset the wait flag
                return;
            }
            instance.MarkTriggered();
        }
    }

    public override void Dispose()
    {
        TimeManager.Instance.OnDayTimeChanged -= OnDayTimeChanged;
    }

    private void OnDayTimeChanged(DayMoments newSegment)
    {
        if (wait)
        {
            wait = false; // Reset the wait flag after the first cycle
            return; // Do not trigger immediately
        }
        if (newSegment == targetMoment)
        {
            instance.MarkTriggered();
        }
    }
}
