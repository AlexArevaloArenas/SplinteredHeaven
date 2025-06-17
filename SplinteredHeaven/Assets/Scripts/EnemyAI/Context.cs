using Ink.Parsed;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityAI
{
    public class Context
    {
        public Brain brain;
        public List<Squad> squads;
        readonly Dictionary<string, Transform> tacticPoints = new Dictionary<string, Transform>();

        public Context(Brain brain)
        {
            if(brain == null)
            {
                throw new System.ArgumentNullException(nameof(brain), "Brain cannot be null");
            }
            else
            {
                this.brain = brain;
            }
            tacticPoints = new Dictionary<string, Transform>();
        }

        public T GetData<T>(string key) => tacticPoints.TryGetValue(key, out Transform value) ? (T)(object)value : default;
        public void SetData<T>(string key, T value) => tacticPoints[key] = value as Transform;
    }
}
