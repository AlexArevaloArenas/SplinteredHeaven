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
            Vector3 pos = context.brain.PickRandomPoint(wanderRadius);

            context.ai.StartMoveBehaviour(pos);
        }

        
    }
}