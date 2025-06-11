using UnityEngine;

[CreateAssetMenu(fileName = "NarrativeTriggerDaytimeChanges", menuName = "Narrative/Trigger/NarrativeTriggerDaytimeChanges")]
public class NarrativeTriggerDaytimeChanges : NarrativeCondition
{
    public DayMoments targetMoment; // The moment of the day that triggers the event
    private NarrativeEventInstance instance;

    public override void Initialize(NarrativeEventInstance instance, NarrativeContext context)
    {
        this.instance = instance;
        TimeManager.Instance.OnDayTimeChanged += OnDayTimeChanged;

        // Optional: If already at target time, trigger instantly
        if (TimeManager.Instance.currentMoment == targetMoment)
        {
            instance.MarkTriggered();
        }
    }

    public override void Dispose()
    {
        TimeManager.Instance.OnDayTimeChanged -= OnDayTimeChanged;
    }

    private void OnDayTimeChanged(DayMoments newSegment)
    {
        if (newSegment == targetMoment)
        {
            instance.MarkTriggered();
        }
    }
}
