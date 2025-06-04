using System.Collections.Generic;
using Unity.AppUI.UI;
using UnityEngine;

public class TargetTracker : MonoBehaviour
{
    //public float DetectionRange = 100f; // Range within which targets can be detected
    public UnitManager CurrentTargetUnit { get; private set; }
    public UnitPart CurrentTargetPart { get; private set; }
    public WeaponModuleInstance weaponWithMoreRange;

    [SerializeField] private float detectionInterval = 2.0f;
    private float detectionTimer = 0f;

    /*
    private void Start()
    {
        foreach (UnitPart part in GetComponent<UnitManager>().unit.Parts)
        {
            if (part == null) continue; // Skip null parts
            if (part.Weapons == null || part.Weapons.Count == 0) continue; // Skip parts without weapons
            foreach (var weapon in part.Weapons)
            {
                if (weapon.GetRange() > weaponWithMoreRange.GetRange())
                {
                    weaponWithMoreRange = weapon;
                }
            }
        }
    }
    */
    private void Update()
    {
        if (detectionTimer > 0f)
        {
            detectionTimer -= Time.deltaTime;
            return;
        }
        detectionTimer = detectionInterval;

        CurrentTargetUnit = FindClosestTargetInRange();
        if (CurrentTargetUnit == null)
        {
            ClearTarget();
            return;
        }
        CurrentTargetPart = PartToAttack(CurrentTargetUnit);
        

    }

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
    
    public UnitManager FindClosestTargetInRange()
    {
        if(weaponWithMoreRange == null)
        {
            Debug.LogWarning("No weapon with range set, cannot find targets.");
            return null;
        }
        //List<UnitManager> candidates = new List<UnitManager>();
        float closestDistance = float.MaxValue;
        UnitManager closestTarget = null;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, weaponWithMoreRange.GetRange(), LayerMask.GetMask("Selectable"));
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider == null || hitCollider.GetComponent<UnitManager>() == null || hitCollider.gameObject == gameObject) continue;
            UnitManager unitManager = hitCollider.GetComponent<UnitManager>();
            if(Vector3.Distance(transform.position, unitManager.transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(transform.position, unitManager.transform.position);
                closestTarget = unitManager;
            }
            //candidates.Add(hitCollider.GetComponent<UnitManager>());
        }

        if(IsTargetBehind(CurrentTargetUnit)){
            Debug.Log("Target is behind, clearing target.");
            return null;
        }

        if(IsTargetBehind(closestTarget))
        {
            return null;
        }

        return closestTarget;

        /*
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
        */
    }

    public UnitPart PartToAttack(UnitManager targetUnit)
    {
        if (targetUnit == null || targetUnit.unit.Parts.Count == 0) return null;
        // Find the first functional part
        foreach (UnitPart part in targetUnit.unit.Parts)
        {
            if (!part.IsDestroyed())
            {
                return part;
            }
        }
        CurrentTargetUnit = null;
        return null;
    }

    public bool IsTargetBehind(UnitManager target)
    {
        if (target == null) return false;

        Vector3 toTarget = target.transform.position - transform.position;
        toTarget.y = 0; // Ignore height

        Vector3 forward = transform.forward;
        forward.y = 0;

        float dot = Vector3.Dot(forward.normalized, toTarget.normalized);
        return dot < 0.25f; // Adjust threshold as needed (0 = directly behind, 1 = in front)
    }

    public void OnDrawGizmosSelected()
    {
        if (weaponWithMoreRange != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, weaponWithMoreRange.GetRange());
        }
    }

}
