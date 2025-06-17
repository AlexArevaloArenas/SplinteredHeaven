using UnityEngine;



namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAi/Considerations/Constant")]
    public abstract class ConstantConsideration : Consideration
    {
        public float value;
        public override float Evaluate(Context context) => value;
    }
}