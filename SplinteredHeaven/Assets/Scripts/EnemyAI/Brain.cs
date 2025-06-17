using Ink.Parsed;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityAI
{
    public class Brain : MonoBehaviour
    {
        public List<AIAction> actions;
        public Context context;

        public List<UnitManager> units;
        public Dictionary<string, Squad> squadDictionary = new Dictionary<string, Squad>();

        private void Awake()
        {
            context = new Context(this);

            foreach (var action in actions)
            {
                action.Initialize(context);
            }
        }

        private void Start()
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
            units = new List<UnitManager>(gameObjects.Length);
            foreach (GameObject obj in gameObjects)
            {
                UnitManager unitManager = obj.GetComponent<UnitManager>();
                SquadTag squadTag = obj.GetComponent<SquadTag>();
                if (unitManager != null)
                {
                    units.Add(unitManager);
                }
                if (squadTag != null)
                {
                    if (!squadDictionary.ContainsKey(squadTag.squadName))
                    {
                        Squad squad = new Squad(new List<UnitManager> { unitManager }, squadTag.squadName, squadTag.squadTypes);
                        squadDictionary[squad.squadName] = squad;
                    }
                    else
                    {
                        // If the squad already exists, you add the unit to it
                        squadDictionary[squadTag.squadName].members.Add(unitManager);
                    }
                }
            }
        }

        public void Update()
        {
            UpdateContext();

            AIAction bestAction = null;
            float bestUtility = float.MinValue;

            foreach (var action in actions)
            {
                float utility = action.CalculateUtility(context);
                if (utility > bestUtility)
                {
                    bestUtility = utility;
                    bestAction = action;
                }
            }

            if (bestAction != null)
            {
                bestAction.Execute(context);
            }
        }

        public void UpdateContext()
        {
            // Update the context with current state, e.g., position, health, etc.
            // This is where you would gather data that your actions will use
            // For example, you might want to update the position of the brain or other relevant data



        }

    }
}

