using System;
using System.Collections.Generic;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UtilityAI;

public class TargetTracker : AISensor // Inherits from Sensor to use its detection capabilities
{
    public UnitManager CurrentTargetUnit { get; private set; }
    public UnitPart CurrentTargetPart { get; private set; }

    public UnitManager PlayerTarget;

    public WeaponModuleInstance weaponWithMoreRange;

    [SerializeField] public float detectionInterval = 2.0f;
    private float detectionTimer = 0f;

    private void Awake()
    {
        if (tag == "Player")
        {
            targetTags.Add("Enemy"); // Add the tag for units to be detected
        }
        else if (tag == "Enemy")
        {
            targetTags.Add("Player"); // Add the tag for units to be detected
        }
    }

    private void Update()
    {

        if (detectionTimer > 0f)
        {
            detectionTimer -= Time.deltaTime;
            return;
        }
        detectionTimer = detectionInterval;

        if (PlayerTarget == null)
        {
            List<UnitManager> detectedTargets = new List<UnitManager>();
            foreach (var obj in detectedObjects)
            {
                if (obj.TryGetComponent(out UnitManager unit) && unit.gameObject != gameObject)
                {
                    detectedTargets.Add(unit);
                }
            }

            CurrentTargetUnit = FindClosestTargetInRange(detectedTargets.ToArray());
            if (CurrentTargetUnit == null)
            {
                //ClearTarget();
                CurrentTargetPart = null;
                return;
            }
            CurrentTargetPart = PartToAttack(CurrentTargetUnit);
        }
        else
        {
            CurrentTargetUnit = PlayerTarget;
            CurrentTargetPart = PartToAttack(CurrentTargetUnit);
        }
    }

    public void SetTarget(UnitManager unit, UnitPart part = null)
    {
        PlayerTarget = unit;
        CurrentTargetPart = PartToAttack(unit);
    }

    public void ClearTarget()
    {
        //CurrentTargetUnit = null;
        //CurrentTargetPart = null;
        PlayerTarget = null;
    }

    public bool HasValidTarget()
    {
        return CurrentTargetUnit != null;
    }

    public UnitManager FindClosestTargetInRange(UnitManager[] candidates)
    {
        if (weaponWithMoreRange == null)
        {
            Debug.LogWarning("No weapon with range set, cannot find targets.");
            return null;
        }

        //weaponWithMoreRange.GetRange()
        float closestDistance = float.MaxValue;
        UnitManager closestTarget = null;

        foreach (UnitManager candidate in candidates)
        {
            if (candidate.GetComponent<UnitManager>() == null || candidate.gameObject == gameObject || Vector3.Distance(transform.position, candidate.transform.position) > weaponWithMoreRange.GetRange()) continue;

            if (Vector3.Distance(transform.position, candidate.transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(transform.position, candidate.transform.position);
                closestTarget = candidate;
            }

        }

        if (IsTargetBehind(CurrentTargetUnit))
        {
            Debug.Log("Target is behind, clearing target.");
            return null;
        }

        if (IsTargetBehind(closestTarget))
        {
            return null;
        }

        return closestTarget;
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

}
