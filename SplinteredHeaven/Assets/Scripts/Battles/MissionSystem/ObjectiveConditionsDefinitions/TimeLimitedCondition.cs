using UnityEngine;

[CreateAssetMenu(menuName = "Missions/Conditions/TimeLimit")]
public class TimeLimitedCondition : ObjectiveCondition
{
    public float timeLimit = 60f;

    private float timeRemaining;
    private ObjectiveInstance currentInstance;

    public override void Initialize(ObjectiveContext context, ObjectiveInstance instance)
    {
        currentInstance = instance;
        timeRemaining = timeLimit;
        context.OnUpdate += Tick;
    }

    public override void Dispose(ObjectiveContext context)
    {
        context.OnUpdate -= Tick;
    }

    private void Tick(float deltaTime)
    {
        if (currentInstance.isComplete) return;

        timeRemaining -= deltaTime;
        if (timeRemaining <= 0f)
        {
            currentInstance.MarkFailed(); // Optional: support failure state
        }
    }
}
