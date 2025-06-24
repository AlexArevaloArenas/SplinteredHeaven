using System.Collections.Generic;
using System.Linq;
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
            List<Transform> gameObjects = (List<Transform>)context.sensor.detectedObjects.Where(obj => obj.CompareTag(targetTag));
            Transform target;
            if (gameObjects.Count > 0)
            {
                target = gameObjects?.Random<Transform>();
            }
            else
            {
                return;
            }

            context.ai.StartMoveBehaviour(target.position);
        }
    }
}