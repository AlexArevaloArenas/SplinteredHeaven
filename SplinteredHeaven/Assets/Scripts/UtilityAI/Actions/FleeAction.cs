using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace UtilityAI {
    [CreateAssetMenu(menuName = "UtilityAI/Actions/FleeAction")]
    public class FleeAction : AIAction {

        public float fleeIntensity = 15f;

        public override void Initialize(Context context) {
            context.sensor.targetTags.Add(targetTag);
        }

        public override void Execute(Context context) {
            Transform[] enemies = context.sensor.detectedObjects.Where(t => t.CompareTag(targetTag)).ToArray();
            Vector3 middleEnemyPoint = MiddlePoint(enemies);
            Vector3 oppositePoint = (context.brain.transform.position+(context.brain.transform.position - middleEnemyPoint)) * fleeIntensity;


            context.ai.StartMoveBehaviour(oppositePoint);
        }

        public Vector3 MiddlePoint(Transform[] list)
        {
            Vector3 sum = Vector3.zero;
            foreach(var point in list){
                sum += point.position;
            }
            Vector3 middlePoint = sum / list.Length;
            return middlePoint;
        }
    }
}