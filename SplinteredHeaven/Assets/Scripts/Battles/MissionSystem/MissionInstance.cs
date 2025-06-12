using System.Collections.Generic;
using System.Linq;

public class MissionInstance
{
    public List<ObjectiveInstance> objectives;
    private ObjectiveContext context;

    public MissionInstance(MissionData data)
    {
        context = new ObjectiveContext();
        objectives = data.objectives.Select(def => new ObjectiveInstance(def, context)).ToList();
    }

    public void Update(float deltaTime)
    {
        context.Tick(deltaTime);
    }

    public bool IsComplete => objectives.All(o => o.isComplete);
    public bool HasFailed => objectives.Any(o => o.isFailed);
}
