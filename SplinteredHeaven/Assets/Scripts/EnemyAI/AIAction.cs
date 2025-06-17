using UnityEngine;

namespace UtilityAI
{
    public abstract class AIAction : ScriptableObject
    {
        public string targetTag;
        public Consideration consideration;

        public virtual void Initialize(Context context)
        {
            //Initialization logic can be added here
        }

        public float CalculateUtility(Context context) => consideration.Evaluate(context);

        public abstract void Execute(Context context);
    }
}