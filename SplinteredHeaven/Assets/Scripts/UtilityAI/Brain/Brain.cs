using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace UtilityAI {
    [RequireComponent(typeof(NavMeshAgent), typeof(Sensor))]
    public class Brain : MonoBehaviour {
        public List<AIAction> actions;
        public Context context;

        public UnitManager unitManager;

        void Awake() {
            context = new Context(this);
            unitManager = GetComponent<UnitManager>();

            foreach (var action in actions) {
                action.Initialize(context);
            }
        }

        void Update() {
            UpdateContext();
            
            AIAction bestAction = null;
            float highestUtility = float.MinValue;

            foreach (var action in actions) {
                float utility = action.CalculateUtility(context);
                if (utility > highestUtility) {
                    highestUtility = utility;
                    bestAction = action;
                }
            }

            if (bestAction != null) {
                bestAction.Execute(context);
            }
        }

        void UpdateContext() {
            //context.SetData("health", health.normalizedHealth);
        }
    }
}