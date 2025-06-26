using System.Linq;
using UnityEngine;
using UnityUtils;
using static UnityEngine.GraphicsBuffer;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/HuntEnemyAction")]
    public class HuntEnemyAction : AIAction
    {
        public override void Initialize(Context context)
        {
            context.sensor.targetTags.Add(targetTag);
        }

        public override void Execute(Context context)
        {
            //GameObject target = context.sensor.detectedObjects.Where(obj => obj.CompareTag(targetTag)).FirstOrDefault()?.gameObject;
            GameObject target = context.sensor.GetClosestTarget(targetTag).gameObject;

            //context.target = target;
            if(target == null)
            {
                Debug.LogWarning("No target found for HuntEnemyAction");
                return;
            }
            context.ai.StartAttackBehaviour(target);
        }
    }
}