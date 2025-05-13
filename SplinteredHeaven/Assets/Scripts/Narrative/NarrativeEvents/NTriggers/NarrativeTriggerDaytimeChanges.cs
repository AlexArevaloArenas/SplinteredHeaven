using UnityEngine;

[CreateAssetMenu(fileName = "NarrativeTriggerDaytimeChanges", menuName = "Scriptable Objects/NarrativeTriggerDaytimeChanges")]
public class NarrativeTriggerDaytimeChanges : NarrativeEventTrigger
{
    public DayMoments triggerMoment; // The moment of the day that triggers the event
    public override void SetUp()
    {
        base.SetUp();
        // Subscribe to the OnDayTimeChanged event
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnDayTimeChanged += OnDayTimeChanged;
        }
        
    }
    public override void Close()
    {
        base.Close();
        // Unsubscribe from the event to avoid memory leaks
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnDayTimeChanged -= OnDayTimeChanged;
        }
        
    }

    private void OnDayTimeChanged(DayMoments newMoment)
    {
        // Check if the new moment matches the trigger condition
        if (newMoment == triggerMoment && isTriggered)
        {
            // Trigger the event
            NarrativeEventManager.Instance.TriggerEvent(narrativeEvent);
        }
    }
}
