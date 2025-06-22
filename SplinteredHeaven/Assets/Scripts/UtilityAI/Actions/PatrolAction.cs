using UnityEngine;
using UnityUtils;

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
            if (context.characterController.velocity != Vector3.zero) return;
            Transform target;
            if (context.sensor.detectedObjects.Count > 0)
            {
                target = context.sensor.detectedObjects?.Random<Transform>();
            }
            else
            {
                return;
            }

            //context.target = target;

            context.ai.StartMoveBehaviour(target.position);
        }
    }
}