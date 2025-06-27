using System.Linq;
using Unity.AppUI.UI;
using UnityEngine;

namespace UtilityAI {
    [CreateAssetMenu(menuName = "UtilityAI/Actions/WanderAction")]
    public class WanderAction : AIAction {

        public float wanderRadius = 10f;
        public override void Initialize(Context context) {
            context.sensor.targetTags.Add(targetTag);
        }

        public override void Execute(Context context) {

            if (context.characterController.velocity != Vector3.zero) return;

            var playerUnits = context.GetPlayerUnits();
            if (playerUnits == null || playerUnits.Length == 0) return;

            Transform closestPlayer = null;
            float closestDistance = float.MaxValue;

            foreach (var playerUnit in playerUnits)
            {
                if (playerUnit == null) continue;

                float distance = Vector3.Distance(context.ai.transform.position, playerUnit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = playerUnit.transform;
                }
            }

            if (closestPlayer == null) return;

            // Direction toward the closest player unit
            Vector3 direction = (closestPlayer.position - context.ai.transform.position).normalized;

            // Pick a point in that direction within wanderRadius
            Vector3 targetPosition = context.ai.transform.position + direction * Random.Range(wanderRadius * 0.5f, wanderRadius);

            context.ai.StartMoveBehaviour(targetPosition);
        }

        
    }
}