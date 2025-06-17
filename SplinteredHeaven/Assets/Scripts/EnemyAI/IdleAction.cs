using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/Idle Action")]
    public abstract class IdleAction : AIAction
    {
        public override void Execute(Context context)
        {
            foreach (var squad in context.squads)
            {
                foreach (var member in squad.members)
                {
                    if (member == null) continue; // Skip null members
                    member.GetComponent<UnitAI>().Stop(); // Stop the unit's current behavior
                }
            }
        }
    }
}