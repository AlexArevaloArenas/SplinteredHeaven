using System.Collections.Generic;
using UnityEngine;

public class TargetTracker : MonoBehaviour
{

    public UnitManager CurrentTargetUnit { get; private set; }
    public UnitPart CurrentTargetPart { get; private set; }

    public void SetTarget(UnitManager unit, UnitPart part = null)
    {
        CurrentTargetUnit = unit;
        if(part == null)
        {
            CurrentTargetPart = unit.unit.Parts[0];
        }
        else
        {
            CurrentTargetPart = part;
        }
        
    }

    public void ClearTarget()
    {
        CurrentTargetUnit = null;
        CurrentTargetPart = null;
    }

    public bool HasValidTarget()
    {
        return CurrentTargetUnit != null;
    }
    /*
    public UnitManager FindClosestTargetInRange(WeaponModule weapon, List<UnitManager> candidates)
    {
        UnitManager best = null;
        float bestDistance = float.MaxValue;

        foreach (var candidate in candidates)
        {
            foreach (var part in candidate.unit.Parts)
            {
                if (!part.IsFunctional()) continue;
                if (!weapon.IsInRange(GetComponent<UnitManager>().unit, candidate.unit, part)) continue;

                float distance = Vector3.Distance(transform.position, part.GetHitPosition());
                if (distance < bestDistance)
                {
                    best = candidate;
                    bestDistance = distance;
                }
            }
        }

        return best;
    }
    */
}
