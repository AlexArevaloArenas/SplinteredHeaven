using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityUtils; // Assuming this namespace contains the UnitAI extension method

namespace UtilityAI {
    public class Context {
        public Brain brain;
        public UnitAI ai;
        public Transform target;
        public AISensor sensor;
        public UnitManager unitManager;
        public CharacterController characterController;

        readonly Dictionary<string, object> data = new();

        public Context(Brain brain) {
            if(brain == null) {
                Debug.LogError("Brain cannot be null");
                return;
            }

            this.brain = brain;
            this.ai = brain.gameObject.GetOrAddComponent<UnitAI>();
            this.sensor = brain.gameObject.GetOrAddComponent<TargetTracker>();
            this.unitManager = brain.GetComponent<UnitManager>();
            this.characterController = brain.GetComponent<CharacterController>();

        }
        
        public T GetData<T>(string key) => data.TryGetValue(key, out var value) ? (T)value : default;
        public void SetData(string key, object value) => data[key] = value;
    }
}