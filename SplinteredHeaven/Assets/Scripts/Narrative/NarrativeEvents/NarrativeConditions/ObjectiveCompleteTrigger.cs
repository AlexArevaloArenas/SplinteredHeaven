using UnityEngine;

[CreateAssetMenu(fileName = "ObjectiveCompleteTrigger", menuName = "Narrative/Trigger/ObjectiveCompleteTrigger")]
public class ObjectiveCompleteTrigger : NarrativeCondition
{
    public ObjectiveData objData; // The moment of the day that triggers the event
    private NarrativeEventInstance instance;

    public override void Initialize(NarrativeEventInstance instance, NarrativeContext context)
    {
        this.instance = instance;
        MissionEvents.OnObjectiveCompleted += OnObjectiveCompleted;
    }

    public override void Dispose()
    {
        MissionEvents.OnObjectiveCompleted -= OnObjectiveCompleted;
    }

    private void OnObjectiveCompleted(ObjectiveInstance objective)
    {
        if (objective.definition == objData)
        {
            instance.MarkTriggered();
        }
    }
}
