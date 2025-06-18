using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/PatrolAction")]
    public class PatrolAction : AIAction
    {
        public override void Initialize(Context context)
        {
            context.sensor.targetTags.Add(targetTag);
        }

        public override void Execute(Context context)
        {
            var target = context.sensor.GetClosestTarget(targetTag);
            if (target == null) return;

            //context.target = target;

            context.ai.StartMoveBehaviour(target.position);
        }
    }
}